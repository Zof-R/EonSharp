using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public interface IMetrics
	{
		Task<string> GetPageAsync();
	}
}
