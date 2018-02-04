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

		Task<State> GetStateAsync(String id);

		Task<Info> GetInformationAsync(String id);

		Task<EONBalance> GetBalanceAsync(String id);



		//Private methods, kept here for future reference
		//Task<string> GetAccount(String id);
	}
}
