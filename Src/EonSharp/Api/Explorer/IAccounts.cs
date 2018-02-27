using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Explorer
{
	public interface IAccounts
	{
		//com.exscudo.eon.bot.AccountService

		Task<State> GetStateAsync(string id);

		Task<Balance> GetBalanceAsync(string id);

		Task<Info> GetInformationAsync(string id);




		//Private methods, kept here for future reference
		//Task<string> GetAccount(String id);
	}
}
