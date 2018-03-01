using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions.Attachments
{
	public class RegistrationAttachment : ISerializable
	{
		public string AccountId { get; set; }
		public string PublicKey { get; set; }


		public RegistrationAttachment()
		{

		}


		public RegistrationAttachment(SerializationInfo info, StreamingContext context)
		{
			var enumer = info.GetEnumerator();
			if (enumer.MoveNext())
			{
				var kv = enumer.Current;
				AccountId = kv.Name;
				PublicKey = kv.Value?.ToString();
			}
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(AccountId, PublicKey);
		}


	}
}
