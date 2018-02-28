using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class OrdinaryPayment : Transaction
	{
		public long Amount
		{
			get
			{
				return ((Attachments.OrdinaryPaymentAttachment)Attachment).Amount;
			}
			set
			{
				((Attachments.OrdinaryPaymentAttachment)Attachment).Amount = value;
			}
		}
		public string Recipient
		{
			get
			{
				return ((Attachments.OrdinaryPaymentAttachment)Attachment).Recipient;
			}
			set
			{
				((Attachments.OrdinaryPaymentAttachment)Attachment).Recipient = value;
			}
		}
		public OrdinaryPayment() : base()
		{
			Type = 200;
			Attachment = new Attachments.OrdinaryPaymentAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}
		public OrdinaryPayment(int version) : this()
		{
			Version = version;

		}
		public OrdinaryPayment(string sender, long amount, string recipient, int deadline = 3600, long fee = 10, int version = 2) : this(version)
		{
			this.Amount = amount;
			this.Recipient = recipient;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
		}
		public OrdinaryPayment(SerializationInfo info, StreamingContext context) : base(info, context)
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
