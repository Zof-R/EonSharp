using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class DepositRefill : Transaction
	{
		public long Amount
		{
			get
			{
				return ((Attachments.DepositRefillAttachment)Attachment).Amount;
			}
			set
			{
				((Attachments.DepositRefillAttachment)Attachment).Amount = value;
			}
		}

		public DepositRefill() : base()
		{
			Type = 310;
			Attachment = new Attachments.DepositRefillAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public DepositRefill(int version) : this()
		{
			Version = version;

		}
		public DepositRefill(string sender, long amount, int deadline = 3600, long fee = 10, int version = 2) : this(version)
		{
			this.Amount = amount;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
		}
		public DepositRefill(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}

		public void SignTransaction(byte[] expandedPrivateKey)
		{
			var beattach = AttachmentToBEncoding();
			base.SignTransaction(beattach, expandedPrivateKey);
		}
		public void ConfirmTransaction(string accountId, byte[] expandedPrivateKey)
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
