using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class Rejection : Transaction
	{
		public string Account
		{
			get
			{
				return ((Attachments.RejectionAttachment)Attachment).Account;
			}
			set
			{
				((Attachments.RejectionAttachment)Attachment).Account = value;
			}
		}

		public Rejection() : base()
		{
			Type = 420;
			Attachment = new Attachments.RejectionAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public Rejection(int version) : this()
		{
			Version = version;

		}
		public Rejection(string sender, string account, int deadline = 3600, long fee = 10, int version = 2) : this(version)
		{
			Account = account;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
		}

		public Rejection(SerializationInfo info, StreamingContext context) : base(info, context)
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
			return new BEncoding.BDictionary
			{
				[nameof(Account).ToLower()] = new BEncoding.BString(Account)
			};
		}
	}
}
