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

		public BotEndpoint(ITransportContext client) : base(client, "bot/")
		{
		}

		async Task<State> IAccounts.GetStateAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("accounts.getState", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<State>();
		}

		async Task<Balance> IAccounts.GetBalanceAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("accounts.getBalance", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Balance>();
		}
		
		async Task<Info> IAccounts.GetInformationAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("accounts.getInformation", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<Info>();
		}

		async Task<IEnumerable<Transaction>> IHistory.GetCommittedAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.getCommitted", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}

		async Task<IEnumerable<Transaction>> IHistory.GetCommittedAllAsync(string id)
		{
			var results = new List<IEnumerable<Transaction>>();
			for (int i = 0; ; i++)
			{
				var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.getCommittedPage", new object[] { id, i }, Interlocked.Increment(ref m_id)));
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
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.getCommittedPage", new object[] { id, page }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}

		async Task<IEnumerable<Transaction>> IHistory.GetUncommittedAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.getUncommitted", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToTransactionCollection();
		}

		async Task<IEnumerable<Block>> IHistory.GetSignedBlockAsync(string id)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("history.getSignedBlock", new object[] { id }, Interlocked.Increment(ref m_id)));
			return (res.Result as JArray)?.ToObject<Block[]>();
		}

		async Task<long> ITime.GetAsync()
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("time.get", null, Interlocked.Increment(ref m_id)));
			return (long)res.Result;
		}

		async Task ITransactions.PutTransactionAsync(Transaction tx)
		{
			await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("transactions.putTransaction", new object[] { tx }, Interlocked.Increment(ref m_id)));
		}

		async Task<ColoredCoinInfo> IColoredCoin.GetInfo(string id, int timestamp)
		{
			var res = await m_transportClient.ProcessCommandAsync(EndpointUrl, new RpcRequest("coloredCoin.getInfo", new object[] { id, timestamp }, Interlocked.Increment(ref m_id)));
			return (res.Result as JObject)?.ToObject<ColoredCoinInfo>();
		}
	}
}
