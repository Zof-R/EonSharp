using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Transactions.Attachments
{
	public class ColoredCoinPaymentAttachment
	{

		public long Amount { get; set; }
		public string Color { get; set; }
		public string Recipient { get; set; }

		public ColoredCoinPaymentAttachment()
		{

		}

	
	}
}
