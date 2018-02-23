using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Network;
using EonSharp.Protocol;

namespace EonSharp.Logging
{
	public class HttpTransportLogger : ITransportContext, ILog
	{
		public string Prefix { get; set; }
		public ITransportContext Context { get; private set; }

		public event EventHandler<LogMessage> LogChanged;

		HashSet<string> ExcludeFilter;

		public HttpTransportLogger(ITransportContext ctx, string prefix)
		{
			Context = ctx;
			Prefix = prefix;

		}
		public HttpTransportLogger(ITransportContext ctx, string prefix, IEnumerable<string> excludeFilter) : this(ctx, prefix)
		{
			if (excludeFilter != null)
			{
				ExcludeFilter = new HashSet<string>(excludeFilter.Select(f => f.ToLower().Replace("async", "")));
			}
		}


		public Uri ServerAddress => Context.ServerAddress;

		public string User => Context.User;

		public string Password => Context.Password;

		public ITransportContext CreateNewTransportContext(string serverAddress, string user = null, string password = null) => new HttpTransportLogger(Context.CreateNewTransportContext(serverAddress, user, password), Prefix, ExcludeFilter);

		public async Task<string> GetPageAsync(string endpointUrl) => await Context.GetPageAsync(endpointUrl);

		public async Task<RpcResponse> ProcessCommandAsync(string endpoint, string method, int id, IEnumerable<object> @params = null) => await Context.ProcessCommandAsync(endpoint, method, id, @params);

		public async Task<RpcResponse> ProcessCommandAsync(string endpointUrl, RpcRequest rpcrequest)
		{
			try
			{
				var response = await Context.ProcessCommandAsync(endpointUrl, rpcrequest);
				var handler = LogChanged;
				if (handler != null)
				{
					var log = true;
					if (ExcludeFilter != null)
					{
						var urlf = endpointUrl.ToLower().Trim("/ ".ToCharArray());
						var methf = rpcrequest.Method.ToLower();
						var meth = methf.Split('.').Last();
						var filtertypes = new string[] { $"{urlf}.{methf}", urlf, methf, meth };
						log = !filtertypes.Any(ft => ExcludeFilter.Contains(ft));
					}
					if (log)
					{
						handler(this, new LogMessage(Prefix, response.RawRpcRequest));
						handler(this, new LogMessage(Prefix, response.RawRpcResponse));
					}
				}
				return response;
			}
			catch (Exception ex)
			{
				var handler = LogChanged;
				if (handler != null)
				{
					if (ex is ProtocolException pex)
					{
						handler(this, new LogMessage(LogMessageType.Warning, Prefix, pex.JsonRpcResponse));
					}
					else
					{
						handler(this, new LogMessage(LogMessageType.Error, Prefix, ex.Message));
					}
				}
				throw ex;
			}
		}

		public async Task<RpcResponse> ProcessCommandAsync(string endpointUrl, string rpcrequest) => await Context.ProcessCommandAsync(endpointUrl, rpcrequest);

		public void Dispose() => Context.Dispose();

	}
}
