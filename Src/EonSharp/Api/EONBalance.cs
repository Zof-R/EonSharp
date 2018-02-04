using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	/**
	 * Account balance
	 */
	public class EONBalance
	{
		public State State { get; set; }
		public long Amount { get; set; }
		public IDictionary<string, long> ColoredCoins { get; set; }


		//public State state;
		//public long amount;

		public EONBalance()
		{

		}
	}
}
