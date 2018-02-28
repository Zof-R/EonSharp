using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class AccountPublication : Transaction
	{
		public string Seed
		{
			get
			{
				return ((Attachments.AccountPublicationAttachment)Attachment).Seed;
			}
			set
			{
				((Attachments.AccountPublicationAttachment)Attachment).Seed = value;
			}
		}

		public AccountPublication() : base()
		{
			Type = 475;
			Attachment = new Attachments.AccountPublicationAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public AccountPublication(int version) : this()
		{
			Version = version;

		}
		public AccountPublication(string sender, string seed, int deadline = 3600, long fee = 10, int version = 2) : this(version)
		{
			Seed = seed;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
		}

		public AccountPublication(SerializationInfo info, StreamingContext context) : base(info, context)
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
				[nameof(Seed).ToLower()] = new BEncoding.BString(Seed)
			};
		}
	}
}
