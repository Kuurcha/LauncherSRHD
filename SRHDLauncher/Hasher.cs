using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Http;
using Google.Apis.Requests;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
namespace SRHDLauncher
{
	internal static class Hasher
	{
		//private static string[] Scopes = new string[3]
		//{
		//	Scope.Drive,
		//	Scope.DriveAppdata,
		//	Scope.DriveMetadata
		//};

		private static string ApplicationName = "Drive API .NET Quickstart";

		private static string GenerateHashString(string text)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(text));
			byte[] hash = mD5CryptoServiceProvider.Hash;
			return string.Join(string.Empty, hash.Select((byte x) => x.ToString("x2")));
		}

		public static bool TestForAppName(string appName, string filePath)
		{
			bool flag = true;
			if (appName.Length < 1000)
			{
				int num = 0;
				int num2 = 0;
				while (flag && num < appName.Length - 1)
				{
					if (appName[num] != filePath[num])
					{
						flag = false;
					}
					else
					{
						num2++;
					}
					num++;
				}
				if (num2 == 0)
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		public static List<string> ParseCfgFile(string[] cfgFileString)
		{
			List<string> list = new List<string>();
			if (cfgFileString != null)
			{
				foreach (string text in cfgFileString)
				{
					string text2 = text;
					text2 = text.Replace("CurrentMod=", "");
					int num = 0;
					int num2 = 0;
					for (num2 = 0; num2 < text2.Length - 1; num2++)
					{
						if (text2[num2] == ',')
						{
							string item = text2.Substring(num, num2 - num);
							list.Add(item);
							num = num2 + 2;
						}
					}
					list.Add(text2.Substring(num, num2 - num + 1));
				}
			}
			return list;
		}

		private static List<string> DirSearch(string sDir, List<string> files)
		{
			string value = "Debug";
			try
			{
				string[] files2 = Directory.GetFiles(sDir);
				foreach (string text in files2)
				{
					string text2 = text.Substring(text.IndexOf(value));
					for (int j = 0; j < text2.Length; j++)
					{
						if (text2[j] == '\\')
						{
							text2 = text2.Substring(j + 1);
							break;
						}
					}
					for (int k = 0; k < text2.Length; k++)
					{
						if (text2[k] == '\\')
						{
							text2.Insert(k + 1, "\\\\");
						}
					}
					files.Add(text2);
				}
				string[] directories = Directory.GetDirectories(sDir);
				foreach (string sDir2 in directories)
				{
					DirSearch(sDir2, files);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			return files;
		}

		public static List<string> GetAllDirectoriesInDirectories(string directory)
		{
			List<string> list = new List<string>();
			string[] directories = Directory.GetDirectories(directory);
			string[] array = directories;
			foreach (string path in array)
			{
				string[] directories2 = Directory.GetDirectories(path);
				string[] array2 = directories2;
				foreach (string item in array2)
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static List<List<string>> SortModsByFolderAndGetHash(List<string> filePaths, List<string> cfgFileList)
		{
			List<List<string>> list = new List<List<string>>();
			int num = 0;
			foreach (string cfgFile in cfgFileList)
			{
				StringBuilder stringBuilder = new StringBuilder();
				list.Add(new List<string>());
				list[num].Add(StringProcessing.getAppName(cfgFile));
				foreach (string filePath in filePaths)
				{
					if (filePath.Contains(cfgFile))
					{
						list[num].Add(filePath);
						FileStream fileStream = new FileStream(filePath, FileMode.Open);
						byte[] array = MD5.Create().ComputeHash(fileStream);
						byte[] array2 = array;
						foreach (byte b in array2)
						{
							stringBuilder.Append(b.ToString("X2").ToLower());
						}
						fileStream.Close();
					}
				}
				string item = GenerateHashString(stringBuilder.ToString());
				list[num].Insert(1, item);
				num++;
			}
			return list;
		}

		//public static string[,] IListToArray(IList<System.IO.File> iList, IList<System.IO.File> searchList, bool fullPath)
		//{
		//	string[,] array = new string[iList.Count + 1, 6];
		//	int num = 0;
		//	foreach (System.IO.File i in iList)
		//	{
		//		array[num, 0] = i.get_Name();
		//		array[num, 1] = i.get_Size().ToString();
		//		array[num, 2] = i.get_Id();
		//		array[num, 3] = i.get_Md5Checksum();
		//		array[num, 4] = i.get_WebContentLink();
		//		if (i.get_Parents() != null)
		//		{
		//			array[num, 5] = i.get_Parents()[0];
		//		}
		//		else
		//		{
		//			array[num, 5] = null;
		//		}
		//		num++;
		//	}
		//	return array;
		//}

		//public static System.IO.File RecursiveSearch(Google.Apis.Drive.v3.Data.File file, IList<System.IO.File> files, ref string s)
		//{
		//	if (file.get_Parents() != null)
		//	{
		//		string b = file.get_Parents()[0];
		//		foreach (Google.Apis.Drive.v3.Data.File file2 in files)
		//		{
		//			if (file2.get_Id() == b)
		//			{
		//				s = s + "\\" + file2.get_Name();
		//				file = RecursiveSearch(file2, files, ref s);
		//				break;
		//			}
		//		}
		//	}
		//	return file;
		//}

		//public static List<List<string[]>> getGoogleDiskModInfo(List<string> modList)
		//{
		//	//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//	//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//	//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//	//IL_005c: Expected O, but got Unknown
		//	//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//	//IL_005d: Expected O, but got Unknown
		//	string text = "1EPjALeLqG9dT3c6YGup30u6dcmxVhsSB";
		//	List<string[]> list = new List<string[]>();
		//	GoogleCredential httpClientInitializer;
		//	using (FileStream fileStream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
		//	{
		//		httpClientInitializer = GoogleCredential.FromStream((Stream)fileStream).CreateScoped(Scopes);
		//	}
		//	Initializer val = new Initializer();
		//	val.set_HttpClientInitializer((IConfigurableHttpClientInitializer)(object)httpClientInitializer);
		//	val.set_ApplicationName("servauth");
		//	DriveService val2 = (DriveService)(object)new DriveService((Initializer)(object)val);
		//	ListRequest val3 = val2.get_Files().List();
		//	val3.set_PageSize((int?)1000);
		//	((DriveBaseServiceRequest<FileList>)(object)val3).set_Fields("nextPageToken, files(id, name, md5Checksum, size, parents)");
		//	val3.set_Q("mimeType='application/vnd.google-apps.folder'");
		//	IList<File> files = ((ClientServiceRequest<FileList>)(object)val3).Execute().get_Files();
		//	string[,] array = IListToArray(files, null, fullPath: false);
		//	ListRequest val4 = val2.get_Files().List();
		//	val4.set_PageSize((int?)1000);
		//	((DriveBaseServiceRequest<FileList>)(object)val4).set_Fields("nextPageToken, files(id, name, md5Checksum, size, parents)");
		//	IList<File> files2 = ((ClientServiceRequest<FileList>)(object)val4).Execute().get_Files();
		//	int num = files.Count();
		//	int num2 = files2.Count();
		//	string[,] array2 = IListToArray(files2, files, fullPath: true);
		//	int num3 = 1;
		//	return null;
		//}

		//public static string checkHash(string folderPath)
		//{
		//	List<string> files = new List<string>();
		//	files = DirSearch(folderPath, files);
		//	StringBuilder stringBuilder = new StringBuilder();
		//	string path = folderPath + "\\Mods\\ModCFG.txt";
		//	string[] cfgFileString = null;
		//	if (System.IO.File.Exists(path))
		//	{
		//		cfgFileString = System.IO.File.ReadAllLines(path);
		//	}
		//	List<string> list = ParseCfgFile(cfgFileString);
		//	getGoogleDiskModInfo(list);
		//	List<List<string>> list2 = SortModsByFolderAndGetHash(files, list);
		//	return GenerateHashString(stringBuilder.ToString());
		//}
	}
}
