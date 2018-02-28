using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class AccountRegistration : Transaction
	{

		public string AccountId
		{
			get
			{
				return ((Attachments.AccountRegistrationAttachment)Attachment).AccountId;
			}
			set
			{
				((Attachments.AccountRegistrationAttachment)Attachment).AccountId = value;
			}
		}

		public string PublicKey
		{
			get
			{
				return ((Attachments.AccountRegistrationAttachment)Attachment).PublicKey;
			}
			set
			{
				((Attachments.AccountRegistrationAttachment)Attachment).PublicKey = value;
			}
		}

		public AccountRegistration() : base()
		{
			Type = 100;
			Attachment = new Attachments.AccountRegistrationAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public AccountRegistration(int version) : this()
		{
			Version = version;

		}
		public AccountRegistration(string sender, string id, string publicKey, int deadline = 3600, long fee = 10, int version = 2) : this(version)
		{
			AccountId = id;
			PublicKey = publicKey;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
		}
		public AccountRegistration(SerializationInfo info, StreamingContext context) : base(info, context)
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
