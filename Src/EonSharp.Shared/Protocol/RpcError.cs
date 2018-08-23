using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Protocol
{
	public class RpcError
	{
		public long Code { get; set; }
		public string Message { get; set; }
		public string Data { get; set; }


		public static implicit operator string(RpcError err)
		{
			return $"Code: {err.Code} | Message: {err.Message} | Data: {err.Data}";
		}
	}
}
