using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class ColoredCoinRegistration : Transaction
	{

		public long Emission
		{
			get
			{
				return ((Attachments.ColoredCoinRegistrationAttachment)Attachment).Emission;
			}
			set
			{
				((Attachments.ColoredCoinRegistrationAttachment)Attachment).Emission = value;
			}
		}
		public int Decimal
		{
			get
			{
				return ((Attachments.ColoredCoinRegistrationAttachment)Attachment).Decimal;
			}
			set
			{
				((Attachments.ColoredCoinRegistrationAttachment)Attachment).Decimal = value;
			}
		}

		public ColoredCoinRegistration() : base()
		{
			Type = 500;
			Attachment = new Attachments.ColoredCoinRegistrationAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}
		public ColoredCoinRegistration(int version) : this()
		{
			Version = version;

		}
		public ColoredCoinRegistration(string sender, long emission, int decimalPoint, int deadline = 3600, long fee = 10, int version = 1) : this(version)
		{
			Emission = emission;
			Decimal = decimalPoint;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
			Version = version;
		}
		public ColoredCoinRegistration(SerializationInfo info, StreamingContext context) : base(info, context)
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
				[nameof(Emission).ToLower()] = new BEncoding.BInteger(Emission),
				["decimal"] = new BEncoding.BInteger(Decimal)
			};
		}

	}
}
