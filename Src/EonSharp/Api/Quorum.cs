using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	/**
	 * Transaction confirmation settings
	 */
	public class Quorum
	{
		//lowercase first letter due to c# rules
		public int quorum { get; set; }
		public IDictionary<int, int> QuorumByTypes { get; set; }



		//public Integer quorum;
		//public Map<Integer, Integer> quorumByTypes;
	}
}
