using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public class CookieAwareWebClient : WebClient
	{
		private class CookieContainer
		{
			private Dictionary<string, string> _cookies;

			public string this[Uri url]
			{
				get
				{
					if (_cookies.TryGetValue(url.Host, out string value))
					{
						return value;
					}
					return null;
				}
				set
				{
					_cookies[url.Host] = value;
				}
			}

			public CookieContainer()
			{
				_cookies = new Dictionary<string, string>();
			}
		}

		private CookieContainer cookies;

		public CookieAwareWebClient()
		{
			cookies = new CookieContainer();
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest webRequest = base.GetWebRequest(address);
			if (webRequest != null)
			{
				if (webRequest is HttpWebRequest)
				{
					string text = cookies[address];
					if (text != null)
					{
						((HttpWebRequest)webRequest).Headers.Set("cookie", text);
					}
				}
				return webRequest;
			}
			return null;
		}

		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			try
			{
				WebResponse webResponse = base.GetWebResponse(request, result);
				string[] values = webResponse.Headers.GetValues("Set-Cookie");
				if (values != null && values.Length != 0)
				{
					string text = "";
					string[] array = values;
					foreach (string str in array)
					{
						text += str;
					}
					cookies[webResponse.ResponseUri] = text;
				}
				return webResponse;
			}
			catch (Exception ex)
			{
				FileDownloader.setDownloadInterrupted(itIs: true);
				return null;
			}
		}

		protected override WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse webResponse = base.GetWebResponse(request);
			string[] values = webResponse.Headers.GetValues("Set-Cookie");
			if (values != null && values.Length != 0)
			{
				string text = "";
				string[] array = values;
				foreach (string str in array)
				{
					text += str;
				}
				cookies[webResponse.ResponseUri] = text;
			}
			return webResponse;
		}
	}
}