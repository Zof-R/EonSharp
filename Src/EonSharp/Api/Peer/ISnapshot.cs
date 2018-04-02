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
		Task<IDictionary<string, object>> GetAccountsAsync(string blockID);
		Task<IDictionary<string, object>> GetNextAccountsAsync(string blockID, string accountID);
	}
}
