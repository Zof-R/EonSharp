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
		public long MoneySupply { get; set; }
		public int DecimalPoint { get; set; }


		public ColoredCoinInfo()
		{

		}

		static Dictionary<string, Action<SerializationInfo, ColoredCoinInfo>> s_entryDict = new Dictionary<string, Action<SerializationInfo, ColoredCoinInfo>>
		{
			{ "state",(info, i)=> i.State = (State)info.GetValue("state", typeof(State)) },
			{ "money_supply",(info, i)=> i.MoneySupply = info.GetInt64("money_supply") },
			{ "decimal_point",(info, i)=> i.DecimalPoint = info.GetInt32("decimal_point") }
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
			info.AddValue("money_supply", MoneySupply);
			info.AddValue("decimal_point", DecimalPoint);
		}
	}
}
