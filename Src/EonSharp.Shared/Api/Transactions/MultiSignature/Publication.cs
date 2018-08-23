using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class Publication : Transaction
	{
		public string Seed
		{
			get
			{
				return ((Attachments.PublicationAttachment)Attachment).Seed;
			}
			set
			{
				((Attachments.PublicationAttachment)Attachment).Seed = value;
			}
		}

		public Publication() : base()
		{
			Type = 430;
			Attachment = new Attachments.PublicationAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}
		public Publication(int version) : this()
		{
			Version = version;

		}
		public Publication(string sender, string seed, int deadline = 3600, long fee = 10, int version = 1) : this(version)
		{
			Seed = seed;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
			Version = version;
		}
		public Publication(SerializationInfo info, StreamingContext context) : base(info, context)
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
