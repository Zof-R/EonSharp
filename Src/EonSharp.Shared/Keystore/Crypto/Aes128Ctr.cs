using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Helpers;
using System.IO;

namespace EonSharp.Keystore.Crypto
{
	[Serializable]
	public class Aes128Ctr : ICrypto
	{

		public Aes128Ctr()
		{
		}


		public string Cypher { get; set; } = "aes-128-ctr";

		public class ParametersClass
		{
			public string Iv { get; set; } = null;
		}
		public ParametersClass Parameters { get; set; } = new ParametersClass();

		/// <summary>
		/// Key size must be 128bits
		/// </summary>
		/// <param name="cyphermessage"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public byte[] Decrypt(byte[] cyphermessage, byte[] key)
		{
			using (var trgt = new MemoryStream())
			using (var src = new MemoryStream(cyphermessage))
			using (var transfr = new SecurityDriven.Inferno.Cipher.AesCtrCryptoTransform(key, new ArraySegment<byte>(Parameters.Iv.FromHexStringToByteArray())))
			using (var cs = new System.Security.Cryptography.CryptoStream(src, transfr, System.Security.Cryptography.CryptoStreamMode.Read))
			{
				cs.CopyTo(trgt);
				return trgt.ToArray();
			}
		}
		/// <summary>
		/// Key size must be 128bits
		/// </summary>
		/// <param name="message"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public byte[] Encrypt(byte[] message, byte[] key)
		{
			Initialize(null);

			using (var ms = new MemoryStream())
			using (var trasnfr = new SecurityDriven.Inferno.Cipher.AesCtrCryptoTransform(key, new ArraySegment<byte>(Parameters.Iv.FromHexStringToByteArray())))
			using (var cs = new System.Security.Cryptography.CryptoStream(ms, trasnfr, System.Security.Cryptography.CryptoStreamMode.Write))
			{
				cs.Write(message, 0, message.Length);
				return ms.ToArray();
			}
		}


		public void Initialize(byte[] iv)
		{
			if (iv != null)
			{
				Parameters.Iv = iv.ToHexString();
			}
			else if (Parameters.Iv == null)
			{
				byte[] saltbuffer = new byte[16];
				using (var rng = System.Security.Cryptography.RNGCryptoServiceProvider.Create())
				{
					rng.GetBytes(saltbuffer);
					Parameters.Iv = saltbuffer.ToHexString();
				}
			}
		}


	}
}
