using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Generators
{
	public class KeyPairGenerator
	{
		public byte[] PrivateKey { get; private set; }
		public byte[] ExpandedPrivateKey { get; private set; }
		public byte[] PublicKey { get; private set; }

		


		public KeyPairGenerator() : this(SeedGenerator.NewSeed())
		{
		}
		public KeyPairGenerator(byte[] seed)
		{
			this.PrivateKey = seed;
			GeneratePublicKey(seed);
		}


		void GeneratePublicKey(byte[] seed)
		{
			//Ed25519 implementation as per API doc (NaCl.sign.keyPair.fromSeed)

			byte[] publicKey, expandedPrivateKey;
			Chaos.NaCl.Ed25519.KeyPairFromSeed(out publicKey, out expandedPrivateKey, seed);

			this.ExpandedPrivateKey = expandedPrivateKey;
			this.PublicKey = publicKey;
		}


	}
}
