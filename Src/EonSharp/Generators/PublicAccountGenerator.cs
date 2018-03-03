using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Helpers;

namespace EonSharp.Generators
{
	public class PublicAccountGenerator : PublicKeyPairGenerator, ISerializable
	{

		public long AccountNumber { get; set; }
		public string AccountId { get; set; }

		public PublicAccountGenerator() : base()
		{
			Initialize();
		}
		public PublicAccountGenerator(byte[] seed) : base(seed)
		{
			Initialize();
		}

		void Initialize()
		{
			AccountNumber = Providers.IdProvider.ComputeAccountNumber(PublicKeyArray);
			AccountId = Providers.IdProvider.ComputeID(AccountNumber, Providers.IdProvider.IdType.Account);
		}

		#region ISerializable

		public PublicAccountGenerator(SerializationInfo info, StreamingContext context) : base(null)
		{
			AccountId = info.GetString("accountid");
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("accountid", AccountId);
		}

		#endregion

	}
}
