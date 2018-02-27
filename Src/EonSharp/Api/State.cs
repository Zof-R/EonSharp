using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	/** 
 	 * Account status 
 	 */
	public class State
	{


		public int Code { get; set; }
		public string Name { get; set; }
		public string ColoredCoin { get; set; }


		public State()
		{

		}


		public static implicit operator bool(State s)
		{
			return s?.Code > 99 && s?.Code < 300;
		}


	}

}
