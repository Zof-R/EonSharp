using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	/** 
 	 * Type of transaction confirmation 
 	 * <p> 
 	 * The type determines the features of the account (e.g., Inbound transaction 
 	 * processing rules) 
 	 */
	public class SignType
	{
		public string Normal { get; } = "normal";
		public string Public { get; } = "public";
		public string MFA { get; } = "mfa";

		public SignType()
		{

		}

	}

}
