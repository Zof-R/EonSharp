using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public class ColoredCoinInfo : ISerializable
	{
		public State State { get; set; }
		public long Supply { get; set; }
		public int Decimal { get; set; }
		public int Timestamp { get; set; }


		public ColoredCoinInfo()
		{

		}

		static Dictionary<string, Action<SerializationInfo, ColoredCoinInfo>> s_entryDict = new Dictionary<string, Action<SerializationInfo, ColoredCoinInfo>>
		{
			{ "state",(info, i)=> i.State = (State)info.GetValue("state", typeof(State)) },
			{ "supply",(info, i)=> i.Supply = info.GetInt64("supply") },
			{ "decimal",(info, i)=> i.Decimal = info.GetInt32("decimal") },
			{ "timestamp",(info, i)=> i.Timestamp = info.GetInt32("timestamp") }
		};
		public ColoredCoinInfo(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				if (s_entryDict.TryGetValue(entry.Name, out Action<SerializationInfo, ColoredCoinInfo>  exec))
				{
					exec.Invoke(info, this);
				}
			}
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("state", State);
			info.AddValue("supply", Supply);
			info.AddValue("decimal", Decimal);
			info.AddValue("timestamp", Timestamp);
		}
	}
}
