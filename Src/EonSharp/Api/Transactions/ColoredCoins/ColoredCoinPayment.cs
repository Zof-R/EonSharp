using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class ColoredCoinPayment : Transaction
	{
		public long Amount
		{
			get
			{
				return ((Attachments.ColoredCoinPaymentAttachment)Attachment).Amount;
			}
			set
			{
				((Attachments.ColoredCoinPaymentAttachment)Attachment).Amount = value;
			}
		}

		public ColoredCoinPayment() : base()
		{
			Type = 510;
			Attachment = new Attachments.ColoredCoinPaymentAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public ColoredCoinPayment(int version) : this()
		{
			Version = version;

		}
		public ColoredCoinPayment(string sender, long amount, int deadline = 3600, long fee = 10, int version = 1) : this(version)
		{
			this.Amount = amount;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
			Version = version;
		}

		public ColoredCoinPayment(SerializationInfo info, StreamingContext context) : base(info, context)
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
			};
		}
	}
}
