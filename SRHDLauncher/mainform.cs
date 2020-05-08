using Microsoft.Win32;
using SHDocVw;
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
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public class mainform : Form
	{
		private string tempFilePath;

		private const int THOUGHT_OF_THE_DAY = 1;

		private const bool FIRST_RUN = true;

		private const string PATH_TO_FILE = "SOSI PUTYA NE BUDET PERVI ZAPUSK";

		private const string LANG = "none";

		private const string STATE = "ХРОНИЧЕСКАЯ ДЕПРЕССИЯ";

		private const bool TURN_OFF_LAUNCHER = false;

		private const bool REPLACECFG = true;

		private const double LAST_DETECTED = 7.0;

		private string[] thoughts;

		private string[] thoughtsRu = new string[12]
		{
			"\"Это все веселье и игры, пока по кому-нибудь не зарядят ядеркой\" - Рейнджер Клот",
			"\"Ничего себе гаалец!\" - Рейнджер Греф",
			"\"Смел тот кто летает с закрытым слотом дроида\" - Дальнобойщик Гусаров",
			"\"Если не вышло с первого раза, сбрось ядерную бомбу\" - Истребитель Хантер -14-",
			"\"Хорошей охоты\" - Истребитель Адама -20-",
			"\"У меня тоже есть для тебя сюрприз...\" - Лякуша Йцукен",
			"\"Хороший солдат подчиняется без вопросов. Хороший офицер командует без сомнения. Особенно когда \r\n ему приказано атаковать Келлера\" - Раванжер Огандрок -5-",
			"\"Есть ли для меня специальное задание?\" - Рейнджер Греф",
			"\"Мистер Саальери передаёт вам поклон\" - Ренегал Виито",
			"\"Где детонатор?\" - Рейнджер Брюс",
			"\"Даже человек, у которого ничего нет, все равно может пойти копать минералы.\" - Консул Веста",
			"\"Запомни меня!\" - Корсар Накс"
		};

		private string[] thoughtsEng = new string[12]
		{
			"\"It's all fun and games until somebody gets nuked\" - Ranger Klot",
			"\"Now that's a gaalian\" - Ranger Gref",
			"\"Brave are they who fly with a closed droid slot\" - Hauler Gusarov",
			"\"If at first you don't succeed, nuke them\" - Fighter Hunter -14-",
			"\"Good hunting\" - Fighter Adama -20-",
			"\"I too have a surprise for you...\" - Lyakusha Jtzukhen",
			"\"A good soldier obeys without question. A good officer commands without doubt. Especially when \r\n ordered to charge at Keller\"- Ravanger Ogandrock -5-",
			"\"Have a secret mission for me?\" - Ranger Gref",
			"\"Mister Saalieri sends his regards\" - Renegal Viito",
			"\"Where's the detonator?\" - Ranger Bruce",
			"\"Even a man who has nothing can still go mine asteroids.\" - Consul Vesta",
			"\"Witness me!\" - Corsair Nux"
		};

		private string pathToGuide = "https://steamcommunity.com/sharedfiles/filedetails/?id=1993610246";

		public bool isModInstalled = false;

		string imagePathRu;

		string imagePathEng;

		public bool oneWindowIsAlreadyLaunched = false;

		private string textOfTheDay;

		private bool launchUpdated = true;

		private string message = "";

		public bool isPathCorrect = false;

		public string executabePath = "";

		bool firstTimeMode = true;

		public bool updateRequired = false;

		public bool updateInProgress = false;

		public bool abortEtoGreh = false;

		public bool archiveBegun = false;

		public long totalBytes = 1L;

		private static string[] ruButtons =
		{
			"Играть",
			"Обновления",
			"Дискорд сервер ЦР Полюс Мира",
			"Настройки",
			"Руководство",
			"Выход",
			"Веб-сайт",
			"Обновить лаунчер",
			"Скачать HD-мод на Космические Рейнджеры 1"
		};

		private static string[] engButtons =
		{
			"Play",
			"Update",
			"Discord Server Celestial Pole",
			"Settings",
			"Guide",
			"Exit",
			"Web-site",
			"Update Launcher",
			"Download HD mod for the Space Rangers 1"
		};

		private List<Point> points = new List<Point>();

		private const int cGrip = 16;

		private const int cCaption = 32;

		private const uint WM_SYSCOMMAND = 274u;

		private const uint DOMOVE = 61458u;

		private const uint DOSIZE = 61448u;

		private static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

		public static Font calcFont;

		private string appPath = Application.StartupPath;

		private string executePath = Application.ExecutablePath;

		private myMessageBox myMessageBox;

		private string[] info = null;

		private bool launcherIsOutdated = true;

		private string linkToWebSite;

		private string linkToGallery;

		private IContainer components = null;

		private PictureBox exit;

		private PictureBox Settings;

		private PictureBox discordServer;

		private PictureBox guide;

		private Button button1;

		private Label versionLbl;

		private Label label2;

		private Label label3;

		private Label label4;

		private Label label5;

		private Label label6;

		private Label label7;

		private Label thoughtOfTheDayLbl;

		private Label authors;

		private ToolTip toolTip1;

		private PictureBox pictureBox3;

		private PictureBox pictureBox4;

		public PictureBox checkUpdates;

		public PictureBox play;

		private Label label1;

		private PictureBox pictureBox6;

		private Button button2;

		public WebBrowser webBrowser1;

		private PictureBox downloadSR1HD;
		private PictureBox updateLauncherPB;
		private Label updateLauncherTB;
		private PictureBox pictureBox2;

		public static int thoughtOfTheDay
		{
			get;
			set;
		}

		public static double lastDetected
		{
			get;
			set;
		}

		public bool firstRun
		{
			get;
			set;
		}

		public string pathToFile
		{
			get;
			set;
		}

		public string Lang
		{
			get;
			set;
		}

		public bool replaceCfg
		{
			get;
			set;
		}

		public static DateTime todayIs
		{
			get;
			set;
		}

		public static string state
		{
			get;
			set;
		}

		public static bool turnOffLauncher
		{
			get;
			set;
		}

		public bool answer
		{
			get;
			set;
		}

		public update updateForm
		{
			get;
			set;
		}

		public settings settingsForm
		{
			get;
			set;
		}

		public play playForm
		{
			get;
			set;
		}

		public bool internetIsAbsent
		{
			get;
			set;
		}

		public bool checkVersionContent(string versionPath)
		{
			bool flag = File.Exists(versionPath);
			bool flag2 = false;
			if (flag)
			{
				string[] array = File.ReadAllLines(versionPath);
				flag2 = (array.Length != 0 && double.TryParse(array[0], out double _));
			}
			return flag && flag2;
		}

		public void setVariables(string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			using (FileStream fileStream = File.Create(fileName))
			{
				byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(thoughtOfTheDay + "\r");
				fileStream.Write(bytes, 0, bytes.Length);
				byte[] bytes2 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(pathToFile + "\r");
				fileStream.Write(bytes2, 0, bytes2.Length);
				byte[] bytes3 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(Lang + "\r");
				fileStream.Write(bytes3, 0, bytes3.Length);
				byte[] bytes4 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(state + "\r");
				fileStream.Write(bytes4, 0, bytes4.Length);
				byte[] bytes5 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(replaceCfg + "\r");
				fileStream.Write(bytes5, 0, bytes5.Length);
				byte[] bytes6 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(turnOffLauncher + "\r");
				fileStream.Write(bytes6, 0, bytes6.Length);
				byte[] bytes7 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(lastDetected + "\r");
				fileStream.Write(bytes7, 0, bytes7.Length);
				byte[] bytes8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(firstRun + "\r");
				fileStream.Write(bytes8, 0, bytes8.Length);
				byte[] bytes9 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(todayIs.ToString() + "\r");
				fileStream.Write(bytes9, 0, bytes9.Length);
			}
		}

		private void Form1_Closing(object sender, CancelEventArgs e)
		{
		}

		private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			if (updateInProgress || archiveBegun)
			{ 
					string text = "Видимо автор малок если это сообщение появилось. Проблема в update.OnClosing, с языком";
					if (Lang == "ru")
					{
						text = "Вы уверены, что хотите закрыть это окно? Обновление в процессе";
					}
					if (Lang == "eng")
					{
						text = "Update is currently in progress. Are you sure that you want to close the window?";
					}
					switch (MessageBox.Show(text, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
					{
						case DialogResult.Yes:
							changeEnabledStatusButtons();					
					
							ZipArchiveExtensions.setUpdateCancel(true);
							FileDownloader.getSetAbourt(true);
							setVariables(tempFilePath);
							break;
						case DialogResult.No:
							updateRequired = true;
							cancelEventArgs.Cancel = true;
							break;
					}
				}
			
			else
			{
				
				setVariables(tempFilePath);
			}
			
		}

		public void RecreateConfig(string fileName)
		{
			using (FileStream fileStream = File.Create(fileName))
			{
				firstRun = true;
				pathToFile = "SOSI PUTYA NE BUDET PERVI ZAPUSK";
				Lang = "none";
				state = "ХРОНИЧЕСКАЯ ДЕПРЕССИЯ";
				replaceCfg = true;
				turnOffLauncher = false;
				lastDetected = 7.0;
				thoughtOfTheDay = 1;
				byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(1 + "\r");
				fileStream.Write(bytes, 0, bytes.Length);
				byte[] bytes2 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes("SOSI PUTYA NE BUDET PERVI ZAPUSK\r");
				fileStream.Write(bytes2, 0, bytes2.Length);
				byte[] bytes3 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes("none\r");
				fileStream.Write(bytes3, 0, bytes3.Length);
				byte[] bytes4 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes("ХРОНИЧЕСКАЯ ДЕПРЕССИЯ\r");
				fileStream.Write(bytes4, 0, bytes4.Length);
				byte[] bytes5 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes("false\r");
				fileStream.Write(bytes5, 0, bytes5.Length);
				byte[] bytes6 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes("false\r");
				fileStream.Write(bytes6, 0, bytes6.Length);
				byte[] bytes7 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(7.0 + "\r");
				fileStream.Write(bytes7, 0, bytes7.Length);
				byte[] bytes8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes("true\r");
				fileStream.Write(bytes8, 0, bytes8.Length);
				byte[] bytes9 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(DateTime.Today.ToString() + "\r");
				fileStream.Write(bytes9, 0, bytes9.Length);
			}
		}

		public void getVariables(string fileName)
		{
			try
			{
				if (File.Exists(fileName))
				{
					using (StreamReader streamReader = File.OpenText(fileName))
					{
						List<string> list = new List<string>();
						string text = "";
						while ((text = streamReader.ReadLine()) != null)
						{
							list.Add(text);
						}
						int.TryParse(list[0], out int result);
						thoughtOfTheDay = result;
						pathToFile = list[1];
						Lang = list[2];
						state = list[3];
						bool.TryParse(list[4], out bool result2);
						replaceCfg = result2;
						bool.TryParse(list[5], out bool result3);
						turnOffLauncher = result3;
						double.TryParse(list[6], out double result4);
						lastDetected = result4;
						bool.TryParse(list[7], out bool result5);
						firstRun = result5;
						todayIs = Convert.ToDateTime(list[8]);
					}
				}
				else
				{
					RecreateConfig(fileName);
				}
			}
			catch (Exception ex)
			{
				RecreateConfig(fileName);
				Console.WriteLine(ex.ToString());
			}
		}

		public void changeEnabledStatusButtons()
		{
			if (oneWindowIsAlreadyLaunched)
			{
				return;
			}
			string versionPath = StringProcessing.StepUp(pathToFile) + "\\Mods\\version.txt";
			bool flag = isPathCorrect && checkVersionContent(versionPath) && File.Exists(StringProcessing.StepUp(pathToFile) + "//Mods//ModCFG.txt");
			if (flag)
			{
				play.Enabled = true;
			}
			if (isPathCorrect)
			{
				checkUpdates.Enabled = true;
			}
			if (flag)
			{
				if (play.Enabled)
				{
					play.Image = Resources._2ContinueA;
				}
				else
				{
					play.Image = Resources._2ContinueD;
				}
			}
			if (isPathCorrect)
			{
				if (checkUpdates.Enabled)
				{
					checkUpdates.Image = Resources._2SettingsA;
				}
				else
				{
					checkUpdates.Image = Resources._2SettingsD;
				}
			}
		}

		public void setRu()
		{
			pathToGuide = "https://steamcommunity.com/sharedfiles/filedetails/?id=1993610246";
			label2.Text = ruButtons[0];
			label3.Text = ruButtons[1];
			label4.Text = ruButtons[2];
			label5.Text = ruButtons[3];
			label6.Text = ruButtons[4];
			label7.Text = ruButtons[5];
			updateLauncherTB.Text = ruButtons[7];
			updateLauncherPB.Image = SRHDLauncher.Properties.Resources._Disk;
			thoughts = thoughtsRu;
			thoughtOfTheDayLbl.Text = thoughtsRu[thoughtOfTheDay];
			toolTip1.SetToolTip(pictureBox2, ruButtons[6]);
			toolTip1.SetToolTip(downloadSR1HD, ruButtons[8]);
			linkToWebSite = "https://ivanpopov1604.wixsite.com/shuniverse";
			linkToGallery = "https://ivanpopov1604.wixsite.com/shuniverse/galereya";
		}

		public void setEng()
		{
			pathToGuide = "https://steamcommunity.com/sharedfiles/filedetails/?id=1994100310";
			label2.Text = engButtons[0];
			label3.Text = engButtons[1];
			label4.Text = engButtons[2];
			label5.Text = engButtons[3];
			label6.Text = engButtons[4];
			label7.Text = engButtons[5];
			updateLauncherTB.Text = engButtons[7];
			updateLauncherPB.Image = SRHDLauncher.Properties.Resources._Disk;
			thoughts = thoughtsEng;
			thoughtOfTheDayLbl.Text = thoughtsEng[thoughtOfTheDay];
			toolTip1.SetToolTip(pictureBox2, engButtons[6]);
			toolTip1.SetToolTip(downloadSR1HD, engButtons[8]);
			linkToWebSite = "https://ivanpopov1604.wixsite.com/shuniverse?lang=en";
			linkToGallery = "https://ivanpopov1604.wixsite.com/shuniverse/galereya?lang=en";
		}

		public bool autodetect()
		{
			Assembly.GetExecutingAssembly().GetManifestResourceNames();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			string downloadPath = null;
			try
			{
				downloadPath = Application.StartupPath + "\\Rangers.exe";
				flag = (StringProcessing.getAppName(downloadPath) == "Rangers.exe");
				flag2 = Directory.Exists(StringProcessing.StepUp(downloadPath) + "\\DATA");
				flag3 = File.Exists(downloadPath);
			}
			catch (Exception)
			{
			}
			if (flag && flag2 && flag3)
			{
				try
				{
					isPathCorrect = BoolConfirmation.SetPath(ref downloadPath);
				}
				catch (Exception ex2)
				{
					MessageBox.Show(ex2.ToString());
				}
				if (isPathCorrect)
				{
					pathToFile = downloadPath;
				}
			}
			else
			{
				downloadPath = pathToFile;
				if (StringProcessing.getAppName(downloadPath) == "Rangers.exe" && Directory.Exists(StringProcessing.StepUp(pathToFile) + "\\DATA") && File.Exists(downloadPath))
				{
					isPathCorrect = BoolConfirmation.SetPath(ref downloadPath);
					changeEnabledStatusButtons();
				}
			}
			if (!isPathCorrect)
			{
				string str = "Unable to find ";
				string caption = "Error!";
				if (Lang == "ru")
				{
					str = "Невозможно автоматически найти ";
					caption = "Ошибка";
				}
				if (Lang == "eng")
				{
					str = "Unable to find ";
					caption = "Error!";
				}
				MessageBox.Show(str + "Rangers.exe", caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				isPathCorrect = BoolConfirmation.OpenDialog(ref executabePath);

				pathToFile = executabePath;
				changeEnabledStatusButtons();
			}
			else
			{
				executabePath = pathToFile;
			}
			executabePath = pathToFile;
			return isPathCorrect;
		}

		public void checkForUpdateMsg()
		{

			string path = Path.GetTempPath() + "updateTest.txt";
			string path2 = Path.GetTempPath() + "updateTest081.txt";
			if (File.Exists(path2))
			{
				File.Delete(path2);
			}
			if (File.Exists(path))
			{
				string s = StringProcessing.StepUp(pathToFile) + "\\Mods\\Mods";
				if (System.IO.Directory.Exists(s))
				{
					Directory.Delete(s, true);
				}
				string text = "" + AppInfo.LAUNCHER_VERSION;
				showLog();
				File.Delete(path);
			}
		}

		public void CopyResource(string resourceName, string file)
		{
			using (Stream stream = GetType().Assembly.GetManifestResourceStream(resourceName))
			{
				if (stream == null)
				{
					throw new ArgumentException("No such resource", "resourceName");
				}
				using (Stream destination = File.OpenWrite(file))
				{
					stream.CopyTo(destination);
				}
			}
		}

		private bool BackUpModeFiles(string configFileName)
		{
			string path = Application.StartupPath + "\\" + configFileName;
			if (File.Exists(path))
			{
				return true;
			}
			return false;
		}

		[DllImport("user32", CharSet = CharSet.Auto)]
		static extern bool PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);

		[DllImport("user32", CharSet = CharSet.Auto)]
		static extern bool ReleaseCapture();

		[DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
		public static extern int AddFontResource([In] [MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

		public void InitialiseFont()
		{
			string path = StringProcessing.StepUp(pathToFile) + "\\testUBCalculator.otf";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			string text = Path.GetTempPath() + "UBCalculator.otf";
			if (!File.Exists(text))
			{
				CopyResource("SRHDLauncher.Resources.UBCalculator.otf", text);
			}
			Fonts.AddFontFile(text);
			calcFont = new Font(Fonts.Families[0], label2.Font.Size);
			label2.Font = calcFont;
			label3.Font = calcFont;
			label4.Font = calcFont;
			label5.Font = calcFont;
			label6.Font = calcFont;
			label7.Font = calcFont;
			updateLauncherTB.Font = calcFont;
		}

		private static int Get45or451FromRegistry()
		{
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
			{
				int result = Convert.ToInt32(registryKey.GetValue("Release"));
				bool flag = true;
				return result;
			}
		}

		public void mainMenuConstruct()
		{
			isPathCorrect = autodetect();
			authors.Text = "by Kurcha 2020. Launcher version: " + AppInfo.LAUNCHER_VERSION;
		}

		public static bool CheckForInternetConnection()
		{
			try
			{
				using (WebClient webClient = new WebClient())
				{
					using (webClient.OpenRead("http://google.com/generate_204"))
					{
						return true;
					}
				}
			}
			catch
			{
				return false;
			}
		}

		public void changeVisibleState()
		{
			pictureBox6.Visible = !pictureBox6.Visible;
			downloadSR1HD.Visible = !downloadSR1HD.Visible;
			button2.Visible = !button2.Visible;
			pictureBox2.Enabled = !pictureBox2.Enabled;
			pictureBox2.Visible = !pictureBox2.Visible;
			pictureBox3.Enabled = !pictureBox3.Enabled;
			pictureBox3.Visible = !pictureBox3.Visible;
			pictureBox4.Enabled = !pictureBox4.Enabled;
			pictureBox4.Visible = !pictureBox4.Visible;
			webBrowser1.Visible = !webBrowser1.Visible;
			play.Enabled = !play.Enabled;
			play.Visible = !play.Visible;
			checkUpdates.Enabled = !checkUpdates.Enabled;
			checkUpdates.Visible = !checkUpdates.Visible;
			discordServer.Enabled = !discordServer.Enabled;
			discordServer.Visible = !discordServer.Visible;
			Settings.Enabled = !Settings.Enabled;
			Settings.Visible = !Settings.Visible;
			guide.Enabled = !guide.Enabled;
			guide.Visible = !guide.Visible;
			exit.Enabled = !exit.Enabled;
			exit.Visible = !exit.Visible;
			label1.Enabled = !label1.Enabled;
			label1.Visible = !label1.Visible;
			label2.Enabled = !label2.Enabled;
			label2.Visible = !label2.Visible;
			label3.Enabled = !label3.Enabled;
			label3.Visible = !label3.Visible;
			label4.Enabled = !label4.Enabled;
			label4.Visible = !label4.Visible;
			label5.Enabled = !label5.Enabled;
			label5.Visible = !label5.Visible;
			label6.Enabled = !label6.Enabled;
			label6.Visible = !label6.Visible;
			label7.Enabled = !label7.Enabled;
			label7.Visible = !label7.Visible;
			updateLauncherPB.Visible = !updateLauncherPB.Visible;
			updateLauncherTB.Visible = !updateLauncherTB.Visible;
		}

		public mainform()
		{
			if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count() > 1)
			{
				Process.GetCurrentProcess().Kill();
			}
			if (Get45or451FromRegistry() < 461308)
			{
				string text = "";
				if (Lang == "ru")
				{
					text = "Необходим Microsoft .NET Framework 4.7.1 или выше.";
				}
				if (Lang == "eng")
				{
					text = "Microsoft .NET Framework 4.7.1 or higher required";
				}
				MessageBox.Show(text, "!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (CheckForInternetConnection())
			{
				tempFilePath = Path.GetTempPath() + "SRHDLauncherSettings.txt";
				getVariables(tempFilePath);
				base.Closing += OnClosing;
				base.TopMost = false;
				InitializeComponent();
				checkForUpdateMsg();
				string currentDirectory = Directory.GetCurrentDirectory();
				base.FormBorderStyle = FormBorderStyle.None;
				base.MouseDown += mainMenu_MouseDown;
				base.MouseDown += backgroundPB_MouseDown;
				string path = Path.GetTempPath() + "updateTest.txt";
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				if (Lang == "ru")
				{
					webBrowser1.Navigate("http://klissancall.000webhostapp.com/rusNews.html");
				}
				if (Lang == "eng")
				{
					webBrowser1.Navigate("http://klissancall.000webhostapp.com/engNews.html");
				}
			}
			else
			{
				string text2 = "Check your internet connection";
				if (Lang == "ru")
				{
					text2 = "Отсустует соединение с интернетом";
				}
				if (Lang == "eng")
				{
					text2 = "Check your internet connection";
				}
				MessageBox.Show(text2);
			}
		}

		private void mainform_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		private void mainMenu_Load(object sender, EventArgs e)
		{
			if (Lang == "none")
			{
				changeVisibleState();
				langChoose langChoose = new langChoose("", this);
				langChoose.Show();
			}
			else
			{
				BackgroundImage = Resources.BGGreen;
				mainMenuConstruct();
				initialise();
			}
		}

		private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
		}

		private void background_Click(object sender, EventArgs e)
		{
		}

		public void RepeatUntilBusy(WebClient client)
		{
			while (client!=null && client.IsBusy)
			{
				Application.DoEvents();
				if (!updateInProgress)
				{
					if (abortEtoGreh)
					{
						client.CancelAsync();
						client = null;
					}
				}
			}
		}

		private void mainMenu_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			PostMessage(base.Handle, 274u, 61458u, 0u);
		}

		private void backgroundPB_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			PostMessage(base.Handle, 274u, 61458u, 0u);
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			exit.Image = Resources._2ExitD;
		}

		private void exit_MouseUp(object sender, MouseEventArgs e)
		{
			exit.Image = Resources._2ExitA;
			Close();
		}

		private void Settings_MouseUp(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
			Settings.Image = Resources._2OptionsA;
			if (settingsForm == null)
			{
				settingsForm = new settings(isPathCorrect, executabePath, this, updateForm, playForm, isModInstalled);
			}
			if (playForm != null)
			{
				settingsForm.playForm = playForm;
			}
			if (updateForm != null)
			{
				settingsForm.updateForm = updateForm;
			}
			settingsForm.Show();
		}

		private void Settings_MouseDown(object sender, MouseEventArgs e)
		{
			Settings.Image = Resources._2OptionsD;
		}

		private void checkUpdates_MouseDown(object sender, MouseEventArgs e)
		{
			checkUpdates.Image = Resources._2SettingsD;
		}

		private void checkUpdates_MouseUp(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
			checkUpdates.Image = Resources._2SettingsA;
			if (!launcherIsOutdated)
			{
				bool sizeDiffers = true;
				updateRequired = BoolConfirmation.checkIfUpdateIsRequired(pathToFile, "https://drive.google.com/file/d/1jDScpEkq-mybtv4SNtL-rjyE-9wM4Uos/view?usp=sharing", ref message, null, this, ref totalBytes, ref sizeDiffers, ref info);
				if (updateRequired)
				{
					if (updateForm != null) updateForm = null;
					myMessageBox = FormsFunctions.CallMessageBox(message, this);
					if (myMessageBox != null)
					{
						myMessageBox.Activate();
					}
				}
				else
				{
					message = "checkUpdates_mouseup";
					if (Lang == "ru")
					{
						message = "Обновления не требуются";
					}
					if (Lang == "eng")
					{
						message = "No updates required";
					}
					MessageBox.Show(message);
				}
			}
			else if (launcherIsOutdated)
			{
				updLauncher();
			}

		}

		private void label1_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox1_MouseHover(object sender, EventArgs e)
		{
			discordServer.Image = Resources.rangerSmall;
		}

		private void discordServer_MouseLeave(object sender, EventArgs e)
		{
			discordServer.Image = Resources.rangerSmall1;
		}

		private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
		{
			play.Image = Resources._2ContinueD;
		}

		private void pictureBox2_MouseDown_1(object sender, MouseEventArgs e)
		{
			pictureBox2.Image = Resources._2ScanerD;
		}

		private void play_MouseUp(object sender, MouseEventArgs e)
		{
			play.Image = Resources._2ContinueA;
			if (firstRun)
			{
				try
				{
					if (updateForm == null)
					{
						FormsFunctions.PlayButtonSound();
						playForm = new play(executabePath, this, turnOffLauncher);
						if (settingsForm != null)
						{
							settingsForm.playForm = playForm;
						}
						playForm.Show();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "");
				}
			}
			else
			{
				try
				{
					FormsFunctions.PlayLeaveButtonSound();
					Process.Start(executabePath);
					if (turnOffLauncher)
					{
						Close();
					}
				}
				catch (Exception ex2)
				{
					MessageBox.Show(ex2.ToString(), "");
				}
			}
		}

		private void guide_MouseDown(object sender, MouseEventArgs e)
		{
			guide.Image = Resources._2HelpD;
		}

		private void guide_MouseUp(object sender, MouseEventArgs e)
		{
			guide.Image = Resources._2HelpA;
			FormsFunctions.PlayButtonSound();
			FileDownloader.OpenUrl(pathToGuide);
		}

		private void play_Click(object sender, EventArgs e)
		{
		}

		private void backgroundPB_Paint(object sender, PaintEventArgs e)
		{
		}

		private void Settings_Click(object sender, EventArgs e)
		{
		}

		private void checkUpdates_Click(object sender, EventArgs e)
		{
		}

		public void callUpdate(string path, bool reisntall, bool SR1HD)
		{
			if (updateForm == null)
			{
				if (!SR1HD)
				{
					updateForm = new update(pathToFile, updateRequired, totalBytes, this, isModInstalled, info, reisntall, SR1HD);
				}
				else
				{
					updateForm = new update(path, updateRequired, totalBytes, this, isModInstalled, info, reisntall, SR1HD);
				}

			}
		}
		public void updLauncher()
		{
			if (!launcherIsOutdated)
			{
				return;
			}
			string text = "Проблема в mainMenu_load. Язык. Или в авторе.";
			if (Lang == "ru")
			{
				text = "Доступно обновление лаунчера. Хотите его скачать?";
			}
			if (Lang == "eng")
			{
				text = "The update is avaliable. Do you want to download it?";
			}
			DialogResult dialogResult = MessageBox.Show(text, "!", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				long totalBytesToUpdate = 1L;
				string text2 = appPath + "\\tempLauncher.exe";
				FileInfo fileInfo = FileDownloader.DownloadFileFromURLToPath("https://drive.google.com/file/d/15s5SHvq_zcMFm9auz6tteTYFPmbqQu4Q/view?usp=sharing", text2, callProgressBar: false, null, this, totalBytesToUpdate, "Скачивание мода :");
				if (fileInfo != null)
				{
					string path = Path.GetTempPath() + "updateTest.txt";
					File.Create(path);
					string format = "/C Choice /C Y /N /D Y /T 2 & Del /Q \"{0}\" &  Choice /C Y /N /D Y /T 1  & Ren \"{1}\" \"{2}\"  & Start \"\" /D  \"{3}\" \"{4}\"  {5} ";
					ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd", "cd c:");
					string text3 = appPath;
					processStartInfo.Arguments = string.Format(format, executePath, text2, "Launcher.exe", appPath, executePath, "");
					processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					processStartInfo.WorkingDirectory = appPath;
					processStartInfo.FileName = "cmd.exe";
					Process.Start(processStartInfo);
					Thread.Sleep(50);
					Close();
				}
			}
		}

		public void initialise()
		{
			if (File.Exists(Application.StartupPath + "\\TempLauncher.exe"))
			{
				File.Delete(Application.StartupPath + "\\TempLauncher.exe");
			}
			try
			{
				DateTime now = DateTime.Now;
				DateTime todayIs = mainform.todayIs;
				string b = now.ToString("MM-dd-yyyy");
				string a = todayIs.ToString("MM-dd-yyyy");
				if (a != b)
				{
					thoughtOfTheDay++;
					if (thoughtOfTheDay > 12)
					{
						thoughtOfTheDay = 1;
					}
				}
				mainform.todayIs = now;
				if (Lang == "ru")
				{
					setRu();
				}
				if (Lang == "eng")
				{
					setEng();
				}
				long num = 1L;
				launcherIsOutdated = BoolConfirmation.checkIfUpdateIsRequired(appPath, "https://drive.google.com/file/d/1vwfyczFB_riZO5oEVMelDmtNRapriFBi/view?usp=sharing", "" + AppInfo.LAUNCHER_VERSION, null, this, ref totalBytes);
				if (launcherIsOutdated)
				{
					checkUpdates.Enabled = true;
					checkUpdates.Image = Resources._2SettingsA;
					Settings.Enabled = false;
					Settings.Image = Resources._2OptionsD;
					updLauncher();
				}
				else
				{
					if (isPathCorrect && !launcherIsOutdated)
					{
						string text = StringProcessing.StepUp(pathToFile) + "\\Mods\\version.txt";
						checkUpdates.Enabled = true;
						checkUpdates.Image = Resources._2SettingsA;
						if (File.Exists(text) && isPathCorrect && checkVersionContent(text) && File.Exists(StringProcessing.StepUp(pathToFile) + "//Mods//ModCFG.txt"))
						{
							play.Enabled = true;
							play.Image = Resources._2ContinueA;
						}
					}
					InitialiseFont();
					string text2 = "";
					bool sizeDiffers = false;
					updateRequired = BoolConfirmation.checkIfUpdateIsRequired(pathToFile, "https://drive.google.com/file/d/1jDScpEkq-mybtv4SNtL-rjyE-9wM4Uos/view?usp=sharing", ref message, null, this, ref totalBytes, ref sizeDiffers, ref info);
					if (updateRequired && isPathCorrect)
					{
						myMessageBox = FormsFunctions.CallMessageBox(message, this);

					}
				}
				SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);

				if (myMessageBox != null)
				{
					myMessageBox.Activate();
				}
				string path = StringProcessing.StepUp(pathToFile) + "\\Mods\\version.txt";
				string text3 = StringProcessing.StepUp(pathToFile) + "\\Mods\\ModCfg.txt";
				if (File.Exists(path) && File.Exists(StringProcessing.StepUp(pathToFile) + "\\Mods\\ModCfg.txt"))
				{
					isModInstalled = true;
					string s = File.ReadAllText(path);
					if (double.TryParse(s, out double result))
					{
						lastDetected = result;
					}
				}
				else if (StringProcessing.getAppName(pathToFile) == "Rangers.exe" && File.Exists(path))
				{
					using (FileStream fileStream = File.Create(path))
					{
						byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(lastDetected.ToString());
						fileStream.Write(bytes, 0, bytes.Length);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void mainMenu_Shown(object sender, EventArgs e)
		{
		}

		private void label2_Click(object sender, EventArgs e)
		{
		}

		private void play_MouseMove(object sender, MouseEventArgs e)
		{
		}

		private void Settings_MouseClick(object sender, MouseEventArgs e)
		{
		}

		private void changeLog_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
		}

		private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
		}

		private void messageToUser_Paint(object sender, PaintEventArgs e)
		{
		}

		private void changeLog_TextChanged(object sender, EventArgs e)
		{
		}

		private void discordServer_Click(object sender, EventArgs e)
		{
			FileDownloader.OpenUrl("https://discord.gg/uQc2Qfh");
		}

		private void guide_Click(object sender, EventArgs e)
		{
		}

		private void webBrowser_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
		}

		private void exit_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			FileDownloader.OpenUrl(linkToWebSite);
			FormsFunctions.PlayInfoButtonSound();
		}

		private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
		{
			pictureBox2.Image = Resources._2ScanerH;
			FileDownloader.OpenUrl("http://klissancall.000webhostapp.com");
			FormsFunctions.PlayInfoButtonSound();
		}

		private void pictureBox1_MouseDown_2(object sender, MouseEventArgs e)
		{
		}

		private void pictureBox1_MouseDown_3(object sender, MouseEventArgs e)
		{
		}

		private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
		{
			FileDownloader.OpenUrl(linkToGallery);
			FormsFunctions.PlayInfoButtonSound();
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			FileDownloader.OpenUrl("https://www.moddb.com/mods/shuniverse-klissan-call");
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			FileDownloader.OpenUrl("https://www.nexusmods.com/spacerangersawarapart/mods/6/");
		}

		public void setDefaultBebug()
		{
			pathToFile = "SOSIBIBUNULL";
			lastDetected = 7.0;
			thoughtOfTheDay = 1;
			replaceCfg = true;
			turnOffLauncher = false;
			state = "nan";
			pathToFile = "";
			firstRun = true;
			Lang = "ru";
		}

		private void button2_Click(object sender, EventArgs e)
		{
			setDefaultBebug();
		}

		private void button2_Click_1(object sender, EventArgs e)
		{
			setDefaultBebug();
		}

		private void pictureBox5_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox5_Paint(object sender, PaintEventArgs e)
		{
		}

		public void showLog()
		{

			string text = "no lang selected";
			if (Lang == "ru")
			{
				text = "Список изменений версии: " + AppInfo.LAUNCHER_VERSION;
				for (int i = 0; i < AppInfo.laucnherUpdateLogRu.Length; i++)
				{
					text = text + "\r\n - " + AppInfo.laucnherUpdateLogRu[i];
				}
			}
			if (Lang == "eng")
			{
				text = "Changelog of version: " + AppInfo.LAUNCHER_VERSION;
				for (int j = 0; j < AppInfo.laucnherUpdateLogEng.Length; j++)
				{
					text = text + "\r\n - " + AppInfo.laucnherUpdateLogEng[j];
				}
			}
			MessageBox.Show(text);
		}

		private void pictureBox6_MouseUp(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
			pictureBox6.Image = Resources.modnaya_trapetsaya_not_pressed;
			showLog();
		}

		private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
		{
			pictureBox6.Image = Resources.modnaya_trapetsaya_pressed;
		}

		private void pictureBox5_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
		}

		private void pictureBox6_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox5_Click_1(object sender, EventArgs e)
		{
		}

		private void webBrowser1_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			InternetExplorer internetExplorer = webBrowser1.ActiveXInstance as InternetExplorer;
			object pvaIn = 70;
			object pvaOut = IntPtr.Zero;
			internetExplorer.ExecWB(OLECMDID.OLECMDID_OPTICAL_ZOOM, OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref pvaIn, ref pvaOut);
		}

		private void button2_Click_2(object sender, EventArgs e)
		{
			webBrowser1.GoBack();
		}

		private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
		}

		public void downloadSR1HDMethod()
		{
		}

		private void downloadSR1HD_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void downloadSR1HD_Click(object sender, EventArgs e)
		{
			if (!checkUpdates.Enabled)
			{
				return;
			}
			FormsFunctions.PlayInfoButtonSound();
			string text = "downloadSR1HD";
			string text2 = "downloadSR1HDUpdateMessage";
			string sRHD1Url = "https://drive.google.com/file/d/1SajJJ421VaesBuGFU80Vo6qkO-0c7iew/view?usp=sharing";
			if (Lang == "ru")
			{
				sRHD1Url = "https://drive.google.com/file/d/1SajJJ421VaesBuGFU80Vo6qkO-0c7iew/view?usp=sharing";
				text2 = "                  Выбрана скачка HD текстур для Space Rangers 1";
				text = "Укажите путь к исполняемому файлу игры SpaceRangers 1";
			}
			if (Lang == "eng")
			{
				sRHD1Url = "https://drive.google.com/file/d/17Ypd9BTddPXKzTe_E1l8ag7S5gZDEQY5/view?usp=sharing";
				text2 = "            HD textures for Space Rangers 1 were chosen to download";
				text = "Select destination of the executable file of Space Rangers 1";
			}
			if (updateForm == null)
			{
				MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				bool flag = false;
				string s = "";
				if (BoolConfirmation.OpenDialogSR1HD(ref s))
				{
					callUpdate(StringProcessing.StepUp(s), reisntall: false, SR1HD: true);
					updateForm.SRHD1Url = sRHD1Url;
					updateForm.downloadSR1HDMode = true;
				}
			}
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainform));
            this.button1 = new System.Windows.Forms.Button();
            this.versionLbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.thoughtOfTheDayLbl = new System.Windows.Forms.Label();
            this.authors = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.downloadSR1HD = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.updateLauncherPB = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.button2 = new System.Windows.Forms.Button();
            this.updateLauncherTB = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.guide = new System.Windows.Forms.PictureBox();
            this.play = new System.Windows.Forms.PictureBox();
            this.discordServer = new System.Windows.Forms.PictureBox();
            this.checkUpdates = new System.Windows.Forms.PictureBox();
            this.Settings = new System.Windows.Forms.PictureBox();
            this.exit = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.downloadSR1HD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateLauncherPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.play)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discordServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkUpdates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Settings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exit)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.Location = new System.Drawing.Point(278, -93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 27);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // versionLbl
            // 
            this.versionLbl.AutoSize = true;
            this.versionLbl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.versionLbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.versionLbl.Location = new System.Drawing.Point(12, 521);
            this.versionLbl.Name = "versionLbl";
            this.versionLbl.Size = new System.Drawing.Size(0, 13);
            this.versionLbl.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(705, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 26);
            this.label2.TabIndex = 12;
            this.label2.Text = "Играть";
            this.label2.UseCompatibleTextRendering = true;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(705, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 26);
            this.label3.TabIndex = 13;
            this.label3.Text = "Обновления";
            this.label3.UseCompatibleTextRendering = true;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Yu Gothic UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(705, 220);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 49);
            this.label4.TabIndex = 14;
            this.label4.Text = "Дискорд сервер ЦР Полюс Мира.";
            this.label4.UseCompatibleTextRendering = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Yu Gothic UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(705, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(175, 25);
            this.label5.TabIndex = 15;
            this.label5.Text = "Настройки";
            this.label5.UseCompatibleTextRendering = true;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("Yu Gothic UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(705, 350);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(175, 24);
            this.label6.TabIndex = 16;
            this.label6.Text = "Руководство по моду";
            this.label6.UseCompatibleTextRendering = true;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Yu Gothic UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(705, 408);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(176, 23);
            this.label7.TabIndex = 17;
            this.label7.Text = "ЯРИК ПРИШЕЛ";
            this.label7.UseCompatibleTextRendering = true;
            // 
            // thoughtOfTheDayLbl
            // 
            this.thoughtOfTheDayLbl.AutoSize = true;
            this.thoughtOfTheDayLbl.BackColor = System.Drawing.Color.Transparent;
            this.thoughtOfTheDayLbl.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.thoughtOfTheDayLbl.ForeColor = System.Drawing.Color.Black;
            this.thoughtOfTheDayLbl.Location = new System.Drawing.Point(43, 51);
            this.thoughtOfTheDayLbl.Name = "thoughtOfTheDayLbl";
            this.thoughtOfTheDayLbl.Size = new System.Drawing.Size(0, 21);
            this.thoughtOfTheDayLbl.TabIndex = 20;
            // 
            // authors
            // 
            this.authors.AutoSize = true;
            this.authors.BackColor = System.Drawing.Color.Transparent;
            this.authors.ForeColor = System.Drawing.Color.Black;
            this.authors.Location = new System.Drawing.Point(43, 561);
            this.authors.Name = "authors";
            this.authors.Size = new System.Drawing.Size(0, 13);
            this.authors.TabIndex = 21;
            // 
            // downloadSR1HD
            // 
            this.downloadSR1HD.BackColor = System.Drawing.Color.Transparent;
            this.downloadSR1HD.Image = global::SRHDLauncher.Properties.Resources.downloadSR1HDLauncher;
            this.downloadSR1HD.Location = new System.Drawing.Point(656, 492);
            this.downloadSR1HD.Name = "downloadSR1HD";
            this.downloadSR1HD.Size = new System.Drawing.Size(52, 49);
            this.downloadSR1HD.TabIndex = 37;
            this.downloadSR1HD.TabStop = false;
            this.toolTip1.SetToolTip(this.downloadSR1HD, "Скачать HD текстуры на Space Rangers 1");
            this.downloadSR1HD.Click += new System.EventHandler(this.downloadSR1HD_Click);
            this.downloadSR1HD.MouseUp += new System.Windows.Forms.MouseEventHandler(this.downloadSR1HD_MouseUp);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::SRHDLauncher.Properties.Resources._2ScanerH;
            this.pictureBox2.Location = new System.Drawing.Point(710, 452);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(41, 34);
            this.pictureBox2.TabIndex = 24;
            this.pictureBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox2, "Ссылка на сайт");
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown_1);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseUp);
            // 
            // updateLauncherPB
            // 
            this.updateLauncherPB.BackColor = System.Drawing.Color.Transparent;
            this.updateLauncherPB.Location = new System.Drawing.Point(658, 546);
            this.updateLauncherPB.Name = "updateLauncherPB";
            this.updateLauncherPB.Size = new System.Drawing.Size(44, 33);
            this.updateLauncherPB.TabIndex = 38;
            this.updateLauncherPB.TabStop = false;
            this.updateLauncherPB.Click += new System.EventHandler(this.updateLauncherPB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(704, 473);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 29;
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Location = new System.Drawing.Point(47, 96);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(598, 451);
            this.webBrowser1.TabIndex = 35;
            this.webBrowser1.Url = new System.Uri("http://klissancall.000webhostapp.com", System.UriKind.Absolute);
            this.webBrowser1.WebBrowserShortcutsEnabled = false;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted_1);
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(592, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(37, 23);
            this.button2.TabIndex = 36;
            this.button2.Text = "<-";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_2);
            // 
            // updateLauncherTB
            // 
            this.updateLauncherTB.BackColor = System.Drawing.Color.Transparent;
            this.updateLauncherTB.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.updateLauncherTB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateLauncherTB.Font = new System.Drawing.Font("Yu Gothic UI", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.updateLauncherTB.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.updateLauncherTB.Location = new System.Drawing.Point(696, 555);
            this.updateLauncherTB.Name = "updateLauncherTB";
            this.updateLauncherTB.Size = new System.Drawing.Size(207, 28);
            this.updateLauncherTB.TabIndex = 39;
            this.updateLauncherTB.Text = "Обновить лаунчер";
            this.updateLauncherTB.UseCompatibleTextRendering = true;
            this.updateLauncherTB.Click += new System.EventHandler(this.updateLauncherTB_Click);
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox6.Image = global::Properties.Resources.МОДНЯВАЯ_ТРАПЕЦИЯ1;
            this.pictureBox6.Location = new System.Drawing.Point(651, 452);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(57, 34);
            this.pictureBox6.TabIndex = 33;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Click += new System.EventHandler(this.pictureBox6_Click);
            this.pictureBox6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox6_MouseDown);
            this.pictureBox6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox6_MouseUp);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = global::SRHDLauncher.Properties.Resources.СУКА_КАРТИНКА;
            this.pictureBox4.Location = new System.Drawing.Point(714, 492);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(57, 50);
            this.pictureBox4.TabIndex = 26;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Image = global::SRHDLauncher.Properties.Resources.modDB;
            this.pictureBox3.Location = new System.Drawing.Point(777, 492);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(82, 50);
            this.pictureBox3.TabIndex = 25;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // guide
            // 
            this.guide.BackColor = System.Drawing.Color.Transparent;
            this.guide.Image = global::SRHDLauncher.Properties.Resources._2HelpA;
            this.guide.Location = new System.Drawing.Point(650, 336);
            this.guide.Name = "guide";
            this.guide.Size = new System.Drawing.Size(49, 52);
            this.guide.TabIndex = 9;
            this.guide.TabStop = false;
            this.guide.Click += new System.EventHandler(this.guide_Click);
            this.guide.MouseDown += new System.Windows.Forms.MouseEventHandler(this.guide_MouseDown);
            this.guide.MouseUp += new System.Windows.Forms.MouseEventHandler(this.guide_MouseUp);
            // 
            // play
            // 
            this.play.BackColor = System.Drawing.Color.Transparent;
            this.play.Enabled = false;
            this.play.Image = global::SRHDLauncher.Properties.Resources._2ContinueD;
            this.play.Location = new System.Drawing.Point(650, 96);
            this.play.Name = "play";
            this.play.Size = new System.Drawing.Size(49, 52);
            this.play.TabIndex = 8;
            this.play.TabStop = false;
            this.play.Click += new System.EventHandler(this.play_Click);
            this.play.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown_1);
            this.play.MouseMove += new System.Windows.Forms.MouseEventHandler(this.play_MouseMove);
            this.play.MouseUp += new System.Windows.Forms.MouseEventHandler(this.play_MouseUp);
            // 
            // discordServer
            // 
            this.discordServer.BackColor = System.Drawing.Color.Transparent;
            this.discordServer.Image = global::SRHDLauncher.Properties.Resources.rangerSmall1;
            this.discordServer.Location = new System.Drawing.Point(651, 220);
            this.discordServer.Name = "discordServer";
            this.discordServer.Size = new System.Drawing.Size(49, 49);
            this.discordServer.TabIndex = 7;
            this.discordServer.TabStop = false;
            this.discordServer.Click += new System.EventHandler(this.discordServer_Click);
            this.discordServer.MouseLeave += new System.EventHandler(this.discordServer_MouseLeave);
            this.discordServer.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            // 
            // checkUpdates
            // 
            this.checkUpdates.BackColor = System.Drawing.Color.Transparent;
            this.checkUpdates.Enabled = false;
            this.checkUpdates.Image = global::SRHDLauncher.Properties.Resources._2SettingsD;
            this.checkUpdates.Location = new System.Drawing.Point(650, 154);
            this.checkUpdates.Name = "checkUpdates";
            this.checkUpdates.Size = new System.Drawing.Size(49, 52);
            this.checkUpdates.TabIndex = 5;
            this.checkUpdates.TabStop = false;
            this.checkUpdates.Click += new System.EventHandler(this.checkUpdates_Click);
            this.checkUpdates.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkUpdates_MouseDown);
            this.checkUpdates.MouseUp += new System.Windows.Forms.MouseEventHandler(this.checkUpdates_MouseUp);
            // 
            // Settings
            // 
            this.Settings.BackColor = System.Drawing.Color.Transparent;
            this.Settings.Image = global::SRHDLauncher.Properties.Resources._2OptionsA;
            this.Settings.Location = new System.Drawing.Point(650, 275);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(49, 52);
            this.Settings.TabIndex = 4;
            this.Settings.TabStop = false;
            this.Settings.Click += new System.EventHandler(this.Settings_Click);
            this.Settings.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Settings_MouseClick);
            this.Settings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Settings_MouseDown);
            this.Settings.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Settings_MouseUp);
            // 
            // exit
            // 
            this.exit.BackColor = System.Drawing.Color.Transparent;
            this.exit.Image = global::SRHDLauncher.Properties.Resources._2ExitA;
            this.exit.Location = new System.Drawing.Point(650, 394);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(49, 52);
            this.exit.TabIndex = 3;
            this.exit.TabStop = false;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            this.exit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.exit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.exit_MouseUp);
            // 
            // mainform
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(903, 617);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.updateLauncherTB);
            this.Controls.Add(this.downloadSR1HD);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.authors);
            this.Controls.Add(this.thoughtOfTheDayLbl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.versionLbl);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.guide);
            this.Controls.Add(this.play);
            this.Controls.Add(this.discordServer);
            this.Controls.Add(this.checkUpdates);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.updateLauncherPB);
            this.ForeColor = System.Drawing.Color.Coral;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainform";
            this.Text = "ShuniverseLauncher";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainform_FormClosing);
            this.Load += new System.EventHandler(this.mainMenu_Load);
            this.Shown += new System.EventHandler(this.mainMenu_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainMenu_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.downloadSR1HD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateLauncherPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.play)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discordServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkUpdates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Settings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private void updateLauncherPB_Click(object sender, EventArgs e)
		{
			if (launcherIsOutdated)
			{
				updLauncher();
			}
			string updateMsg = "Update is not required";
			if (Lang == "ru")
			{
				updateMsg = "Обновление не требуется";
			}
			MessageBox.Show(updateMsg);
		}
		
		private void updateLauncherTB_Click(object sender, EventArgs e)
		{

		}
		private void button3_Click(object sender, EventArgs e)
		{
			updateForm = new update(pathToFile, updateRequired, totalBytes, this, isModInstalled, info, false, false);
			updateForm.Show();

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}
	}
}


