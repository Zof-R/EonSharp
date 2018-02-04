using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions
{
	public class Delegate : Transaction
	{
		public string Account
		{
			get
			{
				return ((Attachments.DelegateAttachment)Attachment).Account;
			}
			set
			{
				((Attachments.DelegateAttachment)Attachment).Account = value;
			}
		}
		public int Weight
		{
			get
			{
				return ((Attachments.DelegateAttachment)Attachment).Weight;
			}
			set
			{
				((Attachments.DelegateAttachment)Attachment).Weight = value;
			}
		}

		public Delegate() : base()
		{
			Type = 425;
			Attachment = new Attachments.DelegateAttachment();
			Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
		}

		public Delegate(int version) : this()
		{
			Version = version;

		}
		public Delegate(string sender, string account,int weight, int deadline = 3600, long fee = 10, int version = 2) : this(version)
		{
			Account = account;
			Weight = weight;
			Sender = sender;
			Deadline = deadline;
			Fee = fee;
		}

		public Delegate(SerializationInfo info, StreamingContext context) : base(info, context)
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
				[nameof(Account).ToLower()] = new BEncoding.BString(Account),
				[nameof(Weight).ToLower()] = new BEncoding.BInteger(Weight)
			};
		}
	}
}
