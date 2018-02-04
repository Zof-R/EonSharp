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

		Task<IEnumerable<Transaction>> GetCommittedAllAsync(String accountId);
		Task<IEnumerable<Transaction>> GetCommittedPageAsync(String accountId, int page);
		Task<IEnumerable<Transaction>> GetUncommittedAsync(String id);
		Task<Transaction> GetRegTransactionAsync(String id);
		Task<IEnumerable<Block>> GetLastBlocksAsync();
		Task<IEnumerable<Block>> GetLastBlocksFromAsync(int height);
		Task<Block> GetBlockByHeightAsync(int height);
		Task<Block> GetBlockByIdAsync(String blockId);
		Task<IEnumerable<Transaction>> GetTrsByBlockIdAsync(String blockId);
		Task<Transaction> GetTransactionByIdAsync(String trId);
		Task<IEnumerable<Transaction>> GetLastUncommittedTrsAsync();


		//Private methods, kept here for future reference
		//Task<long> GetAccountId(String accountId);
		//Task<long> GetBlockId(String blockId);
		//Task<long> GetTrId(String trId);

	}
}
