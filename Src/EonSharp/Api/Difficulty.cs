using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public class Difficulty:ISerializable
	{
		public BigInteger CumulativeDifficulty { get; set; }
		public string LastBlockID { get; set; }

		public Difficulty()
		{
		}

		static Dictionary<string, Action<SerializationInfo, Difficulty>> s_entryDict = new Dictionary<string, Action<SerializationInfo, Difficulty>>
		{
			{ "difficulty",(info, i)=> i.CumulativeDifficulty = (BigInteger)info.GetValue("difficulty", typeof(BigInteger)) },
			{ "last",(info, i)=> i.LastBlockID = info.GetString("last") },
		};
		public Difficulty(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				if (s_entryDict.TryGetValue(entry.Name, out Action<SerializationInfo, Difficulty> exec))
				{
					exec.Invoke(info, this);
				}
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("difficulty", CumulativeDifficulty);
			info.AddValue("last", LastBlockID);
		}
	}
}
