using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Api.Transactions;
using Newtonsoft.Json;
using EonSharp.Helpers;
using EonSharp.Providers;
using EonSharp.Api.Transactions.ExtensionMethods;

namespace EonSharp.Api
{
	public class Transaction : ISerializable
	{

		public int Type { get; set; }
		public long Timestamp { get; set; }
		public string Id { get; set; }
		public int Deadline { get; set; }
		public long Fee { get; set; }
		public string Sender { get; set; }
		public string ReferencedTransaction { get; set; }
		public object Attachment { get; set; }
		public string Signature { get; set; }
		public IDictionary<string, string> Confirmations { get; set; }
		public int Version { get; set; } = 1;
		public string Network { get; set; } = Configuration.NetworkId;

		public Transaction()
		{

		}
		public Transaction(int version)
		{
			Version = version;
		}

		public void SignTransaction(BEncoding.BDictionary attachement, byte[] expandedPrivateKey)
		{
			var signature = ComputeSignature(attachement, expandedPrivateKey);
			Signature = signature.ToHexString();
			Id = IdProvider.ComputeID(IdProvider.ComputeTransactionNumber(signature, (int)Timestamp), IdProvider.IdType.Transaction);
		}

		public void ConfirmTransaction(BEncoding.BDictionary attachement, string accountId, byte[] expandedPrivateKey)
		{
			var signature = ComputeSignature(attachement, expandedPrivateKey);
			if (Confirmations == null)
			{
				Confirmations = new Dictionary<string, string>();
			}
			Confirmations[accountId] = signature.ToHexString();
		}


		private byte[] ComputeSignature(BEncoding.BDictionary attachement, byte[] expandedPrivateKey)
		{
			var be = new BEncoding.BDictionary
			{
				[nameof(Attachment).ToLower()] = attachement,
				[nameof(Type).ToLower()] = new BEncoding.BInteger(Type),
				[nameof(Timestamp).ToLower()] = new BEncoding.BInteger(Timestamp),
				[nameof(Deadline).ToLower()] = new BEncoding.BInteger(Deadline),
				[nameof(Fee).ToLower()] = new BEncoding.BInteger((long)Fee),
				[nameof(Sender).ToLower()] = new BEncoding.BString(Sender),
				[nameof(Network).ToLower()] = new BEncoding.BString(Network)
			};
			if (Version > 1)
			{
				be[nameof(Version).ToLower()] = new BEncoding.BInteger(Version);
			}

			var bestr = be.ToBencodedString().ToUpper();
			var buffer = UTF8Encoding.UTF8.GetBytes(bestr);
			return Chaos.NaCl.Ed25519.Sign(buffer, expandedPrivateKey);
		}


		static Dictionary<string, Action<SerializationInfo, Transaction>> s_entryDict = new Dictionary<string, Action<SerializationInfo, Transaction>>
		{
			{ "type",(info, tx)=> tx.Type = info.GetInt32("type") },
			{ "timestamp",(info, tx)=> tx.Timestamp = info.GetInt64("timestamp") },
			{ "id",(info, tx)=> tx.Id = info.GetString("id") },
			{ "deadline",(info, tx)=> tx.Deadline = info.GetInt32("deadline") },
			{ "fee",(info, tx)=> tx.Fee = info.GetInt64("fee") },
			{ "sender",(info, tx)=> tx.Sender = info.GetString("sender") },
			{ "referencedTransaction",(info, tx)=> tx.ReferencedTransaction = info.GetString("referencedTransaction") },
			//Attachment is dealt with in the Enpoint classes
			{ "signature",(info, tx)=> tx.Signature = info.GetString("signature") },
			{ "confirmations",(info, tx)=> tx.Confirmations = (Dictionary<string,string>)info.GetValue("confirmations", typeof(Dictionary<string, string>)) },
			{ "version",(info, tx)=> tx.Version = info.GetInt32("version") },
			{ "network",(info, tx)=> tx.Network = info.GetString("network") },
		};

		public Transaction(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				Action<SerializationInfo, Transaction> exec;
				if (s_entryDict.TryGetValue(entry.Name, out exec))
				{
					exec.Invoke(info, this);
				}
			}
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("type", Type);
			info.AddValue("timestamp", Timestamp);
			//info.AddValue("id", Id);
			info.AddValue("deadline", Deadline);
			info.AddValue("fee", Fee);
			info.AddValue("sender", Sender);
			//info.AddValue("referencedTransaction", ReferencedTransaction);
			info.AddValue("attachment", Attachment);
			info.AddValue("signature", Signature);
			if (Confirmations != null)
			{
				info.AddValue("confirmations", Confirmations);
			}
			info.AddValue("version", Version);
			info.AddValue("network", Network);
		}


		public static explicit operator Transaction(string jsontx)
		{
			return jsontx.ToTransaction();
		}



	}
}
