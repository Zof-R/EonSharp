using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Helpers;

namespace EonSharp.Generators
{
	public class AccountGenerator : KeyPairGenerator
	{




		public long AccountNumber { get; set; }
		public string AccountId { get; set; }

		public AccountGenerator() : base()
		{
			Initialize();
		}
		public AccountGenerator(byte[] seed) : base(seed)
		{
			Initialize();
		}

		void Initialize()
		{
			AccountNumber = Providers.IdProvider.ComputeAccountNumber(PublicKey);
			AccountId = Providers.IdProvider.ComputeID(AccountNumber, Providers.IdProvider.IdType.Account);
		}

		public string PrivateKeyToString()
		{
			return PrivateKey.ToHexString();
		}
		public string ExpandedPrivateKeyToString()
		{
			return ExpandedPrivateKey.ToHexString();
		}
		public string PublicKeyToString()
		{
			return PublicKey.ToHexString();
		}
	}
}
