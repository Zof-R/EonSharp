using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EonSharp.Api.Transactions.Attachments
{
	public class PaymentAttachment
	{
		public long Amount { get; set; }

		public string Recipient { get; set; }

		public PaymentAttachment()
		{

		}


	}
}
