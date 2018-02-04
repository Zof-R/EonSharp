using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Bot
{
	public interface ITransactions
	{
		//com.exscudo.eon.bot.TransactionService
		Task PutTransactionAsync(Transaction tx);

	}
}
