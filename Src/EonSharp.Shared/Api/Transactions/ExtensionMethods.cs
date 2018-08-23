using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Api.Transactions.Attachments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EonSharp.Api.Transactions.ExtensionMethods
{
	public static class ExtensionMethods
	{
		public static IEnumerable<Transaction> ToTransactionCollection(this string jsonstr)
		{
			var jsonarray = JArray.Parse(jsonstr);
			return jsonarray.ToTransactionCollection();
		}


		public static IEnumerable<Transaction> ToTransactionCollection(this JArray array)
		{
			foreach (JObject jo in array)
			{
				if (jo == null)
				{
					continue;
				}
				yield return jo.ToTransaction();
			}
		}


		public static Transaction ToTransaction(this string jsonstr)
		{
			var jsonobj = JObject.Parse(jsonstr);
			return jsonobj.ToTransaction();
		}

		public static Transaction ToTransaction(this JObject jo)
		{
			if (jo == null)
			{
				return null;
			}
			var type = jo["type"].ToObject<int>();
			Transaction trans;
			switch (type)
			{
				case 100:
					trans = jo.ToObject<Registration>();
					trans.Attachment = jo["attachment"].ToObject<RegistrationAttachment>();
					return trans;
				case 200:
					trans = jo.ToObject<Payment>();
					trans.Attachment = jo["attachment"].ToObject<PaymentAttachment>();
					return trans;
				case 300:
					trans = jo.ToObject<Deposit>();
					trans.Attachment = jo["attachment"].ToObject<DepositAttachment>();
					return trans;
				case 400:
					trans = jo.ToObject<Delegate>();
					trans.Attachment = jo["attachment"].ToObject<DelegateAttachment>();
					return trans;
				case 410:
					trans = jo.ToObject<Quorum>();
					trans.Attachment = jo["attachment"].ToObject<QuorumAttachment>();
					return trans;
				case 420:
					trans = jo.ToObject<Rejection>();
					trans.Attachment = jo["attachment"].ToObject<RejectionAttachment>();
					return trans;
				case 430:
					trans = jo.ToObject<Publication>();
					trans.Attachment = jo["attachment"].ToObject<PublicationAttachment>();
					return trans;
				case 500:
					trans = jo.ToObject<ColoredCoinRegistration>();
					trans.Attachment = jo["attachment"].ToObject<ColoredCoinRegistrationAttachment>();
					return trans;
				case 510:
					trans = jo.ToObject<ColoredCoinPayment>();
					trans.Attachment = jo["attachment"].ToObject<ColoredCoinPaymentAttachment>();
					return trans;
				case 520:
					trans = jo.ToObject<ColoredCoinSupply>();
					trans.Attachment = jo["attachment"].ToObject<ColoredCoinSupplyAttachment>();
					return trans;
				//case 600:
				//	trans = jo.ToObject<ComplexPayment>();
				//	trans.Attachment = jo["attachment"].ToObject<ComplexPaymentAttachment>();
				//	return trans;
			}
			return null;
		}

		public static Balance ToBalance(this JObject jo)
		{
			if (jo == null)
			{
				return null;
			}

			var bal = jo.ToObject<Balance>();
			bal.ColoredCoins = jo["colored_coins"]?.ToObject<IDictionary<string, long>>();
			return bal;
		}
		public static Info ToInfo(this JObject jo)
		{
			if (jo == null)
			{
				return null;
			}

			var info = jo.ToObject<Info>();

			if (jo.TryGetValue("voter", out JToken voter))
			{
				info.Voter = voter.ToObject<IDictionary<string, int>>();
			}
			if (jo.TryGetValue("voting_rights", out JToken votingrights))
			{
				var jot = votingrights as JObject;
				info.VotingRights = jot?.ToVotingRights();
			}
			if (jo.TryGetValue("quorum", out JToken quo))
			{
				var jot = quo as JObject;
				info.Quorum = jot?.ToQuorum();
			}
			return info;
		}
		public static VotingRights ToVotingRights(this JObject jo)
		{
			if (jo == null)
			{
				return null;
			}
			var votingrights = jo.ToObject<VotingRights>();
			if (jo.TryGetValue("delegates", out JToken del))
			{
				votingrights.Delegates = del.ToObject<IDictionary<string, int>>();
			}
			return votingrights;
		}
		public static EonSharp.Api.Quorum ToQuorum(this JObject jo)
		{
			if (jo == null)
			{
				return null;
			}
			var quorum = jo.ToObject<EonSharp.Api.Quorum>();
			if (jo.TryGetValue("quorum_by_types", out JToken del))
			{
				quorum.QuorumByTypes = del.ToObject<IDictionary<int, int>>();
			}
			return quorum;
		}

		public static string ToJson(this Transaction transaction)
		{
			return JsonConvert.SerializeObject(transaction, Formatting.None, new JsonSerializerSettings { Formatting = Formatting.None, StringEscapeHandling = StringEscapeHandling.EscapeHtml, ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
		}
		public static string ToJson(this IEnumerable<Transaction> transactions)
		{
			return JsonConvert.SerializeObject(transactions, Formatting.None, new JsonSerializerSettings { Formatting = Formatting.None, StringEscapeHandling = StringEscapeHandling.EscapeHtml, ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
		}



		public static void ToJson(this EonSharp.Api.Transaction transaction, System.IO.Stream stream)
		{
			using (var writer = new System.IO.StreamWriter(stream))
			{
				writer.Write(ToJson(transaction));
				writer.Flush();
			}
		}
		public static void ToJson(this IEnumerable<EonSharp.Api.Transaction> transactions, System.IO.Stream stream)
		{
			using (var writer = new System.IO.StreamWriter(stream))
			{
				writer.Write(ToJson(transactions));
				writer.Flush();
			}
		}
		public static EonSharp.Api.Transaction ToTransaction(this System.IO.Stream jsonstream)
		{
			using (var reader = new System.IO.StreamReader(jsonstream))
			{
				return ToTransaction(reader.ReadToEnd());
			}
		}
		public static IEnumerable<EonSharp.Api.Transaction> ToTransactionCollection(this System.IO.Stream jsonstream)
		{
			using (var reader = new System.IO.StreamReader(jsonstream))
			{
				return ToTransactionCollection(reader.ReadToEnd());
			}
		}
	}
}
