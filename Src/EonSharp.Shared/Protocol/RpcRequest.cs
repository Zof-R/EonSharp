using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EonSharp.Protocol.ExtensionMethods;
using Newtonsoft.Json.Serialization;

namespace EonSharp.Protocol
{
	public class RpcRequest
	{
		[JsonProperty(PropertyName = "jsonrpc")]
		public string Version { get; set; } = "2.0";

		public int ID { get; set; }

		public string Method { get; set; }

		public IEnumerable<object> Params { get; set; }


		public RpcRequest()
		{

		}
		public RpcRequest(string method, IEnumerable<object> @params = null, int id = 0)
		{
			this.Method = method;
			this.Params = @params;
			this.ID = id;
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { Formatting = Formatting.None, StringEscapeHandling = StringEscapeHandling.EscapeHtml, ContractResolver = new CamelCasePropertyNamesContractResolver() });
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is RpcRequest rrb)
			{
				//TODO: Params needs null and equality checking for each element.
				return this.Version == rrb.Version && this.ID == rrb.ID && this.Method == rrb.Method && this.Params == rrb.Params;
			}
			return false;
		}


		//Implicit convertion

		public static implicit operator string(RpcRequest req)
		{
			return req.ToString();
		}

		public static implicit operator RpcRequest(string req)
		{
			return req.GetRpcRequestFromString();
		}



	}
}
