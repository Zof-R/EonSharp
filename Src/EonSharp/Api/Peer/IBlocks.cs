using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Peer
{
	public interface IBlocks
	{
		//com.exscudo.peer.eon.stubs.SyncBlockService

		Task<Difficulty> GetDifficultyAsync();
		Task<IEnumerable<Block>> GetBlockHistoryAsync(String[] blockSequence);
		Task<Block> GetLastBlockAsync();


	}
}
