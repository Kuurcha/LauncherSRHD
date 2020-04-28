using SRHDLauncher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public class update : Form
	{
		public bool abortEtoGreh = false;

		private static System.Timers.Timer aTimer;

		private mainform form;

		public bool downloadSR1HDMode = false;

		public bool updateRequired = false;

		public bool isModInstalled = false;

		public string[] info;

		private string executePath;

		private const uint WM_SYSCOMMAND = 274u;

		private const uint DOMOVE = 61458u;

		private const uint DOSIZE = 61448u;

		public bool reinstall = false;

		public CustomProgressBar progressBar;

		private static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

		public static Font updaterFont;

		public bool updateInProgress;

		public bool archiveBegun = false;

		private IContainer components = null;

		private PictureBox degenerateChoice;

		private PictureBox minimizeWindow;

		private PictureBox closeWindow;

		public ProgressBar temp;

		public string SRHD1Url
		{
			get;
			set;
		}

		public bool answer
		{
			get;
			set;
		}

		private static void SetTimer()
		{
			aTimer = new System.Timers.Timer(2000.0);
			aTimer.Elapsed += OnTimedEvent;
		}

		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("user32", CharSet = CharSet.Auto)]
		internal static extern bool PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);

		[DllImport("user32", CharSet = CharSet.Auto)]
		internal static extern bool ReleaseCapture();

		[DllImport("User32.dll")]
		public static extern int SetForegroundWindow(int hWnd);

		public FileInfo DownloadFiles(string extractPath, string tempPath, string googleDownloadPath, bool callProgressBar, long totalBytes, string msg)
		{
			string[] files = Directory.GetFiles(extractPath);
			FileInfo result = FileDownloader.DownloadFileFromURLToPath(googleDownloadPath, tempPath, callProgressBar, this, form, totalBytes, msg);
			Thread.Sleep(50);
			return result;
		}

		public void setRu()
		{
		}

		public void setEng()
		{
		}

		
		public void InitialiseFont()
		{
			string text = Path.GetTempPath() + "updaterFont.otf";
			if (!File.Exists(text))
			{
				form.CopyResource("SRHDLauncher.Resources.updaterFont.otf", text);
			}
			Fonts.AddFontFile(text);
		}

		public update(string executePath, bool updateRequired, long totalBytes, mainform form, bool isModInstalled, string[] info, string message, bool reinstall, bool downloadSR1HDMode)
		{
			bool sizeDiffers = false;
			string[] array = null;
			string imagePathRu = "";
			string imagePathEng = "";
			this.reinstall = reinstall;
		
			this.downloadSR1HDMode = downloadSR1HDMode;
			this.info = info;
			this.updateRequired = updateRequired;
			this.form = form;
			this.executePath = executePath;
			this.isModInstalled = isModInstalled;
			progressBar = new CustomProgressBar();
			progressBar.Size = new Size(775, 19);
			progressBar.Location = new Point(121, 544);
			progressBar.DisplayStyle = ProgressBarDisplayText.CustomText;
			base.Controls.Add(progressBar);
			InitializeComponent();

			degenerateChoice.Enabled = false;
			degenerateChoice.Image = SRHDLauncher.Properties.Resources._2OkD;
			if (!downloadSR1HDMode)
			{
				updateRequired = (reinstall || BoolConfirmation.checkIfUpdateIsRequired(executePath, "https://drive.google.com/file/d/1jDScpEkq-mybtv4SNtL-rjyE-9wM4Uos/view?usp=sharing", ref message, this, form, ref totalBytes, ref sizeDiffers, ref array, ref imagePathRu, ref imagePathEng));
			}
			form.changeEnabledStatusButtons();
			base.MouseDown += settings_MouseDown;
			base.MouseDown += pictureBox1_MouseDown;
			base.MouseDown += pictureBox1_MouseDown;
			base.Closing += OnClosing;
			if (form.Lang == "ru")
			{
				setRu();
				progressBar.CustomText = "Инициализация...";
			}
			if (form.Lang == "eng")
			{
				setEng();
				progressBar.CustomText = "Initialising...";
			}
			Show();
			form.checkUpdates.Enabled = false;
		    form.checkUpdates.Image = SRHDLauncher.Properties.Resources._2SettingsD;
			updateAppUpperLevel();
			
		}
		public bool flagToContinue = false;
		public void RepeatUntilBusy(WebClient client)
		{
			if (client == null)
			{
				return;
			}
			while (client.IsBusy)
			{
				if (abortEtoGreh)
				{
					client.CancelAsync();
				}
				Application.DoEvents();
			}
			
			if(flagToContinue)
			{
				Thread.Sleep(50);
				downloadAllPatches();
				flagToContinue = false;
			}
		
		}

		private void settings_Load(object sender, EventArgs e)
		{
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			PostMessage(base.Handle, 274u, 61458u, 0u);
		}

		private void settings_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			PostMessage(base.Handle, 274u, 61458u, 0u);
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
		{
			Close();
		}

		public static void OnTimedEvent(object source, ElapsedEventArgs e)
		{
		}

		private void degenerateChoice_MouseDown(object sender, MouseEventArgs e)
		{
			degenerateChoice.Image = Resources._2OkD;
		}

		private void goodChoice_MouseUp(object sender, MouseEventArgs e)
		{
		}
		public void updateAppUpperLevel()
		{
			if (downloadSR1HDMode)
			{
				callDownloadSR1HD();
				return;
			}
			string message = "";
			long updateBytes = 1L;
			bool sizeDiffers = false;
			string[] array = null;
			string imagePathRu = "";
			string imagePathEng = "";
			updateRequired = (reinstall || BoolConfirmation.checkIfUpdateIsRequired(form.pathToFile, "https://drive.google.com/file/d/1jDScpEkq-mybtv4SNtL-rjyE-9wM4Uos/view?usp=sharing", ref message, this, form, ref updateBytes, ref sizeDiffers, ref array, ref imagePathRu, ref imagePathEng));
			if (updateRequired)
			{
				string path = StringProcessing.StepUp(form.pathToFile) + "\\Mods\\version.txt";
				string path2 = StringProcessing.StepUp(form.pathToFile) + "\\Mods\\ModCFG.txt";
				if (File.Exists(path) && File.Exists(path2))
				{
					isModInstalled = true;
					form.isModInstalled = true;
				}
				else
				{
					isModInstalled = false;
					form.isModInstalled = false;
				}
				if (reinstall)
				{
					try
					{
						Directory.Delete(executePath, recursive: true);
					}
					catch (Exception)
					{
					}
				}
				updateApp();
			}
			
		}
		private void degenerateChoice_MouseUp(object sender, MouseEventArgs e)
		{
			degenerateChoice.Image = Resources._2OkA;
			Close();
			form.updateForm = null;
		}

		private void goodChoice_Click(object sender, EventArgs e)
		{
		}

		private void label1_Click(object sender, EventArgs e)
		{
		}

		private void label1_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			PostMessage(base.Handle, 274u, 61458u, 0u);
		}

		public List<string> ParseCfgFile(string[] cfgFileString)
		{
		
		    List<string> list = new List<string>();
			foreach (string text in cfgFileString)
			{
				string text2 = text;
				text2 = text.Replace("CurrentMod=", "");
				int num = 0;
				int num2 = 0;
				if (text2.Length>4)
				{
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

		private void degenerateChoice_Click(object sender, EventArgs e)
		{
			FormsFunctions.PlayButtonSound();
		}

		private void closeWindow_MouseDown(object sender, MouseEventArgs e)
		{
			closeWindow.Image = Resources._2CrossD;
		}

		private void closeWindow_MouseUp(object sender, MouseEventArgs e)
		{
			closeWindow.Image = Resources._2CrossA;
			Close();
			form.updateForm = null;
		}

		private void minimizeWindow_MouseUp(object sender, MouseEventArgs e)
		{
			minimizeWindow.Image = Resources._2SubA;
			base.WindowState = FormWindowState.Minimized;
		}

		private void minimizeWindow_MouseDown(object sender, MouseEventArgs e)
		{
		}

		public void updateApp()
		{
			if (updateRequired)
			{
				string text = StringProcessing.StepUp(form.pathToFile) + "\\Mods\\version.txt";
				updateInProgress = true;
				degenerateChoice.Enabled = false;
				degenerateChoice.Image = Resources._2OkD;
				if (isModInstalled && !reinstall)
				{
					downloadAllPatches();
				}
				else
				{
					callWholeUpdate();

				}
			}
		}

		private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			if (updateInProgress || archiveBegun)
			{
				string text = "видимо автор малок если это сообщение появилось. Проблема в update.OnClosing, с языком";
				if (form.Lang == "ru")
				{
					text = "Вы уверены, что хотите закрыть это окно? Обновление в процессе";
				}
				if (form.Lang == "eng")
				{
					text = "Update is currently in progress. Are you sure that you want to close the window? ";
				}
				switch (MessageBox.Show(text, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
				{
				case DialogResult.Yes:
					form.changeEnabledStatusButtons();
					abortEtoGreh = true;
					form.updateForm = null;
					ZipArchiveExtensions.setUpdateCancel(state: true);
					break;
				case DialogResult.No:
					if (!downloadSR1HDMode)
					{
						form.updateRequired = true;
					}
					cancelEventArgs.Cancel = true;
					break;
				}
			}
			else if (form != null)
			{
				form.changeEnabledStatusButtons();
				if (!updateRequired)
				{
					form.play.Enabled = true;
					form.play.Image = Resources._2ContinueA;
				}
				form.updateForm = null;
			}
		}

		public void callDownloadSR1HD()
		{
			long totalBytes = 1L;
			string msg = "sosi. CallWholeUpdate";
			if (form.Lang == "ru")
			{
				totalBytes = 179385718L;
				msg = "Скачивание файлов: ";
			}
			if (form.Lang == "eng")
			{
				totalBytes = 171312408L;
				msg = "Downloading files: ";
			}
			string text = executePath + "\\Temp.zip";
			FileInfo fileInfo = DownloadFiles(executePath, text, SRHD1Url, callProgressBar: true, totalBytes, msg);
			Thread.Sleep(1200);
			temp.Value = 0;
			if (!abortEtoGreh)
			{
				archiveBegun = true;
				ZipArchiveExtensions.Unpack(text, executePath, this, form);
			}
			else
			{
				File.Delete(text);
			}
			downloadSR1HDMode = false;
		}

		public void callWholeUpdate()
		{
			string text = StringProcessing.StepUp(form.pathToFile);
			string text2 = text + "\\Mods";
			string path = text2 + "\\ModCFG.txt";
			string path2 = text2 + "\\tempCfg.txt";
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			
			if (File.Exists(path))
			{
				FileDownloader.DownloadFileFromURLToPath(AppInfo.APP_CFG_LINK_FILE, path2, callProgressBar: false, this, null, 1L, "");
				List<string> list = null;
				list = ParseCfgFile(File.ReadAllLines(path));
				List<string> source = ParseCfgFile(File.ReadAllLines(path2));
				foreach (string item in list)
				{
					if (!source.Any(item.Contains))
					{
						string path3 = text2 + "\\" + item;
						if (Directory.Exists(path3))
						{
							Directory.Delete(text2 + "\\" + item, recursive: true);
						}
					}
				}
				File.Delete(path2);
			}
		
			string[] files = Directory.GetFiles(text2);
			if (files.Length > 1)
			{
				string[] array = files;
				foreach (string text3 in array)
				{
					try
					{
						if (!text3.Contains("ModCFG.txt"))
						{
							File.Delete(text3);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			string text4 = text + "\\Temp.zip";
			string msg = "sosi. CallWholeUpdate";
			if (form.Lang == "ru")
			{
				msg = "Скачивание файлов: ";
			}
			if (form.Lang == "eng")
			{
				msg = "Downloading files: ";
			}
			FileInfo fileInfo = DownloadFiles(text, text + "\\Temp.zip", AppInfo.APP_ZIP_FILE_LINK, callProgressBar: true, form.totalBytes, msg);
			Thread.Sleep(600);
			temp.Value = 0;
			if (!abortEtoGreh)
			{
				archiveBegun = true;
				ZipArchiveExtensions.Unpack(text4, text, this, form);
				Thread.Sleep(50);
			}
			else
			{
				File.Delete(text4);
			}
		}

		public void downloadAllPatches()
		{
			double num = 7.0;
			List<string> list = new List<string>();
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SRHDLauncher.launсherHashСopy.txt"))
			{
				TextReader textReader = new StreamReader(stream);
				string item;
				while ((item = textReader.ReadLine()) != null)
				{
					list.Add(item);
				}
			}
			string[] array = list.ToArray();
			for (int i = 1; i < info.Length - 1; i += 3)
			{
				if (list != null)
				{
					float result = 0f;
					long result2 = 1L;
					bool flag = float.TryParse(info[i], out result);
					bool flag2 = long.TryParse(info[i + 2], out result2);
					if (flag && flag2 && mainform.lastDetected < (double)result)
					{
						string message = "";
						if (form.Lang == "ru")
						{
							message = "     Downloading update v" + num;
						}
						if (form.Lang == "eng")
						{
							message = "    Скачивается обновление v" + num;
						}
						string text = StringProcessing.StepUp(executePath) + "\\Mods\\" + info[i] + ".zip";
						FileDownloader.DownloadFileFromURLToPath(info[i + 1], text, callProgressBar: true, this, form, result2, message);
						mainform.lastDetected = result;
						ZipArchiveExtensions.Unpack(text, StringProcessing.StepUp(executePath) + "\\Mods", this, form);
						string path = StringProcessing.StepUp(form.pathToFile) + "\\Mods\\version.txt";
						num = double.Parse(info[i]);
						if (File.Exists(path))
						{
							File.Delete(path);
							using (StreamWriter streamWriter = File.CreateText(path))
							{
								streamWriter.WriteLine(info[i]);
							}
						}
					}
				}
			
			}
			string text2 = "downloadAllPatches";
			if (form.Lang == "ru")
			{
				text2 = "Обновлено до версии " + num;
			}
			if (form.Lang == "eng")
			{
				text2 = "Updated up to " + num;
			}
			FileDownloader.DownloadFileFromURLToPath("https://drive.google.com/file/d/1xkFP-LnD6pa2SWfY55b1y3vSFpeUf2mm/view?usp=sharing", StringProcessing.StepUp(form.pathToFile) + "\\changeLogRu.txt", callProgressBar: false, this, form, "");
			FileDownloader.DownloadFileFromURLToPath("https://drive.google.com/file/d/1Z91zQLaUgWMbudO4m8ucQ0DFzeRA2fXW/view?usp=sharing", StringProcessing.StepUp(form.pathToFile) + "\\changeLogEng.txt", callProgressBar: false, this, form, "");
			if (form.Lang == "ru")
			{
				Process.Start(StringProcessing.StepUp(form.pathToFile) + "\\changeLogRu.txt");
			}
			if (form.Lang == "eng")
			{
				Process.Start(StringProcessing.StepUp(form.pathToFile) + "\\changeLogEng.txt");
			}
	
		}

		private void update_Shown(object sender, EventArgs e)
		{
		}

		private void progressBar_Click(object sender, EventArgs e)
		{
		}

		private void messageLabel_Move(object sender, EventArgs e)
		{
		}

		private void downloadProgress_Click(object sender, EventArgs e)
		{
		}

		private void update_Leave(object sender, EventArgs e)
		{
		}

		private void msgAboutUpd_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			Close();
			form.updateForm = null;
		}

		private void pictureBox1_MouseDown_2(object sender, MouseEventArgs e)
		{

		}

		private void pictureBox1_Click_1(object sender, EventArgs e)
		{
		}

		private void closeWindow_Click(object sender, EventArgs e)
		{
		}

		private void update_MouseUp(object sender, MouseEventArgs e)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(update));
            this.temp = new System.Windows.Forms.ProgressBar();
            this.closeWindow = new System.Windows.Forms.PictureBox();
            this.minimizeWindow = new System.Windows.Forms.PictureBox();
            this.degenerateChoice = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.closeWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.degenerateChoice)).BeginInit();
            this.SuspendLayout();
            // 
            // temp
            // 
            this.temp.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.temp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.temp.Location = new System.Drawing.Point(121, 544);
            this.temp.Name = "temp";
            this.temp.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.temp.Size = new System.Drawing.Size(775, 19);
            this.temp.TabIndex = 17;
            this.temp.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // closeWindow
            // 
            this.closeWindow.BackColor = System.Drawing.Color.Transparent;
            this.closeWindow.Image = global::SRHDLauncher.Properties.Resources._2CrossA;
            this.closeWindow.Location = new System.Drawing.Point(986, 5);
            this.closeWindow.Name = "closeWindow";
            this.closeWindow.Size = new System.Drawing.Size(24, 24);
            this.closeWindow.TabIndex = 16;
            this.closeWindow.TabStop = false;
            this.closeWindow.Click += new System.EventHandler(this.closeWindow_Click);
            this.closeWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.closeWindow_MouseDown);
            this.closeWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.closeWindow_MouseUp);
            // 
            // minimizeWindow
            // 
            this.minimizeWindow.BackColor = System.Drawing.Color.Transparent;
            this.minimizeWindow.Image = global::SRHDLauncher.Properties.Resources._2SubA;
            this.minimizeWindow.Location = new System.Drawing.Point(955, 5);
            this.minimizeWindow.Name = "minimizeWindow";
            this.minimizeWindow.Size = new System.Drawing.Size(25, 24);
            this.minimizeWindow.TabIndex = 15;
            this.minimizeWindow.TabStop = false;
            this.minimizeWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.minimizeWindow_MouseDown);
            this.minimizeWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.minimizeWindow_MouseUp);
            // 
            // degenerateChoice
            // 
            this.degenerateChoice.BackColor = System.Drawing.Color.Transparent;
            this.degenerateChoice.Image = global::SRHDLauncher.Properties.Resources._2OkA;
            this.degenerateChoice.Location = new System.Drawing.Point(476, 569);
            this.degenerateChoice.Name = "degenerateChoice";
            this.degenerateChoice.Size = new System.Drawing.Size(52, 22);
            this.degenerateChoice.TabIndex = 10;
            this.degenerateChoice.TabStop = false;
            this.degenerateChoice.Click += new System.EventHandler(this.degenerateChoice_Click);
            this.degenerateChoice.MouseDown += new System.Windows.Forms.MouseEventHandler(this.degenerateChoice_MouseDown);
            this.degenerateChoice.MouseUp += new System.Windows.Forms.MouseEventHandler(this.degenerateChoice_MouseUp);
            // 
            // update
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1018, 677);
            this.Controls.Add(this.temp);
            this.Controls.Add(this.closeWindow);
            this.Controls.Add(this.minimizeWindow);
            this.Controls.Add(this.degenerateChoice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "update";
            this.Text = "settings";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Load += new System.EventHandler(this.settings_Load);
            this.Shown += new System.EventHandler(this.update_Shown);
            this.Leave += new System.EventHandler(this.update_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.settings_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.update_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.closeWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.degenerateChoice)).EndInit();
            this.ResumeLayout(false);

		}
	}
}
