using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EonSharp.Protocol.ExtensionMethods;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace EonSharp.Protocol
{
	public class RpcResponse
	{
		[JsonIgnore]
		public string RawRpcRequest { get; set; }

		[JsonIgnore]
		public string RawRpcResponse { get; set; }


		[JsonProperty(PropertyName = "jsonrpc")]
		public string Version { get; set; } = "2.0";

		public int ID { get; set; }

		public object Result { get; set; }

		public RpcError Error { get; set; }


		public RpcResponse()
		{

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
			if (obj is RpcResponse)
			{
				var rrb = obj as RpcResponse;
				return this.Version == rrb.Version && this.ID == rrb.ID && this.Result == rrb.Result && this.Error == rrb.Error;
			}
			return false;
		}


		//Implicit convertion

		public static implicit operator string(RpcResponse res)
		{
			return res.ToString();
		}

		public static implicit operator RpcResponse(string res)
		{
			return res.GetRpcResponseFromString();
		}
	}
}
