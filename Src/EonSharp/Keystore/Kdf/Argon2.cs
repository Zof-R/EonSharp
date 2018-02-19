using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Keystore.ExtensionMethods;
using EonSharp.Helpers;
using Konscious.Security.Cryptography;

namespace EonSharp.Keystore.Kdf
{
	[Serializable]
	public class Argon2 : IKdf
	{
		public Argon2()
		{
		}


		public string Function { get; set; } = "argon2d";

		public class ParametersClass
		{
			public int P { get; set; } = 16;
			public int Msize { get; set; } = 8192;
			public int I { get; set; } = 40;
			public string Salt { get; set; } = null;
			public int Dklen { get; set; } = 32;
		}
		public ParametersClass Parameters { get; set; } = new ParametersClass();

		public byte[] ComputeDerivedKey(string password)
		{
			Initialize(null);

			using (var argon2 = new Argon2i(Encoding.UTF8.GetBytes(password))
			{
				DegreeOfParallelism = Parameters.P,
				MemorySize = Parameters.Msize,
				Iterations = Parameters.I,
				Salt = Parameters.Salt.FromHexStringToByteArray()
			})
			{
				return argon2.GetBytes(Parameters.Dklen);
			}
		}


		public void Initialize(byte[] salt)
		{
			if (salt != null)
			{
				Parameters.Salt = salt.ToHexString();
			}
			else if (Parameters.Salt == null)
			{
				byte[] saltbuffer = new byte[16];
				using (var rng = System.Security.Cryptography.RNGCryptoServiceProvider.Create())
				{
					rng.GetBytes(saltbuffer);
					Parameters.Salt = saltbuffer.ToHexString();
				}
			}
		}


	}
}
