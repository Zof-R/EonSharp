using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Keystore;

namespace EonSharp
{
	public class Wallet : ISerializable
	//: Dictionary<string, object>
	{
		/// <summary>
		/// Serialization of Guid
		/// </summary>
		public string Id { get; set; } = Guid.NewGuid().ToString();

		/// <summary>
		/// Wallet's name
		/// </summary>
		public string Name { get; set; } = "Main";

		/// <summary>
		/// Version of wallet protocol
		/// </summary>
		public int Version { get; private set; } = 1;

		/// <summary>
		/// Container of cryptographic functions used to obtain the Seed/Private key
		/// </summary>
		public IKeystore Keystore { get; set; }


		public Wallet()
		{
		}

		public Wallet(string name, byte[] privateKey, string password)
		{
			Name = name;
			Keystore = new KeystoreV1(privateKey, password);
		}


		public Wallet(SerializationInfo info, StreamingContext context)
		//: base(info, context)
		{
			Id = info.GetString("id");
			Name = info.GetString("name");
			Version = info.GetInt32("version");
			Keystore = info.GetValue("keystore", typeof(KeystoreV1)) as IKeystore;
		}

		//public override void GetObjectData(SerializationInfo info, StreamingContext context)
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			//base.GetObjectData(info, context);
			info.AddValue("id", Id);
			info.AddValue("name", Name);
			info.AddValue("version", Version);
			info.AddValue("keystore", Keystore);
		}

		public Generators.PublicAccountGenerator GetPublicAccountGenerator(string password)
		{
			return new Generators.PublicAccountGenerator(Keystore.GetPrivateKey(password));
		}
		public byte[] GetPrivateKey(string password)
		{
			return Keystore.GetPrivateKey(password);
		}
		public void GetPrivateAndExpandedKeys(string password, out byte[] privatekey, out byte[] expandedprivatekey)
		{
			privatekey = GetPrivateKey(password);
			expandedprivatekey = Generators.PublicKeyPairGenerator.ComputeExpandedPrivateKey(privatekey);
		}

		//public bool TryGetValue<T>(string key, out T value)
		//{
		//	return this.TryGetValue<T>(key, out value);
		//}

		public string ToJsonString()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings
			{
				ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
				MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
				NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
			});
		}
		public static Wallet FromJsonString(string json)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<Wallet>(json);
		}


	}
}
