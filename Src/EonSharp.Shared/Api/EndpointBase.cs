using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Network;

namespace EonSharp.Api
{
	public class EndpointBase
	{
		public string EndpointUrl { get; private set; }

		internal ITransportContext m_transportClient;

		public EndpointBase(ITransportContext client, string endpointUrl)
		{
			m_transportClient = client ?? throw new Exception("client can not be null");
			EndpointUrl = endpointUrl;
		}


	}
}
