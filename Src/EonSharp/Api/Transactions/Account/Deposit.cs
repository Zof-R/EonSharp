using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class Deposit : Transaction
	{
		public long Amount
		{
			get
			{
				return ((Attachments.DepositAttachment)Attachment).Amount;
			}
			set
			{
				((Attachments.DepositAttachment)Attachment).Amount = value;
			}
		}

		public Deposit() : base()
		{
			Type = 300;
			Attachment = new Attachments.DepositAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public Deposit(int version) : this()
		{
			Version = version;

		}
		public Deposit(string sender, long amount, int deadline = 3600, long fee = 10, int version = 1) : this(version)
		{
			this.Amount = amount;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
			Version = version;
		}
		public Deposit(SerializationInfo info, StreamingContext context) : base(info, context)
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
