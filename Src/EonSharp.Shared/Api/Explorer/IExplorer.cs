using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Explorer
{
	public interface IExplorer
	{
		//com.exscudo.eon.explorer.BlockchainExplorerService

		Task<IEnumerable<Transaction>> GetCommittedAllAsync(string accountId);
		Task<IEnumerable<Transaction>> GetCommittedPageAsync(string accountId, int page);
		Task<IEnumerable<Transaction>> GetUncommittedAsync(string id);
		Task<IEnumerable<BlockHeader>> GetLastBlocksAsync();
		Task<IEnumerable<BlockHeader>> GetLastBlocksFromAsync(int height);
		Task<Block> GetBlockByHeightAsync(int height);
		Task<Block> GetBlockByIdAsync(string blockId);
		Task<Transaction> GetTransactionByIdAsync(string trId);
		Task<IEnumerable<Transaction>> GetLastUncommittedTrsAsync();


		//Private methods, kept here for future reference
		//Task<long> GetAccountId(String accountId);
		//Task<long> GetBlockId(String blockId);
		//Task<long> GetTrId(String trId);

	}
}
