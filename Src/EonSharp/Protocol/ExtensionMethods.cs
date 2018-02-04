using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EonSharp.Protocol.ExtensionMethods
{
	public static class ExtensionMethods
	{

		public static RpcRequest GetRpcRequestFromString(this String str)
		{
			try
			{
				return JsonConvert.DeserializeObject<RpcRequest>(str);
			}
			catch (Exception ex)
			{
#if DEBUG
				Debug.WriteLine(ex.Message);
#endif
				throw ex;
			}
		}

		public static RpcResponse GetRpcResponseFromString(this String str)
		{
			try
			{
				return JsonConvert.DeserializeObject<RpcResponse>(str, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });
			}
			catch (Exception ex)
			{
#if DEBUG
				Debug.WriteLine(ex.Message);
#endif
				throw ex;
			}
		}

	}
}
