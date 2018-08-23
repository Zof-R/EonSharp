using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Keystore.Crypto;
using EonSharp.Keystore.Kdf;
using System.Runtime.Serialization;

namespace EonSharp.Keystore
{
	public class KeystoreV1 : IKeystore, ISerializable
	{

		public KeystoreV1()
		{

		}
		public KeystoreV1(byte[] privatekey, string password, KdfTypes kdftype = KdfTypes.Pbkdf2)
		{
			switch (kdftype)
			{
				case KdfTypes.Argon2:
					throw new NotImplementedException();
				case KdfTypes.Pbkdf2:
					Kdf = new Pbkdf2();
					break;
				default:
					throw new Exception("Selected Key Derivation Function not supported");
			}

			Crypto = new Aes128Ctr();

			var cypherkey = ComputeCypherKey(password);
			var cyphertxtarray = Crypto.Encrypt(privatekey, cypherkey);

			Ciphertext = Helpers.HexHelper.ArrayToHexString(cyphertxtarray);

			Mac = ComputeMac(cyphertxtarray, password, Kdf);
		}


		#region IKeystore

		/// <summary>
		/// the encrypted seed
		/// </summary>
		public string Ciphertext { get; set; }

		/// <summary>
		/// message authentication code
		/// </summary>
		public string Mac { get; set; }

		/// <summary>
		/// Encryption parameters
		/// </summary>
		public ICrypto Crypto { get; set; }

		/// <summary>
		/// Key derivation function parameters
		/// </summary>
		public IKdf Kdf { get; set; }

		public byte[] GetPrivateKey(string password)
		{
			if (ValidatePassword(password))
			{
				var cypherkey = ComputeCypherKey(password);
				return Crypto.Decrypt(Helpers.HexHelper.HexStringToByteArray(Ciphertext), cypherkey);
			}
			throw new Exception("Password mismatch");
		}
		public string EncryptMessage(string message, string password)
		{
			var cypherkey = ComputeCypherKey(password);
			var messagearray = Crypto.Encrypt(UTF8Encoding.UTF8.GetBytes(message), cypherkey);
			return Helpers.HexHelper.ArrayToHexString(messagearray);
		}
		public string DecryptMessage(string encryptedMessage, string password)
		{
			var cypherkey = ComputeCypherKey(password);
			var messagearray = Crypto.Decrypt(Helpers.HexHelper.HexStringToByteArray(encryptedMessage), cypherkey);
			return UTF8Encoding.UTF8.GetString(messagearray);
		}

		#endregion
		#region ISerializable

		public KeystoreV1(SerializationInfo info, StreamingContext context)
		{
			Ciphertext = info.GetString("ciphertext");
			Mac = info.GetString("mac");
			Crypto = info.GetValue("crypto", typeof(Aes128Ctr)) as ICrypto;

			if (info.GetValue("kdf", typeof(Newtonsoft.Json.Linq.JObject)) is Newtonsoft.Json.Linq.JObject jskdf)
			{
				switch (jskdf["function"].ToObject<KdfTypes>())
				{
					case KdfTypes.Argon2:
						throw new NotImplementedException();
					case KdfTypes.Pbkdf2:
						Kdf = jskdf.ToObject<Pbkdf2>();
						break;
				}
			}
			else if (info.GetValue("kdf", typeof(Pbkdf2)) is Pbkdf2 pbkdf2)
			{
				Kdf = pbkdf2;
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ciphertext", Ciphertext);
			info.AddValue("mac", Mac);
			info.AddValue("crypto", Crypto);
			info.AddValue("kdf", Kdf);
		}

		#endregion


		byte[] ComputeCypherKey(string password)
		{
			var dk = Kdf.ComputeDerivedKey(password);
			var authkey = new byte[16];
			Buffer.BlockCopy(dk, 0, authkey, 0, 16);
			return authkey;
		}
		string ComputeMac(byte[] ciphertext, string password, IKdf kdf)
		{
			var dk = kdf.ComputeDerivedKey(password);

			var concat = new byte[16 + ciphertext.Length];
			Buffer.BlockCopy(dk, dk.Length - 16, concat, 0, 16);
			Buffer.BlockCopy(ciphertext, 0, concat, 16, ciphertext.Length);

			using (var hs = SecurityDriven.Inferno.Hash.HashFactories.SHA256())
			{
				return Helpers.HexHelper.ArrayToHexString(hs.ComputeHash(concat));
			}
		}
		bool ValidatePassword(string password)
		{
			var mac = ComputeMac(Helpers.HexHelper.HexStringToByteArray(Ciphertext), password, Kdf);
			return mac.Equals(Mac, StringComparison.InvariantCultureIgnoreCase);
		}



	}
}
