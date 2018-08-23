using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public class Block : ISerializable
	{
		public BigInteger CumulativeDifficulty { get; set; }
		public string GenerationSignature { get; set; }
		public string Generator { get; set; }
		public int Height { get; set; }
		public string Id { get; set; }
		public string PreviousBlock { get; set; }
		public string Signature { get; set; }
		public string Snapshot { get; set; }
		public int Timestamp { get; set; }
		public int Version { get; set; }
		public IEnumerable<Transaction> Transactions { get; set; }



		static Dictionary<string, Action<SerializationInfo, Block>> s_entryDict = new Dictionary<string, Action<SerializationInfo, Block>>
		{
			{ "difficulty",(info, i)=> i.CumulativeDifficulty = (BigInteger)info.GetValue("difficulty", typeof(BigInteger)) },
			{ "generation",(info, i)=> i.GenerationSignature = info.GetString("generation") },
			{ "generator",(info, i)=> i.Generator = info.GetString("generator") },
			{ "height",(info, i)=> i.Height = info.GetInt32("height") },
			{ "id",(info, i)=> i.Id = info.GetString("id") },
			{ "prev",(info, i)=> i.PreviousBlock = info.GetString("prev") },
			{ "signature",(info, i)=> i.Signature = info.GetString("signature") },
			{ "snapshot",(info, i)=> i.Snapshot = info.GetString("snapshot") },
			{ "timestamp",(info, i)=> i.Timestamp = info.GetInt32("timestamp") },
			{ "version",(info, i)=> i.Version = info.GetInt32("version") },
			{ "transactions",(info, i)=> i.Transactions = (IEnumerable<Transaction>)info.GetValue("transactions", typeof(IEnumerable<Transaction>)) }
		};
		public Block(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				if (s_entryDict.TryGetValue(entry.Name, out Action<SerializationInfo, Block> exec))
				{
					exec.Invoke(info, this);
				}
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("difficulty", CumulativeDifficulty);
			info.AddValue("generation", GenerationSignature);
			info.AddValue("generator", Generator);
			info.AddValue("height", Height);
			info.AddValue("id", Id);
			info.AddValue("prev", PreviousBlock);
			info.AddValue("signature", Signature);
			info.AddValue("snapshot", Snapshot);
			info.AddValue("timestamp", Timestamp);
			info.AddValue("version", Version);
			info.AddValue("transactions", Transactions);
		}

	}
}
