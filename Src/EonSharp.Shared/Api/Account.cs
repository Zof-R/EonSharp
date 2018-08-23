using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EonSharp.Api
{
	public class Account : ISerializable
	{
		public Account()
		{

		}


		static Dictionary<string, Action<SerializationInfo, Account>> s_entryDict = new Dictionary<string, Action<SerializationInfo, Account>>
		{
			{ "balance",(info, i)=> i.Balance = (AccountBalance)info.GetValue("balance", typeof(AccountBalance)) },
			{ "public-key",(info, i)=> i.PublicKey = (AccountPublicKey)info.GetValue("public-key", typeof(AccountPublicKey)) },
			{ "id",(info, i)=> i.Id= info.GetString("id")}
		};
		public Account(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry entry in info)
			{
				if (s_entryDict.TryGetValue(entry.Name, out Action<SerializationInfo, Account> exec))
				{
					exec.Invoke(info, this);
				}
			}
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("balance", Balance);
			info.AddValue("public-key", PublicKey);
			info.AddValue("id", Id);
		}


		public AccountBalance Balance { get; set; }
		public AccountPublicKey PublicKey { get; set; }
		public string Id { get; set; }


	}
}
