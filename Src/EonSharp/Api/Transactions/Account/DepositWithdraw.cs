using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class DepositWithdraw : Transaction
	{
		public long Amount
		{
			get
			{
				return ((Attachments.DepositWithdrawAttachment)Attachment).Amount;
			}
			set
			{
				((Attachments.DepositWithdrawAttachment)Attachment).Amount = value;
			}
		}

		public DepositWithdraw() : base()
		{
			Type = 320;
			Attachment = new Attachments.DepositWithdrawAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public DepositWithdraw(int version) : this()
		{
			Version = version;

		}
		public DepositWithdraw(string sender, long amount, int deadline = 3600, long fee = 10, int version = 2) : this(version)
		{
			this.Amount = amount;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
		}

		public DepositWithdraw(SerializationInfo info, StreamingContext context) : base(info, context)
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
