using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Helpers;

namespace EonSharp.Keystore.Kdf
{
	[Serializable]
	public class Pbkdf2 : IKdf
	{
		public Pbkdf2()
		{
		}


		public string Function { get; set; } = "pbkdf2";

		public class ParametersClass
		{
			public string Prf { get; set; } = "hmac-sha384";
			public int I { get; set; } = 131072;
			public string Salt { get; set; } = null;
			public int Dklen { get; set; } = 32;
		}
		public ParametersClass Parameters { get; set; } = new ParametersClass();

		public byte[] ComputeDerivedKey(string password)
		{
			Initialize(null);

			if (Parameters.Prf == "hmac-sha384")
			{
				using (var pbkdf2 = new SecurityDriven.Inferno.Kdf.PBKDF2(SecurityDriven.Inferno.Mac.HMACFactories.HMACSHA384, password, Parameters.Salt.FromHexStringToByteArray(), Parameters.I))
				{
					return pbkdf2.GetBytes(Parameters.Dklen);
				}
			}

			throw new Exception("Selected Pseudo Random Function not supported");
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
