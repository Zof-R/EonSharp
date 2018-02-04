using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Providers
{
	public static class IdProvider
	{
		const string ALPHABET = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
		static readonly BigInteger TWO64 = BigInteger.Parse("18446744073709551616");
		static readonly BigInteger MASK_B74 = BigInteger.Parse("18889465931478580854784");
		static readonly BigInteger AND_VAL = 0x3FF;
		static readonly BigInteger AND_VAL2 = 0x1F;
		public static class IdType
		{
			public const string Account = "EON";
			public const string Transaction = "EON-T";
			public const string Block = "EON-B";
			public const string ColoredCoin = "EON-C";
		}

		public static long ComputeAccountNumber(byte[] publicKey)
		{
			byte[] hashBytes;
			using (SHA512 shaM = new SHA512Managed())
			{
				hashBytes = shaM.ComputeHash(publicKey);
			}

			//swap to big endian
			//Array.Reverse(hashBytes);

			var bigInteger = new BigInteger(0);
			for (int i = 0; i < hashBytes.Length; i += 8)
			{
				//var bi = new BigInteger(new byte[] { hashBytes[i + 7], hashBytes[i + 6], hashBytes[i + 5], hashBytes[i + 4], hashBytes[i + 3], hashBytes[i + 2], hashBytes[i + 1], hashBytes[i] });
				var bi = new BigInteger(new byte[] { hashBytes[i], hashBytes[i + 1], hashBytes[i + 2], hashBytes[i + 3], hashBytes[i + 4], hashBytes[i + 5], hashBytes[i + 6], hashBytes[i + 7] });
				bigInteger = bigInteger ^ bi;
			}
			return (long)bigInteger;
		}



		public static long ComputeTransactionNumber(byte[] signature, int timestamp)
		{
			//TODO: create static var with endianess detected by BitConverter.IsLittleEndian
			//and choose algorithimc methods during runtime accordingly //Array.Reverse(hash);

			byte[] hash;
			using (SHA512 shaM = new SHA512Managed())
			{
				hash = shaM.ComputeHash(signature);
			}

			var bigInteger = new BigInteger(0);
			for (int i = 0; i < hash.Length; i += 4)
			{
				var bi = new BigInteger(new byte[] { hash[i], hash[i + 1], hash[i + 2], hash[i + 3] });
				bigInteger = bigInteger ^ bi;
			}
			return ((long)(int)bigInteger << 32) | ((long)timestamp & 0xFFFFFFFL);
		}



		public static string ComputeID(long accountId, string prefix = IdType.Account)
		{
			var id = new BigInteger(accountId);
			if (accountId < 0)
			{
				id = BigInteger.Add(id, TWO64);
			}

			BigInteger chs = BigInteger.Zero;
			BigInteger tmp = id;

			while (tmp > BigInteger.Zero)
			{
				chs = chs ^ (tmp & AND_VAL);
				tmp = tmp >> 10;
			}

			id = id | (chs << 64);
			id = id | MASK_B74;

			var idStr = new StringBuilder(prefix);
			for (int i = 0; i < 15; i++)
			{
				if ((i % 5) == 0)
				{
					idStr.Append('-');
				}
				idStr.Append(ALPHABET[(int)(id & AND_VAL2)]);
				id = id >> 5;
			}
			return idStr.ToString();
		}







	}
}
