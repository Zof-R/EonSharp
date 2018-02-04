using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public class Difficulty
	{
		public BigInteger CumulativeDifficulty { get; set; }
		public string LastBlockID { get; set; }

		public Difficulty()
		{
		}

	}
}
