using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class Registration : Transaction
	{

		public string AccountId
		{
			get
			{
				return ((Attachments.RegistrationAttachment)Attachment).AccountId;
			}
			set
			{
				((Attachments.RegistrationAttachment)Attachment).AccountId = value;
			}
		}

		public string PublicKey
		{
			get
			{
				return ((Attachments.RegistrationAttachment)Attachment).PublicKey;
			}
			set
			{
				((Attachments.RegistrationAttachment)Attachment).PublicKey = value;
			}
		}

		public Registration() : base()
		{
			Type = 100;
			Attachment = new Attachments.RegistrationAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public Registration(int version) : this()
		{
			Version = version;

		}
		public Registration(string sender, string id, string publicKey, int deadline = 3600, long fee = 10, int version = 1) : this(version)
		{
			AccountId = id;
			PublicKey = publicKey;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
			Version = version;
		}
		public Registration(SerializationInfo info, StreamingContext context) : base(info, context)
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
				[AccountId] = new BEncoding.BString(PublicKey),
			};
		}
	}
}
