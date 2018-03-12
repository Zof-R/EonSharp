using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	/**
	 * Account balance
	 */
	public class Balance
	{
		public State State { get; set; }
		public long Amount { get; set; }
		public IDictionary<string, long> ColoredCoins { get; set; }


		//public State state;
		//public long amount;

		public Balance()
		{

		}

		static Dictionary<string, Action<SerializationInfo, Balance>> s_entryDict = new Dictionary<string, Action<SerializationInfo, Balance>>
		{
			{ "state",(info, i)=> i.State = (State)info.GetValue("state", typeof(State)) },
			{ "amount",(info, i)=> i.Amount = info.GetInt64("amount") },
			{ "colored_coins",(info, i)=> i.ColoredCoins = info.GetValue("colored_coins", typeof(IDictionary<string, long>)) as IDictionary<string, long> }
		};
		public Balance(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				if (s_entryDict.TryGetValue(entry.Name, out Action<SerializationInfo, Balance> exec))
				{
					exec.Invoke(info, this);
				}
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("state", State);
			info.AddValue("amount", Amount);
			info.AddValue("colored_coins", ColoredCoins);
		}
	}
}
