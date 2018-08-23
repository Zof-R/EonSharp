using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EonSharp.Network;
using EonSharp.Protocol;
using EonSharp.Api.Transactions.ExtensionMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EonSharp.Api.Bot
{
	public class BotEndpoint : EndpointBase, IBot, IAccounts, IHistory, ITime, ITransactions, IColoredCoin
	{
		int m_id;

		public IAccounts Accounts { get { return this; } }
		public IHistory History { get { return this; } }
		public ITime Time { get { return this; } }
		public ITransactions Transactions { get { return this; } }
		public IColoredCoin ColoredCoin { get { return this; } }

		public BotEndpoint(ITransportContext client) : base(client, "bot/v1")
		{
		}

		async Task<State> IAccounts.GetStateAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("accounts.get_state", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<State>();
		}

		async Task<Balance> IAccounts.GetBalanceAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("accounts.get_balance", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToBalance();
		}
		
		async Task<Info> IAccounts.GetInformationAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("accounts.get_information", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToInfo();
		}

		async Task<IEnumerable<Transaction>> IHistory.GetCommittedAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.get_committed", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}

		async Task<IEnumerable<Transaction>> IHistory.GetCommittedAllAsync(string id)
		{
			var results = new List<IEnumerable<Transaction>>();
			for (int i = 0; ; i++)
			{
				var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.get_committed_page", new object[] { id, i }, Interlocked.Increment(ref m_id)));
				var tcol = (res.Result as JArray)?.ToTransactionCollection();
				if (tcol == null || tcol.Count() == 0)
				{
					break;
				}
				results.Add(tcol);
			}
			return results.SelectMany(t => t).ToList();
		}

		async Task<IEnumerable<Transaction>> IHistory.GetCommittedPageAsync(string id, int page)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.get_committed_page", new object[] { id, page }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}

		async Task<IEnumerable<Transaction>> IHistory.GetUncommittedAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.get_uncommitted", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}

		async Task<IEnumerable<BlockHeader>> IHistory.GetSignedBlockAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.get_signed_block", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<BlockHeader[]>();
		}

		async Task<long> ITime.GetAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("time.get", null, Interlocked.Increment(ref m_id)));
			return (long)res.Result;
		}

		async Task ITransactions.PutTransactionAsync(Transaction tx)
		{
			await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("transactions.put_transaction", new object[] { tx }, Interlocked.Increment(ref m_id)));
		}

		async Task<ColoredCoinInfo> IColoredCoin.GetInfo(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("colored.get_info", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<ColoredCoinInfo>();
		}
	}
}
