using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class Payment : Transaction
	{
		public long Amount
		{
			get
			{
				return ((Attachments.PaymentAttachment)Attachment).Amount;
			}
			set
			{
				((Attachments.PaymentAttachment)Attachment).Amount = value;
			}
		}
		public string Recipient
		{
			get
			{
				return ((Attachments.PaymentAttachment)Attachment).Recipient;
			}
			set
			{
				((Attachments.PaymentAttachment)Attachment).Recipient = value;
			}
		}
		public Payment() : base()
		{
			Type = 200;
			Attachment = new Attachments.PaymentAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}
		public Payment(int version) : this()
		{
			Version = version;

		}
		public Payment(string sender, long amount, string recipient, int deadline = 3600, long fee = 10, int version = 1) : this(version)
		{
			this.Amount = amount;
			this.Recipient = recipient;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
			Version = version;
		}
		public Payment(SerializationInfo info, StreamingContext context) : base(info, context)
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
				[nameof(Amount).ToLower()] = new BEncoding.BInteger(Amount),
				[nameof(Recipient).ToLower()] = new BEncoding.BString(Recipient)
			};
		}

	}
}
