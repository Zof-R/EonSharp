using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Peer
{
	public interface ISnapshot
	{
		Task<Block> GetLastBlockAsync();
		Task<Block> GetBlockByHeightAsync(int height);
		Task<IEnumerable<Block>> GetBlocksHeadFromAsync(int height);
		Task<IEnumerable<Account>> GetAccountsAsync(string blockID);
		Task<IEnumerable<Account>> GetNextAccountsAsync(string blockID, string accountID);
	}
}
