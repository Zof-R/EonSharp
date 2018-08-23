using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	/**
	 * Determines the distribution of votes
	 */
	public  class VotingRights
	{

		public int Weight { get; set; }
		public IDictionary<string,int> Delegates { get; set; }


		//public Integer weight;
		//public Map<String, Integer> delegates;
	}
}
