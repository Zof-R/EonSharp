using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class Quorum : Transaction
	{
		public int All
		{
			get
			{
				return ((Attachments.QuorumAttachment)Attachment).All;
			}
			set
			{
				((Attachments.QuorumAttachment)Attachment).All = value;
			}
		}
		public IDictionary<int, int> Types
		{
			get
			{
				return ((Attachments.QuorumAttachment)Attachment).Types;
			}
			set
			{
				((Attachments.QuorumAttachment)Attachment).Types = value;
			}
		}


		public Quorum() : base()
		{
			Type = 410;
			Attachment = new Attachments.QuorumAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public Quorum(int version) : this()
		{
			Version = version;

		}
		public Quorum(string sender, int all, IDictionary<int, int> types = null, int deadline = 3600, long fee = 10, int version = 2) : this(version)
		{
			All = all;
			Types = types;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
		}

		public Quorum(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}

		public override void SignTransaction(byte[] expandedPrivateKey)
		{
			var beattach = AttachmentToBEncoding();
			base.SignTransaction(beattach, expandedPrivateKey);
		}
		public override void ConfirmTransaction(string accountId, byte[] expandedPrivateKey)
		{
			var beattach = AttachmentToBEncoding();
			base.ConfirmTransaction(beattach, accountId, expandedPrivateKey);
		}
		public BEncoding.BDictionary AttachmentToBEncoding()
		{
			var beattach = new BEncoding.BDictionary
			{
				[nameof(All).ToLower()] = new BEncoding.BInteger(All)
			};
			if (Types != null)
			{
				var enumer = Types.GetEnumerator();
				while (enumer.MoveNext())
				{
					var kv = enumer.Current;
					beattach[kv.Key.ToString()] = new BEncoding.BInteger(kv.Value);
				}
			}
			return beattach;
		}
	}
}
