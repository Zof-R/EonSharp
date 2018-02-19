using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Helpers;

namespace EonSharp.Generators
{
	public class PublicKeyPairGenerator
	{
		public byte[] PublicKeyArray { get; private set; }
		public string PublicKey { get; private set; }




		public PublicKeyPairGenerator() : this(SeedGenerator.NewSeed())
		{
		}
		public PublicKeyPairGenerator(byte[] seed)
		{
			GeneratePublicKey(seed);
		}


		/// <summary>
		/// Ed25519 implementation as per API doc (NaCl.sign.keyPair.fromSeed)
		/// </summary>
		/// <param name="seed"></param>
		void GeneratePublicKey(byte[] seed)
		{
			Chaos.NaCl.Ed25519.KeyPairFromSeed(out byte[] publicKey, out byte[] expandedPrivateKey, seed);

			this.PublicKeyArray =  publicKey;
			this.PublicKey =  publicKey.ToHexString();
		}

		public static byte[] ComputeExpandedPrivateKey(byte[] privatekey)
		{
			var sec = new PublicKeyPairGenerator(privatekey);
			var buffer = new byte[privatekey.Length + sec.PublicKeyArray.Length];
			Buffer.BlockCopy(privatekey, 0, buffer, 0, privatekey.Length);
			Buffer.BlockCopy(sec.PublicKeyArray, 0, buffer, privatekey.Length, sec.PublicKeyArray.Length);
			return buffer;
		}
	}
}
