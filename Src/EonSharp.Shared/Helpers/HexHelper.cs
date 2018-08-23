using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Helpers
{
	public static class HexHelper
	{

		public static string ArrayToHexString(byte[] array)
		{
			return BitConverter.ToString(array).Replace("-", "");
		}

		public static byte[] HexStringToByteArray(string hexString)
		{
			if (hexString == null)
			{
				throw new NullReferenceException("hexString cannot be null");
			}
			var res = new byte[hexString.Length / 2];
			for (int i = 0; i < res.Length; i++)
			{
				res[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			}
			return res;
		}
	}
}
