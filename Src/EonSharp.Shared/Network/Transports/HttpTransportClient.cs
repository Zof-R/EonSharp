using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using EonSharp.Protocol;

namespace EonSharp.Network.Transports
{
	public class HttpTransportClient : ITransportContext
	{

		public Uri ServerAddress { get; private set; }
		public string User { get; private set; }
		public string Password { get; private set; }

		public bool IsSecondaryContext { get; set; }

		internal HttpClient m_client;
		internal AuthenticationHeaderValue m_authHeader;

#if !NETCOREAPP2_1
		static HttpTransportClient()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
			{
				if (sslPolicyErrors == SslPolicyErrors.None || Configuration.IgnoreSslErrors)
				{
					return true;
				}
				return false;
			};
		}
#endif

		public HttpTransportClient(string serverAddress, string user = null, string password = null) : this(null, serverAddress, user, password)
		{

		}
		internal HttpTransportClient(HttpClient client, string serverAddress, string user, string password)
		{

			//DNS cache lease time control
			//var spoint = ServicePointManager.FindServicePoint(new Uri(serverAddress));
			//spoint.ConnectionLeaseTimeout = 60 * 1000;

			if (client == null)
			{
#if NETCOREAPP2_1

				var handler = new HttpClientHandler
				{
					ServerCertificateCustomValidationCallback = Configuration.IgnoreSslErrors ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator : null
				};
				//handler.CookieContainer = new System.Net.CookieContainer();
				//handler.UseCookies = true;
				m_client = new HttpClient(handler);
#else
				m_client = new HttpClient();
#endif

				m_client.DefaultRequestHeaders.Accept.Clear();
				m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
				//m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			}
			else
			{
				m_client = client;
				IsSecondaryContext = true;
			}

			if (Uri.TryCreate(serverAddress, UriKind.Absolute, out Uri srvaddr))
			{
				ServerAddress = srvaddr;
			}
			else
			{
				throw new UriFormatException("Malformed address.");
			}

			User = user;
			Password = password;

			if (user != null)
			{
				var byteArray = new UTF8Encoding().GetBytes($"{user}:{password}");
				m_authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
			}

		}


		public ITransportContext CreateNewTransportContext(string serverAddress, string user = null, string password = null)
		{
			return new HttpTransportClient(m_client, serverAddress, user, password);
		}

		public async Task<RpcResponse> ProcessCommandAsync(string endpoint, string method, int id, IEnumerable<object> @params = null)
		{
			return await ProcessCommandAsync(endpoint, new RpcRequest(method, @params, id));
		}

		public async Task<RpcResponse> ProcessCommandAsync(string endpointUrl, RpcRequest rpcrequest)
		{
			return await ProcessCommandAsync(endpointUrl, rpcrequest.ToString());
		}

		public async Task<RpcResponse> ProcessCommandAsync(string endpointUrl, string rpcrequest)
		{
			if (ServerAddress == null)
			{
				throw new Exception("ServerAddress is required");

			}
			using (var requestMessage = CreateRequestFromRpc(endpointUrl, rpcrequest))
			using (var result = await m_client.SendAsync(requestMessage))
			{
				result.EnsureSuccessStatusCode();
				if (result.IsSuccessStatusCode)
				{
					var rpcresponse = await result.Content.ReadAsStringAsync();
					RpcResponse res = rpcresponse;
					res.RawRpcRequest = rpcrequest;
					res.RawRpcResponse = rpcresponse;
					if (res.Error != null)
					{
						throw new ProtocolException(res.Error, rpcrequest, rpcresponse);
					}
#if DEBUG
					System.Diagnostics.Debug.WriteLine(rpcrequest);
					System.Diagnostics.Debug.WriteLine(rpcresponse);
					System.Diagnostics.Debug.WriteLine("");
#endif
					return res;
				}
			}
			return null;
		}

		public async Task<string> GetPageAsync(string endpointUrl)
		{
			if (ServerAddress == null)
			{
				throw new Exception("ServerAddress is required");

			}
			using (var httpRequestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(ServerAddress, endpointUrl),
			})
			{
				httpRequestMessage.Headers.UserAgent.Add(new ProductInfoHeaderValue("eonsharp", "1.0"));
				if (m_authHeader != null)
				{
					httpRequestMessage.Headers.Authorization = m_authHeader;
				}

				using (var result = await m_client.SendAsync(httpRequestMessage))
				{
					result.EnsureSuccessStatusCode();
					if (result.IsSuccessStatusCode)
					{
						var res = await result.Content.ReadAsStringAsync();
#if DEBUG
						System.Diagnostics.Debug.WriteLine(res);
						System.Diagnostics.Debug.WriteLine("");
#endif
						return res;
					}
				}
			}
			return null;
		}


		HttpRequestMessage CreateRequestFromRpc(string endpoint, string rpcrequest)
		{
			var httpRequestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri(ServerAddress, endpoint),
				Content = new StringContent(rpcrequest, Encoding.UTF8, "application/json"),
			};
			httpRequestMessage.Headers.UserAgent.Add(new ProductInfoHeaderValue("eonsharp", "1.0"));
			if (m_authHeader != null)
			{
				httpRequestMessage.Headers.Authorization = m_authHeader;
			}
			return httpRequestMessage;
		}


		public void Dispose()
		{
			if (m_client != null && !IsSecondaryContext)
			{
				m_client.Dispose();
				m_client = null;
			}
		}
	}
}
