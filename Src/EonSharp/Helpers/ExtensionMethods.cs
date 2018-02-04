using System;

namespace EonSharp.Helpers
{
	public static class ExtensionMethods
	{
		public static string ToHexString(this byte[] array)
		{
			return HexHelper.ArrayToHexString(array);
		}

		public static byte[] FromHexStringToByteArray(this string hexString)
		{
			return HexHelper.HexStringToByteArray(hexString);
		}

	}
}
