using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Helpers;

namespace EonSharp.Generators
{
	public static class SeedGenerator
	{
		public static byte[] NewSeed()
		{
			RandomNumberGenerator csprng = new RNGCryptoServiceProvider();
			byte[] seedBytes = new byte[32];
			csprng.GetBytes(seedBytes);
			return seedBytes;
		}

		public static string NewSeedAsString()
		{
			return NewSeed().ToHexString();
		}
	}
}
