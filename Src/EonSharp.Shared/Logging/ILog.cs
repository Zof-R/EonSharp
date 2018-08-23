using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Network;

namespace EonSharp.Logging
{
	public interface ILog
	{
		string Prefix { get; set; }

		ITransportContext Context { get; }

		event EventHandler<LogMessage> LogChanged;
	}
}
