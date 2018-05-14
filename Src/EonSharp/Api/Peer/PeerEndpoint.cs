using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EonSharp.Network;
using EonSharp.Protocol;
using EonSharp.Api.Transactions.ExtensionMethods;
using Newtonsoft.Json.Linq;

namespace EonSharp.Api.Peer
{
	public class PeerEndpoint : EndpointBase, IPeer, IBlocks, IMetadata, ITransactions, ISnapshot
	{
		int m_id;

		public IBlocks Blocks { get { return this; } }
		public IMetadata Metadata { get { return this; } }
		public ITransactions Transactions { get { return this; } }
		public ISnapshot Snapshot { get { return this; } }


		public PeerEndpoint(ITransportContext client) : base(client, "peer/v1")
		{
		}

		async Task<Difficulty> IBlocks.GetDifficultyAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("blocks.get_difficulty", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Difficulty>();
		}
		async Task<IEnumerable<Block>> IBlocks.GetBlockHistoryAsync(string[] blockSequence)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("blocks.get_block_history", new object[] { blockSequence }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<Block[]>();
		}
		async Task<Block> IBlocks.GetLastBlockAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("blocks.get_last_block", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Block>();
		}

		async Task<Attributes> IMetadata.GetAttributesAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("metadata.get_attributes", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Attributes>();
		}
		async Task<IEnumerable<string>> IMetadata.GetWellKnownNodesAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("metadata.get_well_known_nodes", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<string[]>();
		}
		async Task<bool> IMetadata.AddPeerAsync(long peerId, string peerAddress)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("metadata.add_peer", new object[] { peerId, peerAddress }, Interlocked.Increment(ref m_id)));
			return (bool)res.Result;
		}

		async Task<IEnumerable<Transaction>> ITransactions.GetTransactionsAsync(string lastBlockId, string[] ignoreList)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("transactions.get_transactions", new object[] { lastBlockId, ignoreList }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}

		async Task<Block> ISnapshot.GetLastBlockAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("snapshot.get_last_block", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Block>();
		}
		async Task<Block> ISnapshot.GetBlockByHeightAsync(int height)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("snapshot.get_block_by_height", new object[] { height }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Block>();
		}
		async Task<IEnumerable<Block>> ISnapshot.GetBlocksHeadFromAsync(int height)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("snapshot.get_blocks_head_from", new object[] { height }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<Block[]>();
		}
		async Task<IDictionary<string, object>> ISnapshot.GetAccountsAsync(string blockID)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("snapshot.get_accounts", new object[] { blockID }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Dictionary<string, object>>();
		}
		async Task<IDictionary<string, object>> ISnapshot.GetNextAccountsAsync(string blockID, string accountID)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("snapshot.get_next_accounts", new object[] { blockID, accountID }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Dictionary<string, object>>();
		}
	}
}
