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
			}
			return null;
		}



	}
}
