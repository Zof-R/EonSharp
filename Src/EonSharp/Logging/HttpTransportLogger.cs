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


		public HttpTransportLogger(ITransportContext ctx, string prefix)
		{
			Context = ctx;
			Prefix = prefix;
		}


		public Uri ServerAddress => Context.ServerAddress;

		public string User => Context.User;

		public string Password => Context.Password;

		public ITransportContext CreateNewTransportContext(string serverAddress, string user = null, string password = null) => new HttpTransportLogger(Context.CreateNewTransportContext(serverAddress, user, password), Prefix);

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
					handler(this, new LogMessage(Prefix, response.RawRpcRequest));
					handler(this, new LogMessage(Prefix, response.RawRpcResponse));
				}
				return response;
			}
			catch (Exception ex)
			{
				var handler = LogChanged;
				if (handler != null)
				{
					if (ex is ProtocolException)
					{
						handler(this, new LogMessage(LogMessageType.Warning, Prefix, ((ProtocolException)ex).JsonRpcResponse));
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
