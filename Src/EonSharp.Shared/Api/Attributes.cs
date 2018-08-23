using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public class Attributes : ISerializable
	{
		public string AnnouncedAddress { get; set; }
		public string Application { get; set; }
		public long PeerID { get; set; }
		public string Version { get; set; }
		public string NetworkID { get; set; }
		public int Fork { get; set; }


		public Attributes()
		{

		}

		public Attributes(SerializationInfo info, StreamingContext context)
		{
			AnnouncedAddress = info.GetString("announced_address");
			Application = info.GetString("application");
			PeerID = info.GetInt64("peer_id");
			Version = info.GetString("version");
			NetworkID = info.GetString("network_id");
			Fork = info.GetInt32("fork");
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("announced_address", AnnouncedAddress);
			info.AddValue("application", Application);
			info.AddValue("peer_id", PeerID);
			info.AddValue("version", Version);
			info.AddValue("network_id", NetworkID);
			info.AddValue("fork", Fork);
		}
	}
}
