using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Network;

namespace EonSharp.Api.Metrics
{
	public class MetricsEndpoint : EndpointBase, IMetrics
	{
		public MetricsEndpoint(ITransportContext client) : base(client, "metrics")
		{

		}

		public async Task<string> GetPageAsync()
		{
			return await m_transportClient.GetPageAsync(EndpointUrl);
		}


	}
}
