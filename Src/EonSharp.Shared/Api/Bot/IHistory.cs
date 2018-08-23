using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Bot
{
	public interface IHistory
	{
		//com.exscudo.eon.bot.TransactionHistoryService
		Task<IEnumerable<Transaction>> GetCommittedAsync(string id);
		Task<IEnumerable<Transaction>> GetCommittedAllAsync(string id);
		Task<IEnumerable<Transaction>> GetCommittedPageAsync(string id, int page);
		Task<IEnumerable<Transaction>> GetUncommittedAsync(string id);
		Task<IEnumerable<BlockHeader>> GetSignedBlockAsync(string id);

	}
}
