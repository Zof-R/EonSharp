using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Protocol
{
	[Serializable]
	public class ProtocolException : Exception
	{
		public long JsonRpcCode { get; private set; }
		public string JsonRpcData { get; private set; }
		public string JsonRpcRequest { get; private set; }
		public string JsonRpcResponse { get; private set; }

		public ProtocolException()
		{
		}

		public ProtocolException(string message) : base(message)
		{

		}

		public ProtocolException(string message, Exception inner) : base(message, inner)
		{

		}

		public ProtocolException(RpcError rpcError, string request = null, string response = null) : this(rpcError.Message)
		{
			JsonRpcCode = rpcError.Code;
			JsonRpcData = rpcError.Data;
			JsonRpcRequest = request;
			JsonRpcResponse = response;
		}

		public static implicit operator ProtocolException(RpcError e)
		{
			return new ProtocolException(e);
		}



	}
}
