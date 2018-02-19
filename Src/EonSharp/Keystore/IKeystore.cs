using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Keystore
{
	public interface IKeystore
	{
		string Ciphertext { get; set; }
		string Mac { get; set; }

		ICrypto Crypto { get; set; }
		IKdf Kdf { get; set; }


		byte[] GetPrivateKey(string password);


	}
}
