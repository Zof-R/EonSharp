using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Api;
using EonSharp.Network;
using EonSharp.Network.Transports;

namespace EonSharp
{
	/// <summary>
	/// This is the entryPoint class
	/// </summary>
	public class EonClient : IDisposable
	{
		const string SERVER_ADDRESS = Constants.NETWORK_TESTNET;
		const string SERVER_USER = null; // default "admin"
		const string SERVER_PASSWORD = null; //default "pass"


		public static Dictionary<Type, ActivatorDescriptor[]> ClassMapper = new Dictionary<Type, ActivatorDescriptor[]>()
		{
			{typeof(ITransportContext), new ActivatorDescriptor[] { new ActivatorDescriptor(typeof(HttpTransportClient)) }},
			{typeof(IPeer), new ActivatorDescriptor[] { new ActivatorDescriptor(typeof(Api.Peer.PeerEndpoint)) }},
			{typeof(IBot), new ActivatorDescriptor[] { new ActivatorDescriptor(typeof(Api.Bot.BotEndpoint)) }},
			{typeof(IExplorer), new ActivatorDescriptor[] { new ActivatorDescriptor(typeof(Api.Explorer.ExplorerEndpoint)) }},
			{typeof(IMetrics), new ActivatorDescriptor[] { new ActivatorDescriptor(typeof(Api.Metrics.MetricsEndpoint)) }},
		};

		public ITransportContext TransportContext { get; private set; }

		public IPeer Peer;
		public IBot Bot;
		public IExplorer Explorer;
		public IMetrics Metrics;

		/// <summary>
		/// Default constructor pointing to "https://peer.testnet.eontechnology.org:9443"
		/// </summary>
		public EonClient() : this(SERVER_ADDRESS, SERVER_USER, SERVER_PASSWORD)
		{
		}
		public EonClient(string serverAddress, string user = null, string password = null) : this(null, serverAddress, user, password)
		{

		}
		private EonClient(ITransportContext ctx, string serverAddress, string user = null, string password = null)
		{
			if (ctx == null)
			{
				TransportContext = BuildGraph<ITransportContext>(new object[] { serverAddress, user, password });
			}
			else
			{
				TransportContext = ctx.CreateNewTransportContext(serverAddress, user, password);
			}
			Peer = BuildGraph<IPeer>(new object[] { TransportContext });
			Bot = BuildGraph<IBot>(new object[] { TransportContext });
			Explorer = BuildGraph<IExplorer>(new object[] { TransportContext });
			Metrics = BuildGraph<IMetrics>(new object[] { TransportContext });

		}

		/// <summary>
		/// Creates a child ITransportContext pointing to the specified server address.
		/// Disposing a child ITransportContext doesn't dispose the root context, but disposing
		/// a root context will invalidate all child contexts.
		/// </summary>
		/// <param name="serverAddress">The server address</param>
		/// <param name="user">The login username for the choosen server. Defaults to null</param>
		/// <param name="password">The login password for the choosen server. Defaults to null</param>
		/// <returns>A child EonClient instance</returns>
		public EonClient CreateNewContext(string serverAddress, string user = null, string password = null)
		{
			return new EonClient(TransportContext, serverAddress, user, password);
		}

		/// <summary>
		/// Retrieves blockchain state information.
		/// The retrieved information, i.e. NetworkID, will be used by all EonClient instances
		/// and is required to process Transactions.
		/// </summary>
		public async Task UpdateBlockchainDetails()
		{
			var attribs = await Peer.Metadata.GetAttributesAsync();
			EonSharp.Configuration.NetworkId = attribs.NetworkID; //is used by transactions. Afects all transport contexts for now.
		}

		T BuildGraph<T>(object[] parameters)
		{
			var descriptors = ClassMapper[typeof(T)];
			var subclass = (T)Activator.CreateInstance(descriptors[0].ClassType, descriptors[0].Parameters != null ? descriptors[0].Parameters.Concat(parameters).ToArray() : parameters);
			if (descriptors.Length > 1)
			{
				for (int i = 1; i < descriptors.Length; i++)
				{
					var subclassparms = descriptors[i].Parameters != null ? descriptors[i].Parameters.Concat(parameters).ToArray() : parameters;
					subclassparms = subclassparms == null ? new object[] { subclass } : new object[] { subclass }.Concat(subclassparms).ToArray();
					var ctors = descriptors[i].ClassType.GetConstructors();
					foreach (var ctor in ctors.OrderByDescending(c => c.GetParameters().Length))
					{
						var par = ctor.GetParameters();
						if (par.Length == subclassparms.Length && par[0].ParameterType == typeof(T))
						{
							subclass = (T)ctor.Invoke(subclassparms);
							break;
						}
						else if (descriptors[i].Parameters != null && par.Length > 1 && par[0].ParameterType == typeof(T) && par.Length == descriptors[i].Parameters.Length + 1)
						{
							subclass = (T)ctor.Invoke(new object[] { subclass }.Concat(descriptors[i].Parameters).ToArray());
						}
						else if (par.Length == 1 && par[0].ParameterType == typeof(T))
						{
							subclass = (T)ctor.Invoke(new object[] { subclass });
						}
					}
				}
			}
			return subclass;
		}



		public void Dispose()
		{
			Peer = null;
			Bot = null;
			Explorer = null;
			Metrics = null;
			if (TransportContext != null)
			{
				TransportContext.Dispose();
				TransportContext = null;
			}
		}
	}
}
