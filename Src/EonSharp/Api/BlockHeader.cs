using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public class BlockHeader : ISerializable
	{
		public string Id { get; set; }
		public int Timestamp { get; set; }
		public string Generator { get; set; }
		public string Signature { get; set; }
		public int Height { get; set; }
		public int TransactionCount { get; set; }
		public long TotalFee { get; set; }



		static Dictionary<string, Action<SerializationInfo, BlockHeader>> s_entryDict = new Dictionary<string, Action<SerializationInfo, BlockHeader>>
		{
			{ "id",(info, i)=> i.Id = info.GetString("id") },
			{ "timestamp",(info, i)=> i.Timestamp = info.GetInt32("timestamp") },
			{ "generator",(info, i)=> i.Generator = info.GetString("generator") },
			{ "signature",(info, i)=> i.Signature = info.GetString("signature") },
			{ "height",(info, i)=> i.Height = info.GetInt32("height") },
			{ "transaction_count",(info, i)=> i.TransactionCount = info.GetInt32("transaction_count") },
			{ "total_fee",(info, i)=> i.TotalFee = info.GetInt64("total_fee") }
		};
		public BlockHeader(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				if (s_entryDict.TryGetValue(entry.Name, out Action<SerializationInfo, BlockHeader> exec))
				{
					exec.Invoke(info, this);
				}
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("id", Id);
			info.AddValue("timestamp", Timestamp);
			info.AddValue("generator", Generator);
			info.AddValue("signature", Signature);
			info.AddValue("height", Height);
			info.AddValue("transaction_count", TransactionCount);
			info.AddValue("total_fee", TotalFee);
		}

	}
}
