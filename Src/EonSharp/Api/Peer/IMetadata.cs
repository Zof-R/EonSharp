using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api.Peer
{
	public interface IMetadata
	{

		//metadata.com.exscudo.peer.eon.stubs.SyncMetadataService

		Task<Attributes> GetAttributesAsync();
		Task<IEnumerable<string>> GetWellKnownNodesAsync();
		Task<bool> AddPeerAsync(long peerId, string peerAddress);


		
	}
}
