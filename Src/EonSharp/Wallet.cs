using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Generators;
using EonSharp.Keystore;

namespace EonSharp
{
	public class Wallet : ISerializable, INotifyPropertyChanged
	{
		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName]string propertyname = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
		}

		#endregion


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

		public PublicAccountGenerator AccountDetails
		{
			get => m_accountDetails;
			private set
			{
				m_accountDetails = value;
				OnPropertyChanged();
			}
		}
		PublicAccountGenerator m_accountDetails;

		public Wallet()
		{
		}
		public Wallet(string name, string password)
		{
			Name = name;
			var privatekey = SeedGenerator.NewSeed();
			Keystore = new KeystoreV1(privatekey, password);
			AccountDetails = new PublicAccountGenerator(privatekey);
		}
		public Wallet(string name, byte[] privateKey, string password)
		{
			Name = name;
			Keystore = new KeystoreV1(privateKey, password);
			AccountDetails = new PublicAccountGenerator(privateKey);
		}


		#region ISerialization

		public Wallet(SerializationInfo info, StreamingContext context)
		{
			Id = info.GetString("id");
			Name = info.GetString("name");
			Version = info.GetInt32("version");
			Keystore = info.GetValue("keystore", typeof(KeystoreV1)) as IKeystore;
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("id", Id);
			info.AddValue("name", Name);
			info.AddValue("version", Version);
			info.AddValue("keystore", Keystore);
		}

		#endregion

		public void UnlockAccountDetails(string password)
		{
			if (AccountDetails == null)
			{
				UnlockAccountDetails(Keystore.GetPrivateKey(password));
			}
		}

		public byte[] GetPrivateKey(string password)
		{
			var pv = Keystore.GetPrivateKey(password);
			UnlockAccountDetails(pv);
			return pv;
		}
		public byte[] GetExpandedKey(string password)
		{
			return PublicKeyPairGenerator.ComputeExpandedPrivateKey(GetPrivateKey(password));
		}
		public void GetPrivateAndExpandedKeys(string password, out byte[] privatekey, out byte[] expandedprivatekey)
		{
			privatekey = GetPrivateKey(password);
			expandedprivatekey = PublicKeyPairGenerator.ComputeExpandedPrivateKey(privatekey);
		}

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

		void UnlockAccountDetails(byte[] privatekey)
		{
			if (AccountDetails == null)
			{
				AccountDetails = new PublicAccountGenerator(privatekey);
			}
		}
	}
}
