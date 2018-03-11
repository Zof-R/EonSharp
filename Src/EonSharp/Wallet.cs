using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EonSharp.Api;
using EonSharp.Generators;
using EonSharp.Keystore;

namespace EonSharp
{
	public class Wallet : ISerializable, INotifyPropertyChanged
	{
		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName]string propertyname = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
		}

		#endregion


		/// <summary>
		/// Serialization of Guid
		/// </summary>
		public string Id { get; set; } = Guid.NewGuid().ToString();

		/// <summary>
		/// Wallet's name
		/// </summary>
		public string Name { get; set; } = "Main";

		/// <summary>
		/// Version of wallet protocol
		/// </summary>
		public int Version { get; private set; } = 1;

		/// <summary>
		/// Container of cryptographic functions used to obtain the Seed/Private key
		/// </summary>
		public IKeystore Keystore { get; set; }

		public PublicAccountGenerator AccountDetails
		{
			get => m_accountDetails;
			private set
			{
				m_accountDetails = value;
				OnPropertyChanged();
			}
		}
		PublicAccountGenerator m_accountDetails;

		public Wallet()
		{
		}
		public Wallet(string name, string password)
		{
			Name = name;
			var privatekey = SeedGenerator.NewSeed();
			Keystore = new KeystoreV1(privatekey, password);
			AccountDetails = new PublicAccountGenerator(privatekey);
		}
		public Wallet(string name, byte[] privateKey, string password)
		{
			Name = name;
			Keystore = new KeystoreV1(privateKey, password);
			AccountDetails = new PublicAccountGenerator(privateKey);
		}


		#region ISerialization

		static Dictionary<string, Action<SerializationInfo, Wallet>> s_entryDict = new Dictionary<string, Action<SerializationInfo, Wallet>>
		{
			{ "id",(info, i)=> i.Id = info.GetString("id") },
			{ "name",(info, i)=> i.Name = info.GetString("name") },
			{ "version",(info, i)=> i.Version = info.GetInt32("version") },
			{ "keystore",(info, i)=> i.Keystore = info.GetValue("keystore", typeof(KeystoreV1)) as IKeystore },
			{ "accountdetails",(info, i)=> i.AccountDetails = info.GetValue("accountdetails", typeof(PublicAccountGenerator)) as PublicAccountGenerator },
		};
		public Wallet(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				if (s_entryDict.TryGetValue(entry.Name, out Action<SerializationInfo, Wallet> exec))
				{
					exec.Invoke(info, this);
				}
			}
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("id", Id);
			info.AddValue("name", Name);
			info.AddValue("version", Version);
			info.AddValue("keystore", Keystore);
			info.AddValue("accountdetails", AccountDetails);
		}

		#endregion

		public void UnlockAccountDetails(string password)
		{
			if (AccountDetails?.AccountNumber == 0)
			{
				UnlockAccountDetails(Keystore.GetPrivateKey(password));
			}
		}

		public byte[] GetPrivateKey(string password)
		{
			var pv = Keystore.GetPrivateKey(password);
			UnlockAccountDetails(pv);
			return pv;
		}
		public byte[] GetExpandedKey(string password)
		{
			return PublicKeyPairGenerator.ComputeExpandedPrivateKey(GetPrivateKey(password));
		}
		public void GetPrivateAndExpandedKeys(string password, out byte[] privatekey, out byte[] expandedprivatekey)
		{
			privatekey = GetPrivateKey(password);
			expandedprivatekey = PublicKeyPairGenerator.ComputeExpandedPrivateKey(privatekey);
		}

		void UnlockAccountDetails(byte[] privatekey)
		{
			if (AccountDetails?.AccountNumber == 0)
			{
				AccountDetails = new PublicAccountGenerator(privatekey);
			}
		}

		#region runtime


		public Info Information
		{
			get { return m_information; }
			set
			{
				m_information = value;
				OnPropertyChanged();
			}
		}
		private Info m_information;
		public Balance Balance
		{
			get { return m_balance; }
			set
			{
				m_balance = value;
				OnPropertyChanged();
			}
		}
		private Balance m_balance;

		public bool AutoRefreshEnabled { get; private set; }

		CancellationTokenSource m_refreshLoop;


		public IEnumerable<Transaction> CommitedTransactions
		{
			get
			{
				return m_commitedTransactions;
			}
			private set
			{
				m_commitedTransactions = value;
				OnPropertyChanged();
			}
		}
		private IEnumerable<Transaction> m_commitedTransactions;
		public IEnumerable<Transaction> UnCommitedTransactions
		{
			get
			{
				return m_unCommitedTransactions;
			}
			private set
			{
				m_unCommitedTransactions = value;
				OnPropertyChanged();
			}
		}
		private IEnumerable<Transaction> m_unCommitedTransactions;
		public bool AutoRefreshTransactionsEnabled { get; private set; }

		CancellationTokenSource m_refreshTxLoop;


		/// <summary>
		/// Enables or disables polling mechanism. Must be called from UX thread.
		/// </summary>
		/// <param name="enable">Enables or disables polling for changes</param>
		/// <param name="client"></param>
		/// <param name="interval">Interval between calls in milliseconds</param>
		public void SetAutoRefresh(bool enable, EonClient client = null, int interval = 5000)
		{
			if (enable)
			{
				if (m_refreshLoop != null)
				{
					m_refreshLoop.Cancel();
				}
				m_refreshLoop = new CancellationTokenSource();
				var t = RefreshAsyncLoop(client, interval, m_refreshLoop.Token);
			}
			else
			{
				if (m_refreshLoop != null)
				{
					m_refreshLoop.Cancel();
					m_refreshLoop = null;
				}
			}

			AutoRefreshEnabled = enable;
			OnPropertyChanged("AutoRefreshEnabled");
		}
		async Task RefreshAsyncLoop(EonClient client, int interval, CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				await RefreshAsync(client);
				await Task.Delay(interval, ct);
			}
		}
		public async Task RefreshAsync(EonClient client)
		{
			var info = await client.Bot.Accounts.GetInformationAsync(AccountDetails.AccountId);
			if (info?.State)
			{
				Information = info;
			}
			else
			{
				throw new Exception(info?.State?.Name);
			}
			var bal = await client.Bot.Accounts.GetBalanceAsync(AccountDetails.AccountId);
			if (bal?.State)
			{
				Balance = bal;
			}
			else
			{
				throw new Exception(bal?.State?.Name);
			}
		}

		/// <summary>
		/// Enables or disables polling mechanism. Must be called from UX thread.
		/// </summary>
		/// <param name="enable">Enables or disables polling for changes</param>
		/// <param name="client"></param>
		/// <param name="interval">Interval between calls in milliseconds</param>
		public void SetTransactionsAutoRefresh(bool enable, EonClient client = null, int interval = 5000)
		{
			if (enable)
			{
				if (m_refreshTxLoop != null)
				{
					m_refreshTxLoop.Cancel();
				}
				m_refreshTxLoop = new CancellationTokenSource();
				var t = TransactionsRefreshAsyncLoop(client, interval, m_refreshTxLoop.Token);
			}
			else
			{
				if (m_refreshTxLoop != null)
				{
					m_refreshTxLoop.Cancel();
					m_refreshTxLoop = null;
				}
			}

			AutoRefreshEnabled = enable;
			OnPropertyChanged("AutoRefreshEnabled");
		}
		async Task TransactionsRefreshAsyncLoop(EonClient client, int interval, CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				await TransactionsRefreshAsync(client);
				await Task.Delay(interval, ct);
			}
		}
		public async Task TransactionsRefreshAsync(EonClient client)
		{
			var utxs = await client.Bot.History.GetUncommittedAsync(AccountDetails.AccountId);
			this.UnCommitedTransactions = utxs;

			var txs = await client.Bot.History.GetCommittedAllAsync(AccountDetails.AccountId);
			this.CommitedTransactions = txs;
		}

		public async Task PutTransactionAsync(EonClient client, string password, Transaction tx)
		{
			var expkey = GetExpandedKey(password);
			tx.SignTransaction(expkey);
			await client.Bot.Transactions.PutTransactionAsync(tx);
		}

		#endregion

	}
}
