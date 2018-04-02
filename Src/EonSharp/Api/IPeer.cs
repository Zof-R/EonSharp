using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Api.Peer;

namespace EonSharp.Api
{
	public interface IPeer
	{
		IBlocks Blocks { get; }
		IMetadata Metadata { get; }
		ITransactions Transactions { get; }
		ISnapshot Snapshot { get; }

	}
}
