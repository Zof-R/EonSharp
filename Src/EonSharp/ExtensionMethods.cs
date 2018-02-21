using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp
{
	public static class ExtensionMethods
	{
		public static string ToJson(this Wallet wallet)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(wallet, new Newtonsoft.Json.JsonSerializerSettings
			{
				ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
				MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
				NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
			});
		}
		public static void ToJson(this Wallet wallet, System.IO.Stream stream)
		{
			using (var writer = new System.IO.StreamWriter(stream))
			using (var jsonWriter = new Newtonsoft.Json.JsonTextWriter(writer))
			{
				var ser = new Newtonsoft.Json.JsonSerializer()
				{
					ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
					MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
					NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
				};
				ser.Serialize(jsonWriter, wallet);
				jsonWriter.Flush();
			}
		}
		public static string ToJson(this IEnumerable<Wallet> wallet)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(wallet, new Newtonsoft.Json.JsonSerializerSettings
			{
				ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
				MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
				NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
			});
		}
		public static void ToJson(this IEnumerable<Wallet> wallets, System.IO.Stream stream)
		{
			using (var writer = new System.IO.StreamWriter(stream))
			using (var jsonWriter = new Newtonsoft.Json.JsonTextWriter(writer))
			{
				var ser = new Newtonsoft.Json.JsonSerializer()
				{
					ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
					MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
					NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
				};
				ser.Serialize(jsonWriter, wallets);
				jsonWriter.Flush();
			}
		}

		public static Wallet FromJsonToWallet(this string json)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<Wallet>(json);
		}
		public static Wallet FromJsonToWallet(this System.IO.Stream jsonstream)
		{
			using (var reader = new System.IO.StreamReader(jsonstream))
			using (var jsonReader = new Newtonsoft.Json.JsonTextReader(reader))
			{
				var ser = new Newtonsoft.Json.JsonSerializer()
				{
					ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
					MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
					NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
				};
				return ser.Deserialize<Wallet>(jsonReader);
			}
		}
		public static IEnumerable<Wallet> FromJsonToWallets(this string json)
		{
			var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
			if (obj is Newtonsoft.Json.Linq.JObject jobj)
			{
				yield return jobj.ToObject<Wallet>();
			}
			else if (obj is Newtonsoft.Json.Linq.JArray jarr)
			{
				foreach (var jro in jarr)
				{
					yield return jro.ToObject<Wallet>();
				}
			}
			else
			{
				throw new Exception("Unexpected json type");
			}
		}
		public static IEnumerable<Wallet> FromJsonToWallets(this System.IO.Stream jsonstream)
		{
			using (var reader = new System.IO.StreamReader(jsonstream))
			using (var jsonReader = new Newtonsoft.Json.JsonTextReader(reader))
			{
				var ser = new Newtonsoft.Json.JsonSerializer();
				var obj = ser.Deserialize(jsonReader);
				if (obj is Newtonsoft.Json.Linq.JObject jobj)
				{
					yield return jobj.ToObject<Wallet>();
				}
				else if (obj is Newtonsoft.Json.Linq.JArray jarr)
				{
					foreach (var jro in jarr)
					{
						yield return jro.ToObject<Wallet>();
					}
				}
				else
				{
					throw new Exception("Unexpected json type");
				}
			}
		}

	}
}
