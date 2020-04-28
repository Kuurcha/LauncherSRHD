using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public static class ZipArchiveExtensions
	{
		private static bool updateCanceled = false;

		private static update form1;

		private static mainform form2;

		private static Progress<ZipProgress> _progress;

		private static string archivePath = "";

		private static WebClient wc = null;

		private static Stream zipReadingStream = null;

		private static ZipArchive zip = null;

		public static void setUpdateCancel(bool state)
		{
			updateCanceled = state;
		}
		static bool flag = false;
		private static void Report(object sender, ZipProgress zipProgress)
		{
			int value = (int)((float)zipProgress.Processed / (float)zipProgress.Total * 100f);
			form1.progressBar.Invoke((MethodInvoker)delegate {
				// Running on the UI thread

				form1.progressBar.Value = value;
			});

		
			
			string text = StringProcessing.getMessage(form2.Lang, " Распаковка: ", " Unpacking: "	);
			string	customText = StringProcessing.getMessage(form2.Lang, "               Распаковка завершена", "              File has been unzipped");
			
			form1.progressBar.Invoke((MethodInvoker)delegate {
				// Running on the UI thread

				form1.progressBar.CustomText = "            " + text + zipProgress.Processed + "  из " + zipProgress.Total;
			});


			if (zipProgress.Processed == zipProgress.Total)
			{
				form1.progressBar.Invoke((MethodInvoker)delegate {
					// Running on the UI thread

					form1.progressBar.CustomText = customText;
				});
				_progress = null;
			}
			if (updateCanceled)
			{
				disposeHere();
				updateCanceled = false;
			}
		}

		public static void disposeHere()
		{
			if (wc != null)
			{
				wc.Dispose();
				zipReadingStream.Dispose();
				zip.Dispose();
				wc = null;
				zipReadingStream = null;
				zip = null;
			}
		}

		static bool patch = false;
		/// <summary>
		/// Распаковывает архив из указанного пути в указанный путь
		/// </summary>
		/// <param name="zipFilePath">Путь источника, где находится архив</param>
		/// <param name="zipFileFolderPath">Путь назначения распаковки, куда распаковывать</param>
		/// <param name="form">Форма update, для изменения всевозможных параметров в ней, обработки прерываний и прочего</</param>
		/// <param name="mainform">Форма mainform, для изменения параметров в ней, обработки прерываний и прочего</param>
		public static void Unpack(string zipFilePath, string zipFileFolderPath, update form, mainform mainform, bool patch1)
		{
			//Установка переданных переменных
			patch = patch1;
			form1 = form;
			form2 = mainform;
			archivePath = zipFilePath;
			//Создание переменной для тикания прогресса и подпись на событие
			_progress = new Progress<ZipProgress>();
			_progress.ProgressChanged += Report;
			//Сброс прогресс бара
			form.progressBar.Invoke((MethodInvoker)delegate {
				// Running on the UI thread

				form.progressBar.Value = 0;
			});
			
			
			//Лямбда-выражение для запуска потока с распаковкой
			new Thread((ThreadStart)delegate
			{
				Download(zipFilePath, zipFileFolderPath);
			}).Start();
			

		}

		/// <summary>
		/// Метод для самого процесса скачки
		/// </summary>
		/// <param name="zipFilePath"></param>
		/// <param name="zipFileFolderPath"></param>
		public static void Download(string zipFilePath, string zipFileFolderPath)
		{
			wc = new WebClient();
			zipReadingStream = wc.OpenRead(zipFilePath);
			zip = new ZipArchive(zipReadingStream);
			zip.ExtractToDirectory(zipFileFolderPath, _progress);
			flag = BoolConfirmation.checkIfUpdateIsRequired(form2.pathToFile, "https://drive.google.com/file/d/1jDScpEkq-mybtv4SNtL-rjyE-9wM4Uos/view?usp=sharing", form1, form2);		
			form1.flagToContinue = flag;
			form1.RepeatUntilBusy(wc);
			disposeHere();
			File.Delete(archivePath);
			form1.updateInProgress = false;
			form1.archiveBegun = false;
			string message = "";
			long updateBytes = 1L;
			bool sizeDiffers = false;
			string[] info = null;
			Thread.Sleep(1500);
			flag = BoolConfirmation.checkIfUpdateIsRequired(form2.pathToFile, "https://drive.google.com/file/d/1jDScpEkq-mybtv4SNtL-rjyE-9wM4Uos/view?usp=sharing", ref message, form1, form2, ref updateBytes, ref sizeDiffers, ref info);
			form1.flagToContinue = true;
			form1.isModInstalled = true;
			form2.isModInstalled = true;
			form2.updateRequired = flag;
			form1.updateRequired = flag;
			if (form1.reinstall)
			{
				form1.reinstall = false;
				form1.isModInstalled = true;
			}
			if (!flag || patch) form1.updateInProgress = false;

			else { form1.downloadAllPatches(true); } //костыль по смене галочки для скачки фигни
													 //Установка технически переменных, которые были не убраны из метода по обновлению короч этого самого вашего лончера.



		}
	}
}
