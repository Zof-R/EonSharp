using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public class Block
	{
		public BigInteger CumulativeDifficulty { get; set; }
		public string GenerationSignature { get; set; }
		public string Generator { get; set; }
		public int Height { get; set; }
		public string Id { get; set; }
		public string Prev { get; set; }
		public string Signature { get; set; }
		public string Snapshot { get; set; }
		public int Timestamp { get; set; }
		public int Version { get; set; }
		public IEnumerable<Transaction> Transactions { get; set; }
	}
}
