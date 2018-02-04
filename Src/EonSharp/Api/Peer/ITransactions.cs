using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Peer
{
	public interface ITransactions
	{
		//com.exscudo.peer.eon.stubs.SyncTransactionService
		Task<IEnumerable<Transaction>> GetTransactionsAsync(String lastBlockId, String[] ignoreList);
	}
}
