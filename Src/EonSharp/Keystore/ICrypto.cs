using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Keystore
{
	public interface ICrypto
	{
		/// <summary>
		/// defaults to 'aes-128-ctr'
		/// </summary>
		string Cypher { get; set; }

		byte[] Decrypt(byte[] message, byte[] key);
		byte[] Encrypt(byte[] message, byte[] key);
	}
}
