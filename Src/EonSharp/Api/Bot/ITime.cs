using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Bot
{
	public interface ITime
	{
		//com.exscudo.eon.bot.TimeService
		Task<long> GetAsync();
	}
}
