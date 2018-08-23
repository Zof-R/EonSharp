using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Api.Bot;

namespace EonSharp.Api
{
	public interface IBot
	{
		IAccounts Accounts { get; }
		IHistory History { get; }
		ITime Time { get; }
		ITransactions Transactions { get; }
		IColoredCoin ColoredCoin { get; }
	}
}
