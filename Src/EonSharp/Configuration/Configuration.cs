using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp
{
	public static class Configuration
	{

		//public static string NetworkId { get; set; } = "EON-B-PASRV-6ATZH-6ML5Z";

		/// <summary>
		/// Is used in Transaction processing.
		/// Afects all transport contexts for now.
		/// </summary>
		public static string NetworkId { get; set; } = "EON-B-2UUHB-F79EY-TWFRY";

		/// <summary>
		/// Usefull only during beta. Default is false.
		/// Afects all transport contexts for now.
		/// </summary>
		public static bool IgnoreSslErrors { get; set; } = false;

		static Configuration()
		{

		}




	}
}
