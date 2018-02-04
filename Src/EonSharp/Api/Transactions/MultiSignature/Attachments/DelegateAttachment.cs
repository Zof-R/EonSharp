using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions.Attachments
{
	public class DelegateAttachment: ISerializable
	{

		public string Account { get; set; }
		public int Weight { get; set; }

		public DelegateAttachment()
		{

		}

		public DelegateAttachment(SerializationInfo info, StreamingContext context)
		{
			var enumer = info.GetEnumerator();
			if (enumer.MoveNext())
			{
				var kv = enumer.Current;
				Account = kv.Name;
				if (kv.Value != null && kv.Value is Newtonsoft.Json.Linq.JValue)
				{
					Weight = ((Newtonsoft.Json.Linq.JValue)kv.Value).ToObject<int>();
				}
			}
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(Account, Weight);
		}
	}
}
