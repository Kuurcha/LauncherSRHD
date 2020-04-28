using Microsoft.VisualBasic.FileIO;
using SRHDLauncher.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public class settings : Form
	{
		private string defaultPath = "NEIN NEIN NEIN NEIN";

		private bool isModInstalled = false;

		private bool pathIsCorrect;

		private string executePath;

		private static string cfgFilePath;

		private const uint WM_SYSCOMMAND = 274u;

		private const uint DOMOVE = 61458u;

		private const uint DOSIZE = 61448u;

		public string[] ruLabels = 
		{
			"Комплектация модов",
			"  Cохранить пресет настроек",
			"Сменить тип запуска",
			"Путь к игре",
			"Изменить",
			"   Создать бекап папки mods",
			"   Востановить бекап папки mods",
			"Выключать лаунчер при запуске игры",
			"Активно",
			"Cбросить настройки лаунчера"
		};

		public string[] engLabels = 
		{
			"Mod sets",
			"  Save mod set",
			"  Сhange launch settings",
			"Game path",
			"Change",
			"   Create a backup for mods folder",
			"   Restore the mods folder backup",
			"Close the launcher upon game start",
			"Active",
			"Reset launcher setings"
		};

		private static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

		public static Font calcFont;

		private IContainer components = null;

		private PictureBox minimizeWindow;

		private PictureBox closeWindow;

		private Label label2;

		private PictureBox firstSet;

		private PictureBox secondSet;

		private PictureBox thirdSet;

		private CheckBox russian;

		private Label label1;

		private Label label4;

		private CheckBox english;

		private PictureBox checkHash;

		private Label label5;

		private TextBox pathToFile;

		private PictureBox fuckYou;

		private Label label7;

		private PictureBox createBackupButton;

		private PictureBox restoreBackupMods;

		private PictureBox saveState;

		private PictureBox defaultSettings;

		private CheckBox Ubercharge;

		private CheckBox turnOffLauncher;

		private Label label3;

		private ToolTip toolTip1;

		private ToolTip toolTip2;

		private Label label8;

		private FileSystemWatcher fileSystemWatcher1;
		private Label label6;
		private PictureBox pictureBox2;
		private PictureBox reinstallBtn;

		public mainform mainform
		{
			get;
			set;
		}

		public update updateForm
		{
			get;
			set;
		}

		public play playForm
		{
			get;
			set;
		}

		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("user32", CharSet = CharSet.Auto)]
		internal static extern bool PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);

		[DllImport("user32", CharSet = CharSet.Auto)]
		internal static extern bool ReleaseCapture();

		[DllImport("User32.dll")]
		public static extern int SetForegroundWindow(int hWnd);

		public void changeSetStates(bool state)
		{
			firstSet.Enabled = state;
			secondSet.Enabled = state;
			thirdSet.Enabled = state;
		}

		public void changeButtonImageRu()
		{
			string str = StringProcessing.StepUp(mainform.pathToFile);
			bool flag = pathIsCorrect && Directory.Exists(str + "\\Mods");
			if (flag)
			{
				defaultSettings.Image = Resources.restoreSettingsRu;
				defaultSettings.Enabled = true;
			}
			else
			{
				defaultSettings.Image = Resources.pressedRestoreSettingsRu;
				defaultSettings.Enabled = false;
			}
			bool flag2 = flag && File.Exists(str + "\\Mods\\version.txt") && mainform.checkVersionContent(str + "\\Mods\\version.txt");
			if (flag2)
			{
				changeSetStates(state: true);
				createBackupButton.Enabled = true;
				saveState.Enabled = true;
				createBackupButton.Image = Resources.createModSetRu;
				saveState.Image = Resources.createSettingsRu;
			}
			else
			{
				changeSetStates(state: false);
				createBackupButton.Enabled = false;
				saveState.Enabled = false;
				createBackupButton.Image = Resources.pressedCreateModSetRu;
				saveState.Image = Resources.pressedCreateSettingsEng;
			}
			if (flag2 && Directory.Exists(str + "\\Backup\\Mods"))
			{
				restoreBackupMods.Enabled = true;
				restoreBackupMods.Image = Resources.restoreBackupRu;
			}
			else
			{
				restoreBackupMods.Enabled = false;
				restoreBackupMods.Image = Resources.pressedRestoreBackupRu;
			}
			if (flag2 && isModInstalled)
			{
				reinstallBtn.Image = Resources.reinstallRu;
				reinstallBtn.Enabled = true;
			}
			else
			{
				reinstallBtn.Image = Resources.pressedReinstallRu;
				reinstallBtn.Enabled = false;
			}
		}

		public void changeButtonImageEng()
		{
			string str = StringProcessing.StepUp(mainform.pathToFile);
			bool flag = pathIsCorrect && Directory.Exists(str + "\\Mods");
			if (flag)
			{
				defaultSettings.Image = Resources.restoreSettingsEng;
				defaultSettings.Enabled = true;
			}
			else
			{
				defaultSettings.Image = Resources.pressedRestoreSettingsEng;
				defaultSettings.Enabled = false;
			}
			bool flag2 = flag && File.Exists(str + "\\Mods\\version.txt") && mainform.checkVersionContent(str + "\\Mods\\version.txt");
			if (flag2)
			{
				changeSetStates(state: true);
				createBackupButton.Enabled = true;
				saveState.Enabled = true;
				createBackupButton.Image = Resources.createBackupEng;
				saveState.Image = Resources.createSettingsEng;
			}
			else
			{
				changeSetStates(state: false);
				createBackupButton.Enabled = false;
				saveState.Enabled = false;
				createBackupButton.Image = Resources.pressedCreateBackupEng;
				saveState.Image = Resources.pressedCreateSettingsEng;
			}
			if (flag2 && Directory.Exists(str + "\\Backup\\Mods"))
			{
				restoreBackupMods.Enabled = true;
				restoreBackupMods.Image = Resources.resotreBackupEng;
			}
			else
			{
				restoreBackupMods.Enabled = false;
				restoreBackupMods.Image = Resources.pressedResotreBackupEng;
			}
			if (flag2 && isModInstalled)
			{
				reinstallBtn.Image = Resources.reinstalEng;
				reinstallBtn.Enabled = true;
			}
			else
			{
				reinstallBtn.Image = Resources.pressedReinstalEng;
				reinstallBtn.Enabled = false;
			}
		}

		public void setRu()
		{
			changeButtonImageRu();
			label2.Text = ruLabels[0];
			label5.Text = ruLabels[2];
			label7.Text = ruLabels[4];
			label8.Text = ruLabels[7];
			label3.Text = ruLabels[8];
			label6.Text = ruLabels[9];
		}

		public void setEng()
		{
			changeButtonImageEng();
			label2.Text = engLabels[0];
			label5.Text = engLabels[2];
			label7.Text = engLabels[4];
			label8.Text = engLabels[7];
			label3.Text = engLabels[8];
			label6.Text = engLabels[9];
		}

		public void DownloadFiles(string extractPath, string tempPath, string googleDownloadPath, bool callProgressBar, long totalBytes)
		{
		}

		public settings(bool pathIsCorrect, string executePath, mainform mainForm, update updateForm, play playForm, bool isModInstalled)
		{
			this.executePath = executePath;
			mainform = mainForm;
			this.updateForm = updateForm;
			this.playForm = playForm;
			this.pathIsCorrect = pathIsCorrect;
			this.isModInstalled = isModInstalled;
			cfgFilePath = StringProcessing.StepUp(executePath) + "\\Mods\\ModCFG.txt";
			defaultPath = StringProcessing.StepUp(executePath);
			InitializeComponent();
			if (mainform.state == "First")
			{
				firstSet.Image = Resources._1_acitve;
			}
			if (mainform.state == "Second")
			{
				secondSet.Image = Resources._2_active;
			}
			if (mainform.state == "Third")
			{
				thirdSet.Image = Resources._3active;
			}
			pathToFile.Text = mainform.pathToFile;
			base.MouseDown += settings_MouseDown;
			base.MouseDown += pictureBox1_MouseDown;
			base.MouseDown += pictureBox1_MouseDown;
			base.Closing += OnClosing;
			if (mainform.Lang == "ru")
			{
				setRu();
			}
			if (mainform.Lang == "eng")
			{
				setEng();
			}
			Ubercharge.Checked = mainform.replaceCfg;
			turnOffLauncher.Checked = mainform.turnOffLauncher;
			mainform.changeEnabledStatusButtons();
			mainForm.oneWindowIsAlreadyLaunched = true;
			russian.Checked = (mainform.Lang == "ru");
			english.Checked = (mainform.Lang == "eng");
			Show();
		}

		public void RepeatUntilBusy(WebClient client)
		{
			while (client.IsBusy)
			{
				Application.DoEvents();
			}
		}

		private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			if (mainform != null)
			{
				mainform.oneWindowIsAlreadyLaunched = false;
				mainform.changeEnabledStatusButtons();
			}
		}

		private void settings_Load(object sender, EventArgs e)
		{
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();d
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

		private void goodChoice_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void degenerateChoice_MouseUp(object sender, MouseEventArgs e)
		{
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

		private void degenerateChoice_Click(object sender, EventArgs e)
		{
		}

		private void closeWindow_MouseDown(object sender, MouseEventArgs e)
		{
			closeWindow.Image = Resources._2CrossD;
		}

		private void closeWindow_MouseUp(object sender, MouseEventArgs e)
		{
			closeWindow.Image = Resources._2CrossA;
			Close();
			mainform.settingsForm = null;
			FormsFunctions.PlayLeaveButtonSound();
		}

		private void minimizeWindow_MouseUp(object sender, MouseEventArgs e)
		{
			minimizeWindow.Image = Resources._2SubA;
			base.WindowState = FormWindowState.Minimized;
		}

		private void minimizeWindow_MouseDown(object sender, MouseEventArgs e)
		{
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

		private void pictureBox1_MouseDown_2(object sender, MouseEventArgs e)
		{
		}

		private void firstSet_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void secondSet_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void secondSet_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void thirdSet_MouseDown(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
		}

		private void thirdSet_MouseUp(object sender, MouseEventArgs e)
		{
		}

		public void CopyCFgFile()
		{
			if (mainform.state != "стейта не существует его навязывает вам правительство")
			{
				string text = StringProcessing.StepUp(executePath) + "\\Mods\\ModCFG" + mainform.state + ".txt";
				if (File.Exists(text))
				{
					File.Copy(text, cfgFilePath, overwrite: true);
				}
			}
		}

		public void checkForFirst()
		{
			if (!saveState.Enabled)
			{
				saveState.Image = Resources.pressedButtonExtraThicc;
				saveState.Enabled = true;
			}
		}

		private void firstSet_Click(object sender, EventArgs e)
		{
			bool flag = File.Exists(cfgFilePath);
			string path = StringProcessing.StepUp(cfgFilePath) + "\\version.txt";
			bool flag2 = File.Exists(path);
			if (flag && flag2)
			{
				FormsFunctions.PlayButtonSound();
				mainform.state = "First";
				checkForFirst();
				firstSet.Image = Resources._1_acitve;
				secondSet.Image = Resources._2_not_active;
				thirdSet.Image = Resources._3_not_active;
				CopyCFgFile();
			}
		}

		private void secondSet_Click(object sender, EventArgs e)
		{
			if (File.Exists(cfgFilePath) && File.Exists(StringProcessing.StepUp(cfgFilePath) + "\\version.txt"))
			{
				FormsFunctions.PlayButtonSound();
				mainform.state = "Second";
				checkForFirst();
				firstSet.Image = Resources._1_not_active;
				secondSet.Image = Resources._2_active;
				thirdSet.Image = Resources._3_not_active;
				CopyCFgFile();
			}
		}

		private void thirdSet_Click(object sender, EventArgs e)
		{
			if (File.Exists(cfgFilePath) && File.Exists(StringProcessing.StepUp(cfgFilePath) + "\\version.txt"))
			{
				FormsFunctions.PlayButtonSound();
				mainform.state = "Third";
				checkForFirst();
				firstSet.Image = Resources._1_not_active;
				secondSet.Image = Resources._2_not_active;
				thirdSet.Image = Resources._3active;
				CopyCFgFile();
			}
		}

		private void english_CheckedChanged(object sender, EventArgs e)
		{
			if (!english.Checked)
			{
				mainform.webBrowser1.Navigate("http://klissancall.000webhostapp.com/engNews.html");
			}
		}

		private void russian_CheckedChanged(object sender, EventArgs e)
		{
			if (!russian.Checked)
			{
				mainform.webBrowser1.Navigate("http://klissancall.000webhostapp.com/engNews.html");
			}
		}

		private void pictureBox1_MouseDown_3(object sender, MouseEventArgs e)
		{
			checkHash.Image = Resources._2AutoD;
		}

		private void checkHash_MouseUp(object sender, MouseEventArgs e)
		{
			checkHash.Image = Resources._2AutoA;
			mainform.playForm = new play(executePath, mainform, turnOffLauncher: false);
			mainform.playForm.Show();
		}

		private void settings_Click(object sender, EventArgs e)
		{
		}

		private void russian_Click(object sender, EventArgs e)
		{
			russian.Checked = true;
			english.Checked = false;
			if (mainform != null)
			{
				mainform.setRu();
			}
			if (updateForm != null)
			{
				updateForm.setRu();
			}
			if (playForm != null)
			{
				playForm.setRu();
			}
			mainform.Lang = "ru";
			setRu();
		}

		private void english_Click(object sender, EventArgs e)
		{
			russian.Checked = false;
			english.Checked = true;
			if (mainform != null)
			{
				mainform.setEng();
			}
			if (updateForm != null)
			{
				updateForm.setEng();
			}
			if (playForm != null)
			{
				playForm.setEng();
			}
			mainform.Lang = "eng";
			setEng();
		}

		private void checkHashButton_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void checkHashButton_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void createBackupButton_MouseDown(object sender, MouseEventArgs e)
		{
			createBackupButton.Image = Resources.pressedButtonExtraThicc;
		}

		private void createBackupButton_MouseUp(object sender, MouseEventArgs e)
		{
			createBackupButton.Image = Resources.unpressedButtonExtraThicc;
			string sourcePath = StringProcessing.StepUp(mainform.pathToFile);
			string text = Application.StartupPath + "\\Backup";
			Directory.CreateDirectory(text);
			string[] array = null;
			try
			{
				array = File.ReadAllLines(cfgFilePath);
			}
			catch (Exception ex)
			{
				FormsFunctions.CallMessageBox(ex.ToString(), this);
			}
			if (array != null)
			{
				File.Copy(mainform.pathToFile, text + "\\Rangers.exe");
				ParseAndMakeBackup(sourcePath, text);
			}
			if (Directory.Exists(text))
			{
				restoreBackupMods.Enabled = true;
			}
		}

		private void restoreBackupMods_MouseUp(object sender, MouseEventArgs e)
		{
			restoreBackupMods.Image = Resources.unpressedButtonExtraThicc;
		}

		private void restoreBackupMods_MouseDown(object sender, MouseEventArgs e)
		{
			restoreBackupMods.Image = Resources.pressedButtonExtraThicc;
		}

		private void checkHashButton_Click(object sender, EventArgs e)
		{
		}

		private void label8_MouseUp(object sender, MouseEventArgs e)
		{
			saveState.Image = Resources.pressedButtonExtraThicc;
			if (File.Exists(cfgFilePath))
			{
				string destFileName = StringProcessing.StepUp(executePath) + "\\Mods\\ModCFG" + mainform.state + ".txt";
				File.Copy(cfgFilePath, destFileName, overwrite: true);
			}
			else
			{
				FormsFunctions.CallMessageBox("CFG Файл не найден.", this);
			}
		}

		private void saveSettings_Click(object sender, EventArgs e)
		{
		}

		private void saveState_MouseMove(object sender, MouseEventArgs e)
		{
		}

		private void closeWindow_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void fuckYou_Click(object sender, EventArgs e)
		{
			FormsFunctions.PlayButtonSound();
		}

		private void checkHash_Click(object sender, EventArgs e)
		{
		}

		private void settings_Leave(object sender, EventArgs e)
		{
		}

		private void saveState_Click(object sender, EventArgs e)
		{
		}

		private void restoreBackupMods_Click_1(object sender, EventArgs e)
		{
		}

		private void restoreBackupMods_MouseUp_1(object sender, MouseEventArgs e)
		{
			if (mainform.Lang == "eng")
			{
				restoreBackupMods.Image = Resources.resotreBackupEng;
			}
			if (mainform.Lang == "ru")
			{
				restoreBackupMods.Image = Resources.restoreBackupRu;
			}
			string text = defaultPath + "\\Backup\\Mods";
			string destinationDirectoryName = defaultPath + "\\Mods";
			string path = defaultPath + "\\Backup";
			bool flag = Directory.Exists(text);
			bool flag2 = Directory.Exists(path);
			if (flag && flag2)
			{
				try
				{
					FileSystem.CopyDirectory(text, destinationDirectoryName, UIOption.AllDialogs);
				}
				catch (OperationCanceledException)
				{
				}
			}
		}

		private void saveState_MouseUp(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
			if (mainform.Lang == "eng")
			{
				saveState.Image = Resources.createSettingsEng;
			}
			if (mainform.Lang == "ru")
			{
				saveState.Image = Resources.createSettingsRu;
			}
		}

		private void saveState_MouseDown(object sender, MouseEventArgs e)
		{
			if (mainform.Lang == "eng")
			{
				saveState.Image = Resources.pressedCreateSettingsEng;
			}
			if (mainform.Lang == "ru")
			{
				saveState.Image = Resources.pressedCreateSettingsRu;
			}
		}

		private void defaultState_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void defaultState_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void createBackupButton_MouseUp_1(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
			if (mainform.Lang == "eng")
			{
				createBackupButton.Image = Resources.createBackupEng;
			}
			if (mainform.Lang == "ru")
			{
				createBackupButton.Image = Resources.createModSetRu;
			}
			string text = StringProcessing.StepUp(cfgFilePath);
			string text2 = StringProcessing.StepUp(text) + "\\Backup";
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(StringProcessing.StepUp(text2));
			}
			text2 += "\\Mods";
			string[] array = null;
			ParseAndMakeBackup(text, text2);
			if (Directory.Exists(text2))
			{
				defaultSettings.Enabled = true;
			}
		}

		public void ParseAndMakeBackup(string sourcePath, string destinationPath)
		{
			if (Directory.Exists(destinationPath))
			{
				Directory.Delete(destinationPath, recursive: true);
			}
			if (Directory.Exists(sourcePath))
			{
				try
				{
					FileSystem.CopyDirectory(sourcePath, destinationPath, UIOption.AllDialogs);
				}
				catch (OperationCanceledException)
				{
				}
			}
		}

		private void createBackupButton_MouseDown_1(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
			int num = 1;
			if (mainform.Lang == "eng")
			{
				createBackupButton.Image = Resources.pressedCreateBackupEng;
			}
			if (mainform.Lang == "ru")
			{
				createBackupButton.Image = Resources.pressedCreateSettingsRu;
			}
		}

		private void restoreBackupMods_MouseDown_1(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
			if (mainform.Lang == "eng")
			{
				restoreBackupMods.Image = Resources.pressedResotreBackupEng;
			}
			if (mainform.Lang == "ru")
			{
				restoreBackupMods.Image = Resources.pressedRestoreBackupRu;
			}
		}

		private void createBackupButton_Click(object sender, EventArgs e)
		{
		}

		private void fuckYou_MouseDown(object sender, MouseEventArgs e)
		{
			fuckYou.Image = Resources._2SC_RepareD;
		}

		private void createBackupButton_MouseMove(object sender, MouseEventArgs e)
		{
		}

		private void fuckYou_MouseUp(object sender, MouseEventArgs e)
		{
			fuckYou.Image = Resources._2SC_RepareA;
			string s = "соси";
			if (!BoolConfirmation.OpenDialog(ref s))
			{
				return;
			}
			mainform.pathToFile = s;
			pathToFile.Text = mainform.pathToFile;
			if (Directory.Exists(StringProcessing.StepUp(s)) && BoolConfirmation.SetPath(ref s))
			{
				defaultSettings.Enabled = true;
				mainform.play.Enabled = true;
				mainform.play.Image = Resources._2ContinueA;
				mainform.checkUpdates.Enabled = true;
				mainform.checkUpdates.Image = Resources._2SettingsA;
				pathIsCorrect = true;
				mainform.isPathCorrect = true;
				if (mainform.Lang == "ru")
				{
					changeButtonImageRu();
				}
				else
				{
					changeButtonImageEng();
				}
				executePath = s;
				cfgFilePath = StringProcessing.StepUp(executePath) + "\\Mods\\ModCFG.txt";
				defaultPath = StringProcessing.StepUp(executePath);
				mainform.changeEnabledStatusButtons();
			}
		}

		private void defaultState_Click(object sender, EventArgs e)
		{
		}

		private void defaultSet_Click(object sender, EventArgs e)
		{
		}

		private void defaultSet_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void pictureBox1_MouseDown_4(object sender, MouseEventArgs e)
		{
			if (mainform.Lang == "eng")
			{
				defaultSettings.Image = Resources.pressedRestoreSettingsEng;
			}
			if (mainform.Lang == "ru")
			{
				defaultSettings.Image = Resources.pressedRestoreSettingsRu;
			}
		}

		private void test_MouseUp(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
			string text = "test_mouseUp (polzyas sluchaem hochu peredat 4to neketa gay";
			if (mainform.Lang == "eng")
			{
				defaultSettings.Image = Resources.restoreSettingsEng;
				text = "default ModCFG file has been successfully restored";
				if (Directory.Exists(defaultPath + "\\Mods"))
				{
					File.WriteAllText(cfgFilePath, Resources.ModCFGen);
					MessageBox.Show(text);
				}
			}
			if (mainform.Lang == "ru")
			{
				text = "ModCFG файл по-умолчанию успешно востановлен";
				defaultSettings.Image = Resources.restoreSettingsRu;
				if (Directory.Exists(defaultPath + "\\Mods"))
				{
					File.WriteAllText(cfgFilePath, Resources.ModCFGru);
					MessageBox.Show(text);
				}
			}
		}

		private void label5_Click(object sender, EventArgs e)
		{
		}

		private void replace_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void Ubercharge_Click(object sender, EventArgs e)
		{
			mainform.replaceCfg = !mainform.replaceCfg;
			changeSetStates(mainform.replaceCfg);
		}

		private void turnOffLauncher_Click(object sender, EventArgs e)
		{
			mainform.turnOffLauncher = !mainform.turnOffLauncher;
		}

		private void checkHash_MouseClick(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayButtonSound();
		}

		private void label7_Click(object sender, EventArgs e)
		{
		}

		private void reinstallBtn_MouseUp(object sender, MouseEventArgs e)
		{
			string message = "reinstallBtn";
			if (mainform.Lang == "ru")
			{
				message = "Выбрана чистая переустановка. ";
				reinstallBtn.Image = Resources.reinstallRu;
			}
			if (mainform.Lang == "eng")
			{
				message = "Clean reinstall was chosen. ";
				reinstallBtn.Image = Resources.reinstalEng;
			}
			mainform.callUpdate(mainform.pathToFile, message, reisntall: true, SR1HD: false);
		}

		private void reinstallBtn_MouseDown(object sender, MouseEventArgs e)
		{
			if (mainform.Lang == "eng")
			{
				reinstallBtn.Image = Resources.pressedReinstalEng;
			}
			if (mainform.Lang == "ru")
			{
				Image image2 = reinstallBtn.Image = (reinstallBtn.Image = Resources.pressedReinstallRu);
			}
		}

		private void fuckYou_MouseMove(object sender, MouseEventArgs e)
		{
		}

		private void label8_Click(object sender, EventArgs e)
		{
		}

		private void pathToFile_TextChanged(object sender, EventArgs e)
		{
		}

		private void reinstallBtn_Click(object sender, EventArgs e)
		{
		}

		private void saveState_MouseUp_1(object sender, MouseEventArgs e)
		{
			if (mainform.Lang == "eng")
			{
				saveState.Image = Resources.createSettingsEng;
			}
			if (mainform.Lang == "ru")
			{
				saveState.Image = Resources.createSettingsRu;
			}
			if (File.Exists(cfgFilePath))
			{
				string destFileName = StringProcessing.StepUp(executePath) + "\\Mods\\ModCFG" + mainform.state + ".txt";
				File.Copy(cfgFilePath, destFileName, overwrite: true);
				string text = "kekestan (Save_state_Mouseup)";
				if (mainform.Lang == "ru")
				{
					text = "CFG file created successfully";
				}
				if (mainform.Lang == "eng")
				{
					text = "CFG файл создан успешно";
				}
				MessageBox.Show(text);
			}
			else
			{
				string text2 = "kekestan (Save_state_Mouseup)";
				if (mainform.Lang == "ru")
				{
					text2 = "CFG hasn't been detected";
				}
				if (mainform.Lang == "eng")
				{
					text2 = "CFG Файл не найден.";
				}
				MessageBox.Show(text2);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settings));
            this.closeWindow = new System.Windows.Forms.PictureBox();
            this.minimizeWindow = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.firstSet = new System.Windows.Forms.PictureBox();
            this.secondSet = new System.Windows.Forms.PictureBox();
            this.thirdSet = new System.Windows.Forms.PictureBox();
            this.russian = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.english = new System.Windows.Forms.CheckBox();
            this.checkHash = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pathToFile = new System.Windows.Forms.TextBox();
            this.fuckYou = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.createBackupButton = new System.Windows.Forms.PictureBox();
            this.restoreBackupMods = new System.Windows.Forms.PictureBox();
            this.saveState = new System.Windows.Forms.PictureBox();
            this.defaultSettings = new System.Windows.Forms.PictureBox();
            this.Ubercharge = new System.Windows.Forms.CheckBox();
            this.turnOffLauncher = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.reinstallBtn = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.closeWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkHash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuckYou)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createBackupButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.restoreBackupMods)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defaultSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reinstallBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // closeWindow
            // 
            this.closeWindow.BackgroundImage = global::SRHDLauncher.Properties.Resources.uniTopBackground;
            this.closeWindow.Image = ((System.Drawing.Image)(resources.GetObject("closeWindow.Image")));
            this.closeWindow.Location = new System.Drawing.Point(857, 55);
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
            this.minimizeWindow.BackgroundImage = global::SRHDLauncher.Properties.Resources.uniTopBackground;
            this.minimizeWindow.Image = global::SRHDLauncher.Properties.Resources._2SubA;
            this.minimizeWindow.Location = new System.Drawing.Point(827, 55);
            this.minimizeWindow.Name = "minimizeWindow";
            this.minimizeWindow.Size = new System.Drawing.Size(24, 24);
            this.minimizeWindow.TabIndex = 15;
            this.minimizeWindow.TabStop = false;
            this.minimizeWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.minimizeWindow_MouseDown);
            this.minimizeWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.minimizeWindow_MouseUp);
            // 
            // label2
            // 
            this.label2.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Image = global::SRHDLauncher.Properties.Resources.background_universe_form;
            this.label2.Location = new System.Drawing.Point(96, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "Комплектация модов";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // firstSet
            // 
            this.firstSet.BackgroundImage = global::SRHDLauncher.Properties.Resources.background_universe_form;
            this.firstSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.firstSet.Image = global::SRHDLauncher.Properties.Resources._1_not_active;
            this.firstSet.Location = new System.Drawing.Point(96, 126);
            this.firstSet.Name = "firstSet";
            this.firstSet.Size = new System.Drawing.Size(27, 20);
            this.firstSet.TabIndex = 20;
            this.firstSet.TabStop = false;
            this.firstSet.Click += new System.EventHandler(this.firstSet_Click);
            this.firstSet.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown_2);
            this.firstSet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.firstSet_MouseUp);
            // 
            // secondSet
            // 
            this.secondSet.BackgroundImage = global::SRHDLauncher.Properties.Resources.background_universe_form;
            this.secondSet.Image = global::SRHDLauncher.Properties.Resources._2_not_active;
            this.secondSet.Location = new System.Drawing.Point(129, 126);
            this.secondSet.Name = "secondSet";
            this.secondSet.Size = new System.Drawing.Size(27, 20);
            this.secondSet.TabIndex = 21;
            this.secondSet.TabStop = false;
            this.secondSet.Click += new System.EventHandler(this.secondSet_Click);
            this.secondSet.MouseDown += new System.Windows.Forms.MouseEventHandler(this.secondSet_MouseDown);
            this.secondSet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.secondSet_MouseUp);
            // 
            // thirdSet
            // 
            this.thirdSet.BackgroundImage = global::SRHDLauncher.Properties.Resources.background_universe_form;
            this.thirdSet.Image = global::SRHDLauncher.Properties.Resources._3_not_active;
            this.thirdSet.Location = new System.Drawing.Point(162, 126);
            this.thirdSet.Name = "thirdSet";
            this.thirdSet.Size = new System.Drawing.Size(27, 20);
            this.thirdSet.TabIndex = 22;
            this.thirdSet.TabStop = false;
            this.thirdSet.Click += new System.EventHandler(this.thirdSet_Click);
            this.thirdSet.MouseDown += new System.Windows.Forms.MouseEventHandler(this.thirdSet_MouseDown);
            this.thirdSet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.thirdSet_MouseUp);
            // 
            // russian
            // 
            this.russian.AutoSize = true;
            this.russian.Location = new System.Drawing.Point(88, 238);
            this.russian.Name = "russian";
            this.russian.Size = new System.Drawing.Size(15, 14);
            this.russian.TabIndex = 23;
            this.russian.UseVisualStyleBackColor = true;
            this.russian.CheckedChanged += new System.EventHandler(this.russian_CheckedChanged);
            this.russian.Click += new System.EventHandler(this.russian_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(249, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "English";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(101, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 21);
            this.label4.TabIndex = 26;
            this.label4.Text = "Русский";
            this.label4.UseCompatibleTextRendering = true;
            // 
            // english
            // 
            this.english.AutoSize = true;
            this.english.Location = new System.Drawing.Point(235, 238);
            this.english.Name = "english";
            this.english.Size = new System.Drawing.Size(15, 14);
            this.english.TabIndex = 27;
            this.english.UseVisualStyleBackColor = true;
            this.english.CheckedChanged += new System.EventHandler(this.english_CheckedChanged);
            this.english.Click += new System.EventHandler(this.english_Click);
            // 
            // checkHash
            // 
            this.checkHash.BackgroundImage = global::SRHDLauncher.Properties.Resources.background_universe_form;
            this.checkHash.Image = global::SRHDLauncher.Properties.Resources._2AutoA;
            this.checkHash.Location = new System.Drawing.Point(195, 328);
            this.checkHash.Name = "checkHash";
            this.checkHash.Size = new System.Drawing.Size(43, 34);
            this.checkHash.TabIndex = 28;
            this.checkHash.TabStop = false;
            this.checkHash.Click += new System.EventHandler(this.checkHash_Click);
            this.checkHash.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkHash_MouseClick);
            this.checkHash.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown_3);
            this.checkHash.MouseUp += new System.Windows.Forms.MouseEventHandler(this.checkHash_MouseUp);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(141, 305);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(186, 20);
            this.label5.TabIndex = 29;
            this.label5.Text = "Сменить тип запуска";
            this.label5.UseCompatibleTextRendering = true;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // pathToFile
            // 
            this.pathToFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(201)))), ((int)(((byte)(210)))));
            this.pathToFile.Location = new System.Drawing.Point(469, 114);
            this.pathToFile.Name = "pathToFile";
            this.pathToFile.ReadOnly = true;
            this.pathToFile.Size = new System.Drawing.Size(382, 20);
            this.pathToFile.TabIndex = 30;
            this.pathToFile.TextChanged += new System.EventHandler(this.pathToFile_TextChanged);
            // 
            // fuckYou
            // 
            this.fuckYou.BackgroundImage = global::SRHDLauncher.Properties.Resources.background_universe_form;
            this.fuckYou.Image = global::SRHDLauncher.Properties.Resources._2SC_RepareA;
            this.fuckYou.Location = new System.Drawing.Point(469, 140);
            this.fuckYou.Name = "fuckYou";
            this.fuckYou.Size = new System.Drawing.Size(23, 20);
            this.fuckYou.TabIndex = 32;
            this.fuckYou.TabStop = false;
            this.fuckYou.Click += new System.EventHandler(this.fuckYou_Click);
            this.fuckYou.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fuckYou_MouseDown);
            this.fuckYou.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fuckYou_MouseMove);
            this.fuckYou.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fuckYou_MouseUp);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(492, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(192, 18);
            this.label7.TabIndex = 33;
            this.label7.Text = "Изменить";
            this.label7.UseCompatibleTextRendering = true;
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // createBackupButton
            // 
            this.createBackupButton.Enabled = false;
            this.createBackupButton.Image = ((System.Drawing.Image)(resources.GetObject("createBackupButton.Image")));
            this.createBackupButton.Location = new System.Drawing.Point(484, 170);
            this.createBackupButton.Name = "createBackupButton";
            this.createBackupButton.Size = new System.Drawing.Size(247, 29);
            this.createBackupButton.TabIndex = 41;
            this.createBackupButton.TabStop = false;
            this.createBackupButton.Click += new System.EventHandler(this.createBackupButton_Click);
            this.createBackupButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.createBackupButton_MouseDown_1);
            this.createBackupButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.createBackupButton_MouseMove);
            this.createBackupButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.createBackupButton_MouseUp_1);
            // 
            // restoreBackupMods
            // 
            this.restoreBackupMods.Enabled = false;
            this.restoreBackupMods.Image = ((System.Drawing.Image)(resources.GetObject("restoreBackupMods.Image")));
            this.restoreBackupMods.Location = new System.Drawing.Point(484, 205);
            this.restoreBackupMods.Name = "restoreBackupMods";
            this.restoreBackupMods.Size = new System.Drawing.Size(247, 28);
            this.restoreBackupMods.TabIndex = 42;
            this.restoreBackupMods.TabStop = false;
            this.restoreBackupMods.Click += new System.EventHandler(this.restoreBackupMods_Click_1);
            this.restoreBackupMods.MouseDown += new System.Windows.Forms.MouseEventHandler(this.restoreBackupMods_MouseDown_1);
            this.restoreBackupMods.MouseUp += new System.Windows.Forms.MouseEventHandler(this.restoreBackupMods_MouseUp_1);
            // 
            // saveState
            // 
            this.saveState.Enabled = false;
            this.saveState.Image = ((System.Drawing.Image)(resources.GetObject("saveState.Image")));
            this.saveState.Location = new System.Drawing.Point(81, 170);
            this.saveState.Name = "saveState";
            this.saveState.Size = new System.Drawing.Size(246, 29);
            this.saveState.TabIndex = 43;
            this.saveState.TabStop = false;
            this.saveState.Click += new System.EventHandler(this.saveState_Click);
            this.saveState.MouseDown += new System.Windows.Forms.MouseEventHandler(this.saveState_MouseDown);
            this.saveState.MouseUp += new System.Windows.Forms.MouseEventHandler(this.saveState_MouseUp_1);
            // 
            // defaultSettings
            // 
            this.defaultSettings.Enabled = false;
            this.defaultSettings.Image = ((System.Drawing.Image)(resources.GetObject("defaultSettings.Image")));
            this.defaultSettings.Location = new System.Drawing.Point(81, 205);
            this.defaultSettings.Name = "defaultSettings";
            this.defaultSettings.Size = new System.Drawing.Size(247, 28);
            this.defaultSettings.TabIndex = 44;
            this.defaultSettings.TabStop = false;
            this.defaultSettings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown_4);
            this.defaultSettings.MouseUp += new System.Windows.Forms.MouseEventHandler(this.test_MouseUp);
            // 
            // Ubercharge
            // 
            this.Ubercharge.AutoSize = true;
            this.Ubercharge.Location = new System.Drawing.Point(195, 129);
            this.Ubercharge.Name = "Ubercharge";
            this.Ubercharge.Size = new System.Drawing.Size(15, 14);
            this.Ubercharge.TabIndex = 45;
            this.toolTip2.SetToolTip(this.Ubercharge, "Активно - будет означать что при выборе разных комплектаций основной ModCFG файл " +
        "будет заменяться уже существующими пресетами");
            this.Ubercharge.UseVisualStyleBackColor = true;
            this.Ubercharge.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.Ubercharge.Click += new System.EventHandler(this.Ubercharge_Click);
            // 
            // turnOffLauncher
            // 
            this.turnOffLauncher.AutoSize = true;
            this.turnOffLauncher.Location = new System.Drawing.Point(463, 307);
            this.turnOffLauncher.Name = "turnOffLauncher";
            this.turnOffLauncher.Size = new System.Drawing.Size(15, 14);
            this.turnOffLauncher.TabIndex = 46;
            this.turnOffLauncher.UseVisualStyleBackColor = true;
            this.turnOffLauncher.CheckedChanged += new System.EventHandler(this.replace_CheckedChanged);
            this.turnOffLauncher.Click += new System.EventHandler(this.turnOffLauncher_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(209, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 20);
            this.label3.TabIndex = 47;
            this.label3.Text = "Активно";
            this.toolTip1.SetToolTip(this.label3, "Активно - будет означать что при выборе разных комплектаций основной ModCFG файл " +
        "будет заменяться уже существующими пресетами");
            this.label3.UseCompatibleTextRendering = true;
            // 
            // label8
            // 
            this.label8.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Image = global::SRHDLauncher.Properties.Resources.background_universe_form;
            this.label8.Location = new System.Drawing.Point(484, 305);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(309, 20);
            this.label8.TabIndex = 49;
            this.label8.Text = "Закрывать лаунчер при запуске игры";
            this.label8.UseCompatibleTextRendering = true;
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // reinstallBtn
            // 
            this.reinstallBtn.BackColor = System.Drawing.Color.Transparent;
            this.reinstallBtn.Enabled = false;
            this.reinstallBtn.Image = ((System.Drawing.Image)(resources.GetObject("reinstallBtn.Image")));
            this.reinstallBtn.Location = new System.Drawing.Point(484, 239);
            this.reinstallBtn.Name = "reinstallBtn";
            this.reinstallBtn.Size = new System.Drawing.Size(247, 32);
            this.reinstallBtn.TabIndex = 50;
            this.reinstallBtn.TabStop = false;
            this.reinstallBtn.Click += new System.EventHandler(this.reinstallBtn_Click);
            this.reinstallBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.reinstallBtn_MouseDown);
            this.reinstallBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.reinstallBtn_MouseUp);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::SRHDLauncher.Properties.Resources.background_universe_form;
            this.pictureBox2.Image = global::SRHDLauncher.Properties.Resources._2SC_RepareA;
            this.pictureBox2.Location = new System.Drawing.Point(469, 423);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(23, 20);
            this.pictureBox2.TabIndex = 51;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseUp);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(491, 424);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(253, 18);
            this.label6.TabIndex = 52;
            this.label6.Text = "Cбросить настройки лаунчера";
            this.label6.UseCompatibleTextRendering = true;
            // 
            // settings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::SRHDLauncher.Properties.Resources.additionalForm;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(906, 503);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.reinstallBtn);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.turnOffLauncher);
            this.Controls.Add(this.Ubercharge);
            this.Controls.Add(this.defaultSettings);
            this.Controls.Add(this.saveState);
            this.Controls.Add(this.restoreBackupMods);
            this.Controls.Add(this.createBackupButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.fuckYou);
            this.Controls.Add(this.pathToFile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkHash);
            this.Controls.Add(this.english);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.russian);
            this.Controls.Add(this.thirdSet);
            this.Controls.Add(this.secondSet);
            this.Controls.Add(this.firstSet);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.closeWindow);
            this.Controls.Add(this.minimizeWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "settings";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Load += new System.EventHandler(this.settings_Load);
            this.Shown += new System.EventHandler(this.update_Shown);
            this.Click += new System.EventHandler(this.settings_Click);
            this.Leave += new System.EventHandler(this.settings_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.settings_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.closeWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thirdSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkHash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuckYou)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createBackupButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.restoreBackupMods)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defaultSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reinstallBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
		{
			pictureBox2.Image = Resources._2SC_RepareD;
		}

		private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
		{
			pictureBox2.Image = Resources._2SC_RepareA;
			string s =  Path.GetTempPath() + "SRHDLauncherSettings.txt";
			if (File.Exists(s))
			{
				File.Delete(s);
				mainform.RecreateConfig(s);
				Application.Restart();
			}
		}
	}
}
