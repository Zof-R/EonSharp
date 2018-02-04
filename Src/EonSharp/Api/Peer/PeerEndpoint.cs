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
	public class PeerEndpoint : EndpointBase, IPeer, IBlocks, IMetadata, ITransactions
	{
		int m_id;

		public IBlocks Blocks { get { return this; } }
		public IMetadata Metadata { get { return this; } }
		public ITransactions Transactions { get { return this; } }


		public PeerEndpoint(ITransportContext client) : base(client, "peer/")
		{
		}

		async Task<Difficulty> IBlocks.GetDifficultyAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("blocks.getDifficulty", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Difficulty>();
		}

		async Task<IEnumerable<Block>> IBlocks.GetBlockHistoryAsync(string[] blockSequence)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("blocks.getBlockHistory", new object[] { blockSequence }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<Block[]>();
		}

		async Task<Block> IBlocks.GetLastBlockAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("blocks.getLastBlock", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Block>();
		}

		async Task<SalientAttributes> IMetadata.GetAttributesAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("metadata.getAttributes", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<SalientAttributes>();
		}

		async Task<IEnumerable<string>> IMetadata.GetWellKnownNodesAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("metadata.getWellKnownNodes", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<string[]>();
		}

		async Task<bool> IMetadata.AddPeerAsync(long peerId, string peerAddress)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("metadata.addPeer", new object[] { peerId, peerAddress }, Interlocked.Increment(ref m_id)));
			return (bool)res.Result;
		}

		async Task<IEnumerable<Transaction>> ITransactions.GetTransactionsAsync(string lastBlockId, string[] ignoreList)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("transactions.getTransactions", new object[] { lastBlockId, ignoreList }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}


	}
}
