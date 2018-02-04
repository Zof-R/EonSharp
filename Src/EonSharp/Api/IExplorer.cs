using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Api.Explorer;

namespace EonSharp.Api
{
	public interface IExplorer
	{
		Api.Explorer.IAccounts Accounts { get; }
		Api.Explorer.IExplorer Explorer { get; }
	}
}
