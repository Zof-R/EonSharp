using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EonSharp.Network;
using EonSharp.Protocol;
using Newtonsoft.Json.Linq;
using EonSharp.Api.Transactions.ExtensionMethods;

namespace EonSharp.Api.Explorer
{
	public class ExplorerEndpoint : EndpointBase, Api.IExplorer, IAccounts, IExplorer
	{
		int m_id;

		public IAccounts Accounts { get { return this; } }
		public IExplorer Explorer { get { return this; } }


		public ExplorerEndpoint(ITransportContext client) : base(client, "explorer/v1")
		{
		}

		#region IAccounts

		async Task<State> IAccounts.GetStateAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("accounts.getState", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<State>();
		}

		async Task<Balance> IAccounts.GetBalanceAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("accounts.getBalance", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToBalance();
		}

		async Task<Info> IAccounts.GetInformationAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("accounts.getInformation", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToInfo();
		}

		#endregion
		#region IExplorer

		async Task<IEnumerable<Transaction>> IExplorer.GetCommittedAllAsync(string accountId)
		{
			var results = new List<IEnumerable<Transaction>>();
			for (int i = 0; ; i++)
			{
				var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getCommittedPage", new object[] { accountId, i }, Interlocked.Increment(ref m_id)));
				var tcol = (res.Result as JArray)?.ToTransactionCollection();
				if (tcol == null || tcol.Count() == 0)
				{
					break;
				}
				results.Add(tcol);
			}
			return results.SelectMany(t => t).ToList();
		}

		async Task<IEnumerable<Transaction>> IExplorer.GetCommittedPageAsync(string accountId, int page)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getCommittedPage", new object[] { accountId, page }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}

		async Task<IEnumerable<Transaction>> IExplorer.GetUncommittedAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getUncommitted", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}

		async Task<Transaction> IExplorer.GetRegTransactionAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getRegTransaction", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToTransaction();
		}

		async Task<IEnumerable<Block>> IExplorer.GetLastBlocksAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getLastBlocks", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<Block[]>();
		}

		async Task<IEnumerable<Block>> IExplorer.GetLastBlocksFromAsync(int height)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getLastBlocksFrom", new object[] { height }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<Block[]>();
		}

		async Task<Block> IExplorer.GetBlockByHeightAsync(int height)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getBlockByHeight", new object[] { height }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Block>();
		}

		async Task<Block> IExplorer.GetBlockByIdAsync(string blockId)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getBlockById", new object[] { blockId }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Block>();
		}

		async Task<IEnumerable<Transaction>> IExplorer.GetTrsByBlockIdAsync(string blockId)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getTrsByBlockId", new object[] { blockId }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<Transaction[]>();
		}

		async Task<Transaction> IExplorer.GetTransactionByIdAsync(string trId)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getTransactionById", new object[] { trId }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToTransaction();
		}

		async Task<IEnumerable<Transaction>> IExplorer.GetLastUncommittedTrsAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("explorer.getLastUncommittedTrs", null, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}



		#endregion

	}
}
