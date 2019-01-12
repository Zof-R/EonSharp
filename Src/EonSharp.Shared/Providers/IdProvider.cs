using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Providers
{
	public static class IdProvider
	{
		const string ALPHABET = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
		//0x10000000000000000 = 0xFFFFFFFFFFFFFFFF + 1
		static readonly BigInteger TWO64 = BigInteger.Parse("18446744073709551616");
		static readonly BigInteger MASK_B74 = BigInteger.Parse("18889465931478580854784");
		static readonly BigInteger AND_VAL = 0x3FF;
		static readonly BigInteger AND_VAL2 = 0x1F;
		static readonly int ID_LEN = 18;
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
			return ((long)(int)bigInteger << 32) | ((long)timestamp & 0xFFFFFFFFL);
		}



		public static string ComputeID(long id, string prefix = IdType.Account)
		{
			var biid = new BigInteger(id);
			if (id < 0)
			{
				biid = BigInteger.Add(biid, TWO64);
			}

			BigInteger chs = BigInteger.Zero;
			BigInteger tmp = biid;

			while (tmp > BigInteger.Zero)
			{
				chs = chs ^ (tmp & AND_VAL);
				tmp = tmp >> 10;
			}

			biid = biid | (chs << 64);
			biid = biid | MASK_B74;

			var idStr = new StringBuilder(prefix);
			for (int i = 0; i < 15; i++)
			{
				if ((i % 5) == 0)
				{
					idStr.Append('-');
				}
				idStr.Append(ALPHABET[(int)(biid & AND_VAL2)]);
				biid = biid >> 5;
			}
			return idStr.ToString();
		}

		public static long ParseID(string id, string prefix = IdType.Account)
		{
			id = id.Trim().ToUpper();
			if (id.Length != ID_LEN + prefix.Length)
			{
				throw new Exception($"Malformed id string {id}");
			}

			var biid = BigInteger.Zero;
			for (int i = id.Length - 1; i > prefix.Length; i--)
			{
				var p = ALPHABET.IndexOf(id[i]);
				if (p >= 0)
				{
					biid = biid << 5;
					biid = biid + p;
				}
			}

			////NOT NEEDED IN C# IMPLEMENTATION
			//for (int i = 64; i < 75; i++)
			//{
			//	biid = biid & ~(1 << i);
			//}

			var res = BitConverter.ToInt64(biid.ToByteArray(), 0);
			if (id != (ComputeID(res, prefix)))
			{
				throw new Exception($"Error parsing {id}");
			}

			return res;
		}


		public static byte[] ParsePublicKey(long id)
		{
			return new BigInteger(id).ToByteArray();
		}
		public static byte[] ParsePublicKey(string id, string prefix = IdType.Account)
		{
			return new BigInteger(ParseID(id, prefix)).ToByteArray();
		}


	}
}
