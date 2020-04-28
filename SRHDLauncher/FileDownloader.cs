using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public static class FileDownloader
	{
		private static bool abortUpdate = false;

		private const string GOOGLE_DRIVE_DOMAIN = "drive.google.com";

		private const string GOOGLE_DRIVE_DOMAIN2 = "https://drive.google.com";

		private static bool downloadCompleted = false;

		private static bool progressBar = false;

		private static update currentUpdateForm;

		private static mainform currentMainMenuForm;

		private static long TotalBytesToUpdate;

		private static string currentMessage = "";

		private static WebClient copy = null;

		private static bool downloadInterrupted = false;

		public static bool getSetAbourt(bool set)
		{
			abortUpdate = set;
			return abortUpdate;
		}

		public static void OpenUrl(string url)
		{
			try
			{
				Process.Start(url);
			}
			catch
			{
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					url = url.Replace("&", "^&");
					Process.Start(new ProcessStartInfo("cmd", "/c start " + url)
					{
						CreateNoWindow = true
					});
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					Process.Start("xdg-open", url);
				}
				else
				{
					if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					{
						throw;
					}
					Process.Start("open", url);
				}
			}
		}

		public static FileInfo DownloadFileFromURLToPath(string url, string path, bool callProgressBar, update updateForm, mainform mainMenuForm, string message)
		{
			try
			{
				currentMessage = message;
				currentUpdateForm = updateForm;
				currentMainMenuForm = mainMenuForm;
				if (url.StartsWith("drive.google.com") || url.StartsWith("https://drive.google.com"))
				{
					return DownloadGoogleDriveFileFromURLToPath(url, path, callProgressBar);
				}
				return DownloadFileFromURLToPath(url, path, null, callProgressBar);
			}
			catch (Exception ex)
			{
				mainMenuForm.internetIsAbsent = true;
				MessageBox.Show(ex.ToString());
				return null;
			}
		}

		public static FileInfo DownloadFileFromURLToPath(string url, string path, bool callProgressBar, update updateForm, mainform mainMenuForm, long totalBytesToUpdate, string message)
		{
			TotalBytesToUpdate = totalBytesToUpdate;
			currentUpdateForm = updateForm;
			currentMainMenuForm = mainMenuForm;
			currentMessage = message;
			if (url.StartsWith("drive.google.com") || url.StartsWith("https://drive.google.com"))
			{
				return DownloadGoogleDriveFileFromURLToPath(url, path, callProgressBar);
			}
			return DownloadFileFromURLToPath(url, path, null, callProgressBar);
		}

		public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
		{
			Task waitTask = Task.Run(async delegate
			{
				while (!condition())
				{
					await Task.Delay(frequency);
				}
			});
			object obj = waitTask;
			if (obj != await Task.WhenAny(waitTask, Task.Delay(timeout)))
			{
				throw new TimeoutException();
			}
		}

		public static void setDownloadInterrupted(bool itIs)
		{
			downloadInterrupted = itIs;
		}

		private static FileInfo DownloadFileFromURLToPath(string url, string path, WebClient webClient, bool callProgressBar)
		{
			progressBar = callProgressBar;
			if (callProgressBar)
			{
				int num = 0;
			}
			try
			{
				if (webClient == null)
				{
					using (webClient = new WebClient())
					{
						webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
						webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
						webClient.DownloadFileAsync(new Uri(url), path);
						WebClient webClient2 = webClient;
						if (!downloadInterrupted)
						{
							if (currentUpdateForm != null)
							{
								currentUpdateForm.RepeatUntilBusy(webClient);
							}
							if (currentMainMenuForm != null)
							{
								currentMainMenuForm.RepeatUntilBusy(webClient);
							}
							return new FileInfo(path);
						}
						return null;
					}
				}
				webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
				webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
				webClient.DownloadFileAsync(new Uri(url), path);
				if (!downloadInterrupted)
				{
					if (currentUpdateForm != null)
					{
						currentUpdateForm.RepeatUntilBusy(webClient);
					}
					if (currentMainMenuForm != null)
					{
						currentMainMenuForm.RepeatUntilBusy(webClient);
					}
					return new FileInfo(path);
				}
				return null;
			}
			catch (WebException)
			{
				currentMainMenuForm.internetIsAbsent = true;
				return null;
			}
		}

		private static bool ReturnDownloadCompletedFlag()
		{
			return downloadCompleted;
		}

		private static void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			if (progressBar)
			{
				double num = double.Parse(e.BytesReceived.ToString());
				double num2 = double.Parse(TotalBytesToUpdate.ToString());
				double d = num / num2 * 100.0;
				int num3 = (int)e.BytesReceived / 1048576;
				int num4 = (int)TotalBytesToUpdate / 1048576;
				if (currentMainMenuForm.Lang == "ru")
				{
					currentUpdateForm.progressBar.CustomText = currentMessage + "   " + num3 + "МБ of " + num4 + "МБ";
				}
				if (currentMainMenuForm.Lang == "eng")
				{
					currentUpdateForm.progressBar.CustomText = currentMessage + "   " + num3 + "MB of " + num4 + "MB";
				}
		     currentUpdateForm.progressBar.Value = Math.Min(int.Parse(Math.Truncate(d).ToString()), 100);
			}
		}

		private static void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (progressBar && currentUpdateForm.temp.Value != 0)
			{
				currentUpdateForm.temp.Value = 100;
			}
		}

		private static FileInfo DownloadGoogleDriveFileFromURLToPath(string url, string path, bool callProgressBar)
		{
			url = GetGoogleDriveDownloadLinkFromUrl(url);
			using (CookieAwareWebClient webClient = new CookieAwareWebClient())
			{
				for (int i = 0; i < 2; i++)
				{
					FileInfo fileInfo = DownloadFileFromURLToPath(url, path, webClient, callProgressBar);
					if (fileInfo == null)
					{
						return null;
					}
					if (fileInfo.Length > 60000)
					{
						return fileInfo;
					}
					string text;
					using (StreamReader streamReader = fileInfo.OpenText())
					{
						char[] array = new char[20];
						int num = streamReader.ReadBlock(array, 0, 20);
						if (num < 20 || !new string(array).Contains("<!DOCTYPE html>"))
						{
							return fileInfo;
						}
						text = streamReader.ReadToEnd();
					}
					int num2 = text.LastIndexOf("href=\"/uc?");
					if (num2 < 0)
					{
						return fileInfo;
					}
					num2 += 6;
					int num3 = text.IndexOf('"', num2);
					if (num3 < 0)
					{
						return fileInfo;
					}
					url = "https://drive.google.com" + text.Substring(num2, num3 - num2).Replace("&amp;", "&");
				}
				return DownloadFileFromURLToPath(url, path, webClient, callProgressBar);
			}
		}

		public static string GetGoogleDriveDownloadLinkFromUrl(string url)
		{
			int num = url.IndexOf("id=");
			int num2;
			if (num > 0)
			{
				num += 3;
				num2 = url.IndexOf('&', num);
				if (num2 < 0)
				{
					num2 = url.Length;
				}
			}
			else
			{
				num = url.IndexOf("file/d/");
				if (num < 0)
				{
					return string.Empty;
				}
				num += 7;
				num2 = url.IndexOf('/', num);
				if (num2 < 0)
				{
					num2 = url.IndexOf('?', num);
					if (num2 < 0)
					{
						num2 = url.Length;
					}
				}
			}
			return $"https://drive.google.com/uc?id={url.Substring(num, num2 - num)}&export=download";
		}
	}
}
