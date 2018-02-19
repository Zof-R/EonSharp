using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Bot
{
	public interface IAccounts
	{
		//com.exscudo.eon.bot.AccountService

		Task<State> GetStateAsync(String id);
		Task<Info> GetInformationAsync(String id);



	}
}
