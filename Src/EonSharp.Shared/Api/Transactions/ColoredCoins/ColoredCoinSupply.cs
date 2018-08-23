using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class ColoredCoinSupply : Transaction
	{
		public long Supply
		{
			get
			{
				return ((Attachments.ColoredCoinSupplyAttachment)Attachment).Supply;
			}
			set
			{
				((Attachments.ColoredCoinSupplyAttachment)Attachment).Supply = value;
			}
		}

		public ColoredCoinSupply() : base()
		{
			Type = 520;
			Attachment = new Attachments.ColoredCoinSupplyAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}
		public ColoredCoinSupply(int version) : this()
		{
			Version = version;

		}
		public ColoredCoinSupply(string sender, long moneySupply, int deadline = 3600, long fee = 10, int version = 1) : this(version)
		{
			Supply = moneySupply;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
			Version = version;
		}
		public ColoredCoinSupply(SerializationInfo info, StreamingContext context) : base(info, context)
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
				["supply"] = new BEncoding.BInteger(Supply)
			};
		}
	}
}
