using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	/** 
 	 * Account state 
 	 */
	public class Info : ISerializable
	{
		public State State { get; set; }
		public string PublicKey { get; set; }
		public long Amount { get; set; }
		public long Deposit { get; set; }


		public string SignType { get; set; }
		public VotingRights VotingRights { get; set; }
		public Quorum Quorum { get; set; }
		public string Seed { get; set; }
		public IDictionary<string, int> Voter { get; set; }

		public string ColoredCoin { get; set; }


		public Info()
		{

		}

		static Dictionary<string, Action<SerializationInfo, Info>> s_entryDict = new Dictionary<string, Action<SerializationInfo, Info>>
		{
			{ "state",(info, i)=> i.State = (State)info.GetValue("state", typeof(State)) },
			{ "public_key",(info, i)=> i.PublicKey = info.GetString("public_key") },
			{ "amount",(info, i)=> i.Amount = info.GetInt64("amount") },
			{ "deposit",(info, i)=> i.Deposit = info.GetInt64("deposit") },
			{ "sign_type",(info, i)=> i.SignType = info.GetString("sign_type") },
			{ "voting_rights",(info, i)=> i.VotingRights = (VotingRights)info.GetValue("voting_rights", typeof(VotingRights)) },
			{ "quorum",(info, i)=> i.Quorum = (Quorum)info.GetValue("quorum", typeof(Quorum)) },
			{ "seed",(info, i)=> i.Seed = info.GetString("seed") },
			{ "voter",(info, i)=> i.Voter = (Dictionary<string,int>)info.GetValue("voter", typeof(Dictionary<string, int>)) },
			{ "colored_coin",(info, i)=> i.ColoredCoin = info.GetString("colored_coin") },
		};
		public Info(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				if (s_entryDict.TryGetValue(entry.Name, out Action<SerializationInfo, Info> exec))
				{
					exec.Invoke(info, this);
				}
			}
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("state", State);
			info.AddValue("public_key", PublicKey);
			info.AddValue("amount", Amount);
			info.AddValue("deposit", Deposit);
			info.AddValue("sign_type", SignType);
			info.AddValue("voting_rights", VotingRights);
			info.AddValue("quorum", Quorum);
			info.AddValue("seed", Seed);
			info.AddValue("voter", Voter);
			info.AddValue("colored_coin", ColoredCoin);
		}
	}

}
