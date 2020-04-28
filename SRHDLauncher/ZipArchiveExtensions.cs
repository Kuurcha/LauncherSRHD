using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;

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
			form1.progressBar.Value = value;
			string text = "testMessage (ZipArchiveExtensions)";
			string customText = "testMessage2 (ZipArchiveExtension)";
			if (form2.Lang == "eng")
			{
				text = " Unpacking: ";
				customText = "              File has been unzipped";
			}
			if (form2.Lang == "ru")
			{
				text = " Распаковка: ";
				customText = "               Распаковка завершена";
			}
			form1.progressBar.CustomText = "            " + text + zipProgress.Processed + "  из " + zipProgress.Total;
			if (zipProgress.Processed == zipProgress.Total)
			{
				form1.progressBar.CustomText = customText;
			   
				
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

		public static void Unpack(string zipFilePath, string zipFileFolderPath, update form, mainform mainform)
		{
			form1 = form;
			form2 = mainform;
			_progress = new Progress<ZipProgress>();
			_progress.ProgressChanged += Report;
			form.progressBar.Value = 0;
			archivePath = zipFilePath;
			new Thread((ThreadStart)delegate
			{
				Download(zipFilePath, zipFileFolderPath);
			}).Start();
		}

		public static void Download(string url, string filePathDir)
		{
			wc = new WebClient();
			zipReadingStream = wc.OpenRead(url);
			zip = new ZipArchive(zipReadingStream);
			zip.ExtractToDirectory(filePathDir, _progress);
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
			string imagePathRu = "";
			string imagePathEng = "";

			flag = BoolConfirmation.checkIfUpdateIsRequired(form2.pathToFile, "https://drive.google.com/file/d/1jDScpEkq-mybtv4SNtL-rjyE-9wM4Uos/view?usp=sharing", ref message, form1, form2, ref updateBytes, ref sizeDiffers, ref info, ref imagePathRu, ref imagePathEng);
			form1.flagToContinue = true;
			if (!flag)
			{
				if (form2.Lang == "ru")
				{
					message = "                                         Мод установлен.";
				}
				if (form2.Lang == "eng")
				{
					message = "                                         Mod installed. ";		}
				
			}
			form1.isModInstalled = true;
			form2.isModInstalled = true;
			form2.updateRequired = flag;
			form1.updateRequired = flag;

			if (form1.reinstall)
			{
				form1.reinstall = false;
				form1.isModInstalled = true;
			}
			if (flag)
			{
				
			}
			else
			{
				form1.updateInProgress = false;
			}
		}
	}
}
