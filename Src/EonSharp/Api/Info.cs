using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	/** 
 	 * Account state 
 	 */
	public class Info
	{
		public State State { get; set; }
		public string PublicKey { get; set; }
		public long Amount { get; set; }
		public long Deposit { get; set; }


		public string SignType { get; set; }
		public VotingRights VotingRights { get; set; }
		public Quorum Quorum { get; set; }
		public string Seed { get; set; }
		public Dictionary<string, int> Voter { get; set; }


	}

}
