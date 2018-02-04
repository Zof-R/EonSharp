using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EonSharp.Api.Transactions.Attachments
{
	public class DepositWithdrawAttachment
	{
		public long Amount { get; set; }

		public DepositWithdrawAttachment()
		{

		}

	}
}
