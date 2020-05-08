using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public static class BoolConfirmation
	{

		public static bool SetPath(ref string downloadPath)
		{
			bool flag = false;
			string appName = StringProcessing.getAppName(downloadPath);
			if (appName == "Rangers.exe")
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
			if (File.Exists(downloadPath) && StringProcessing.getAppName(downloadPath) == "Rangers.exe")
			{
				return true;
			}
			return false;
		}

		public static bool OpenDialog(ref string s)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.ShowDialog();
			s = openFileDialog.FileName;
			bool result = SetPath(ref s);
			string path = s;
			string[] additionalDataFiles = AppInfo.additionalDataFiles;
			foreach (string str in additionalDataFiles)
			{
				string path2 = StringProcessing.StepUp(path) + "\\Data\\" + str;
				if (!File.Exists(path2))
				{
					result = false;
				}
			}
			return result;
		}

		public static bool OpenDialogSR1HD(ref string s)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.ShowDialog();
			s = openFileDialog.FileName;
			return SetPath(ref s);
		}

		public static long DirSize(DirectoryInfo d)
		{
			long num = 0L;
			FileInfo[] files = d.GetFiles();
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				num += fileInfo.Length;
			}
			DirectoryInfo[] directories = d.GetDirectories();
			DirectoryInfo[] array2 = directories;
			foreach (DirectoryInfo d2 in array2)
			{
				num += DirSize(d2);
			}
			return num;
		}

		public static Stream GetEmbeddedResourceStream(string resourceName)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
		}

		public static bool checkIfUpdateIsRequired(string path, string URI, update updateForm, mainform mainMenuForm)
		{
			bool result = false;
			bool flag = false;
			path = StringProcessing.StepUp(path);
			string path2 = path + "\\Mods\\version.txt";
			string path3 = path + "\\tempIni.ini";
			string[] info;
			try
			{
				List<string> list = new List<string>();
				Assembly.GetExecutingAssembly().GetManifestResourceNames();
				string[] array;
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SRHDLauncher.Resources.launсherHash.txt"))
				{
					int num = 0;
					bool flag2 = false;
					TextReader textReader = new StreamReader(stream);
					string text2;
					while ((text2 = textReader.ReadLine()) != null)
					{
						num++;
						if (text2 == "<StartVersions>")
						{
							flag2 = true;
						}
						if (text2 == "<EndVersions>")
						{
							flag2 = false;
						}
						if (flag2)
						{
							list.Add(text2);
						}
					}
					array = (info = list.ToArray());
				}
				double result2 = 0.0;
				bool modCfgExistsFlag = File.Exists(path + "\\Mods\\ModCFG.txt");
				bool versionExistsFlag = File.Exists(path2);
				if (modCfgExistsFlag && versionExistsFlag)
				{
					string[] array2 = File.ReadAllLines(path2);
					if (array2.Length == 0 || !double.TryParse(array2[0], out result2))
					{
						return true;
					}
					for (int i = 1; i < array.Length - 1; i += 3)
					{
						double num2 = double.Parse(array[i]);
						flag = !(result2 >= num2);
						if (flag)
						{
							break;
						}
					}
					if (flag && File.Exists(path2))
					{

					}
					result = (flag);
				}
				else { result = true; }
				File.Delete(path3);
			}
			catch (Exception)
			{
			}
			return result;
		}

		/// <summary>
		/// Метод, для проверки необходимо ли лаунчеру обновлять предложение
		/// </summary>
		/// <param name="path">Путь к файлу</param>
		/// <param name="URI">Путь к обновлению на гугл диске</param>
		/// <param name="message">Конкретное сообщение для мессейдж бокса об обновлении</param>
		/// <param name="updateForm">Форма с обновлением, если та была вызвана из нее.</param>
		/// <param name="mainMenuForm">Форма главного меню, для смены глобальный параметров</param>
		/// <param name="updateBytes">Суммарное количество байтов что займет скачиваемый файл</param>
		/// <param name="sizeDiffers"></param>
		/// <param name="info"></param>
		/// <returns>Необходимо ли обновлять приложение</returns>
		public static bool checkIfUpdateIsRequired(string path, string URI, ref string message, update updateForm, mainform mainMenuForm, ref long updateBytes, ref bool sizeDiffers, ref string[] info)
		{
			bool result = false;
			bool flag = false;
			path = StringProcessing.StepUp(path);
			string pathToVersionTxt = path + "\\Mods\\version.txt";

			string text = message;
			try
			{
				List<string> list = new List<string>(); //Список для хранения всех обновлений по версиям
				string[] updatesArray; //Массив, в который будет  в дальнейшем превращен список выше
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SRHDLauncher.Resources.launсherHash.txt"))
				{
					int i = 0; //Переменная для итерации
					bool flag2 = false; //Переменная для проверки, начался ли при парсинге участок с версиями 
					TextReader textReader = new StreamReader(stream); //Объект StreamReader для чтения текстового файла
					string text2; // Переменная которая будет отвечать за считывающуюся строку в цикле ниже. Почему-то нельзя объявлять такие вещи в while
					while ((text2 = textReader.ReadLine()) != null)
					{
						i++;
						if (text2 == "<StartVersions>")
						{
							flag2 = true;
						}
						if (text2 == "<EndVersions>")
						{
							flag2 = false;
						}
						if (flag2)
						{
							list.Add(text2);
						}
					}
					updatesArray = (info = list.ToArray());
				}
				double result2 = 0.0;
				bool modCfgExistsFlag = File.Exists(path + "\\Mods\\ModCFG.txt"); //Условие существования modCfg.txt файла
				bool versionExistsFlag = File.Exists(pathToVersionTxt); //Условие существование version.txt файла
				if (modCfgExistsFlag && versionExistsFlag) //Если и файл версий, и модкфг существует, в таком случае считается что мод скачен, идет преобразование массива с текстом о версиях в ссылки и их соотвествующие версии, размеры.
				{
					string[] array2 = File.ReadAllLines(pathToVersionTxt);
					//В случае если файл версий нельзя пропарсить, в таком случае невозможно проверить как много надо добавить патчей поверх, выдаем соотвествующую ошибку и качаем игру с нуля.
					if (array2.Length == 0 || !double.TryParse(array2[0], out result2))
					{
						message = StringProcessing.getMessage(mainMenuForm.Lang, "Файл версий в неверном формате. Невозможно проверить наличие обновлений. Нажмите √ для продолжения", "Version file is incorrect. It's impossible to check, if updates are necssary. Press √ to proceed");
						return true;
					}
					//Цикл парсящий каждую третью строку, проверяющий, есть ли хоть одно обновление, выше версией нежели в versions.txt
					for (int i = 1; i < updatesArray.Length - 1; i += 3)
					{
						double num2 = double.Parse(updatesArray[i]);
						flag = !(result2 >= num2);
						if (flag) break;
					}
					//Если все выполняется, выводим сообщения об этом, и изменяет результат на то, что обновления таки нужны.
					if (flag && File.Exists(pathToVersionTxt)) message = StringProcessing.getMessage(mainMenuForm.Lang, "Доступна новая версия мода. Нажмите √ для продолжения", "The mod update is avaliable. Press √ to proceed");
					result = (flag);
				}
				else
				{
					//Обновления нужны, но, идет проверка. Первый раз ли ставится Мод (нету ни версий, ни мод кфг), либо проблема какая-либо с файлом версий.
					if ((modCfgExistsFlag && !versionExistsFlag)) message = StringProcessing.getMessage(mainMenuForm.Lang, "Отсуствует файл версий. Нажмите √ для продолжения", "Version file is absent. Press √ to proceed");
					if ((!modCfgExistsFlag && !versionExistsFlag) || (!modCfgExistsFlag && versionExistsFlag)) message = StringProcessing.getMessage(mainMenuForm.Lang, "Мод еще не установлен.Нажмите √ для продолжения", "Version file or ModCfg is absent.Press √ to proceed");
					result = true;
				}
			}
			catch (Exception) { }
			return result;
		}

		public static bool checkIfUpdateIsRequired(string path, string URI, string version, update updateForm, mainform mainMenuForm, ref long totalBytes)
		{
			bool result = false;
			string path2 = path + "\\tempIni.ini";
			try
			{
				FileInfo fileInfo = FileDownloader.DownloadFileFromURLToPath(URI, path2, callProgressBar: false, updateForm, mainMenuForm, "");
				if (fileInfo != null)
				{
					string[] array = File.ReadAllLines(path2);
					totalBytes = long.Parse(array[array.Length - 3]);
					string a = array[array.Length - 1];
					result = !(a == version);
					File.Delete(path2);
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		public static bool checkIfUpdateIsRequired(string path, string URI, string version, update updateForm, mainform mainMenuForm, ref long total, ref string[] text)
		{
			bool result = false;
			string path2 = path + "\\tempIni.ini";
			try
			{
				string[] array = File.ReadAllLines(path2);
				FileInfo fileInfo = FileDownloader.DownloadFileFromURLToPath(URI, path2, callProgressBar: false, updateForm, mainMenuForm, "");
				string a = null;
				if (fileInfo != null)
				{
					try
					{
						total = long.Parse(array[array.Length - 2]);
						a = array[array.Length - 1];
					}
					catch (Exception ex)
					{
						MessageBox.Show("Проблема со стороны загрузки файла настроек на сервер: " + ex.Message);
					}
					result = !(a == version);
					File.Delete(path2);
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
	}
}
