using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions.Attachments
{
	public class QuorumAttachment : ISerializable
	{
		public int All { get; set; }
		public IDictionary<int, int> Types { get; set; }



		public QuorumAttachment()
		{

		}

		public QuorumAttachment(SerializationInfo info, StreamingContext context)
		{
			Types = new Dictionary<int, int>();
			var enumer = info.GetEnumerator();
			while (enumer.MoveNext())
			{
				var kv = enumer.Current;

				if (kv.Name == "all" && kv.Value != null && kv.Value is Newtonsoft.Json.Linq.JValue)
				{
					All = ((Newtonsoft.Json.Linq.JValue)kv.Value).ToObject<int>();
				}
				else
				{
					if (kv.Name != null && kv.Value != null && kv.Value is Newtonsoft.Json.Linq.JValue)
					{
						if (int.TryParse(kv.Name, out int key))
						{
							Types[key] = ((Newtonsoft.Json.Linq.JValue)kv.Value).ToObject<int>();
						}
					}
				}
			}
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(All).ToLower(), All);
			var enumer = Types.GetEnumerator();
			while (enumer.MoveNext())
			{
				var kv = enumer.Current;
				info.AddValue(kv.Key.ToString(), kv.Value);
			}
		}

	}
}
