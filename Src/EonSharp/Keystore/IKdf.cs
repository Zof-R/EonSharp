using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Keystore
{
	public interface IKdf
	{
		/// <summary>
		/// defaults to 'pbkdf2'
		/// </summary>
		string Function { get; set; }

		byte[] ComputeDerivedKey(string password);
	}
}
