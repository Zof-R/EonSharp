using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Protocol;

namespace EonSharp.Network
{
	public interface ITransportContext : IDisposable
	{
		Uri ServerAddress { get;  }
		string User { get;  }
		string Password { get;  }

		ITransportContext CreateNewTransportContext(string serverAddress, string user = null, string password = null);
		Task<RpcResponse> ProcessCommandAsync(string endpoint, string method, int id, IEnumerable<object> @params = null);
		Task<RpcResponse> ProcessCommandAsync(string endpointUrl, RpcRequest rpcrequest);
		Task<RpcResponse> ProcessCommandAsync(string endpointUrl, string rpcrequest);

		Task<string> GetPageAsync(string endpointUrl);


	}
}
