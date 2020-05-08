using SRHDLauncher.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public class play : Form
	{
		private bool degenerateClod = false;

		private bool turnOffLauncher = false;

		private string executePath;

		private mainform mainform;

		private const uint WM_SYSCOMMAND = 274u;

		private const uint DOMOVE = 61458u;

		private const uint DOSIZE = 61448u;

		public static string[] Ru = new string[4]
		{
			"Рекомеднуемые настройки",
			"Произвольные настройки",
			"Помните, что только рекомендуемые настройки полностью поддерживаются авторами мода. Играть с произвольными настройками на свой риск",
			"Запомнить выбор при запуске"
		};

		public static string[] Eng = new string[4]
		{
			"Recommended settings",
			"Custom settings",
			"Remember that only reccomended settings are fully supported by the creators of the modpack. Custom settings are always at your own risk..",
			"Save the launch preferences "
		};

		private static string message = null;

		private static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

		public static Font calcFont;

		private IContainer components = null;

		private PictureBox degenerateChoice;

		private Label label2;

		private Label label3;

		private PictureBox pictureBox1;

		private PictureBox minimizeWindow;

		private PictureBox closeWindow;

		private Label REMEMBERME;

		private CheckBox recomended;

		private CheckBox custom;

		private PictureBox pictureBox2;

		private PictureBox pictureBox3;

		public bool answer
		{
			get;
			set;
		}

		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);

		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern bool ReleaseCapture();

		public void setRu()
		{
			label2.Text = Ru[0];
			label3.Text = Ru[1];
			message = Ru[2];
			REMEMBERME.Text = Ru[3];
		}

		public void setEng()
		{
			label2.Text = Eng[0];
			label3.Text = Eng[1];
			message = Eng[2];
			REMEMBERME.Text = Eng[3];
		}

		public play(string executePath, mainform currentForm, bool turnOffLauncher)
		{
			this.turnOffLauncher = turnOffLauncher;
			this.executePath = executePath;
			mainform = currentForm;
			InitializeComponent();
			recomended.Checked = mainform.firstRun;
			custom.Checked = !mainform.firstRun;
			if (mainform.Lang == "ru")
			{
				setRu();
			}
			if (mainform.Lang == "eng")
			{
				setEng();
			}
			mainform.changeEnabledStatusButtons();
			base.MouseDown += settings_MouseDown;
			base.MouseDown += pictureBox1_MouseDown;
			base.MouseDown += pictureBox1_MouseDown;
			base.Closing += OnClosing;
		}

		private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			if (mainform != null)
			{
				mainform.changeEnabledStatusButtons();
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
		}

		private void degenerateChoice_MouseDown(object sender, MouseEventArgs e)
		{
			if (!degenerateClod)
			{
				try
				{
					string text = StringProcessing.StepUp(mainform.pathToFile) + "\\Mods";
					if (File.Exists(text))
					{
						text += "\\ModCFG.txt";
						if (mainform.Lang == "eng")
						{
							mainform.CopyResource("SRHDLauncher.Resources.ModCFGen.txt", text);
						}
						if (mainform.Lang == "ru")
						{
							mainform.CopyResource("SRHDLauncher.Resources.ModCFGru.txt", text);
						}
					}
					Process.Start(mainform.pathToFile);
					if (turnOffLauncher)
					{
						mainform.Close();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "");
				}
				degenerateChoice.Image = Resources._2ContinueD;
			}
			else
			{
				try
				{
					Process.Start(mainform.pathToFile);
					if (turnOffLauncher)
					{
						mainform.Close();
					}
				}
				catch (Exception ex2)
				{
					MessageBox.Show(ex2.ToString(), "");
				}
			}
			Close();
			mainform.playForm = null;
		}

		private void goodChoice_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void degenerateChoice_MouseUp(object sender, MouseEventArgs e)
		{
			degenerateChoice.Image = Resources._2ContinueA;
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

		private void pictureBox1_Click_1(object sender, EventArgs e)
		{
			if (mainform.firstRun)
			{
				pictureBox1.Image = Resources._2OkA;
				mainform.firstRun = false;
			}
			else
			{
				pictureBox1.Image = Resources._2OkEmpty;
				mainform.firstRun = true;
			}
		}

		private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayLeaveButtonSound();
			closeWindow.Image = Resources._2CrossD;
		}

		private void closeWindow_MouseUp(object sender, MouseEventArgs e)
		{
			closeWindow.Image = Resources._2CrossA;
			Close();
			mainform.playForm = null;
		}

		private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
		{
			minimizeWindow.Image = Resources._2SubD;
		}

		private void minimizeWindow_Click(object sender, EventArgs e)
		{
			minimizeWindow.Image = Resources._2SubA;
			base.WindowState = FormWindowState.Minimized;
		}

		private void play_Shown(object sender, EventArgs e)
		{
			Activate();
			base.TopMost = true;
		}

		private void label2_Click(object sender, EventArgs e)
		{
		}

		private void closeWindow_Click(object sender, EventArgs e)
		{
		}

		private void recomended_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void custom_Click(object sender, EventArgs e)
		{
			recomended.Checked = !custom.Checked;
			degenerateClod = true;
			if (custom.Checked)
			{
				MessageBox.Show(message, "!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void recomended_Click(object sender, EventArgs e)
		{
			custom.Checked = !custom.Checked;
			degenerateClod = false;
		}

		private void REMEMBERME_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
		{
			pictureBox2.Image = Resources._2CrossA;
			Close();
			mainform.playForm = null;
			FormsFunctions.PlayInfoButtonSound();
		}

		private void pictureBox2_MouseDown_1(object sender, MouseEventArgs e)
		{
			pictureBox2.Image = Resources._2CrossD;
		}

		private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
		{
			pictureBox3.Image = Resources._2SubA;
			base.WindowState = FormWindowState.Minimized;
			FormsFunctions.PlayInfoButtonSound();
		}

		private void pictureBox3_MouseDown_1(object sender, MouseEventArgs e)
		{
			pictureBox3.Image = Resources._2SubD;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SRHDLauncher.play));
			closeWindow = new System.Windows.Forms.PictureBox();
			minimizeWindow = new System.Windows.Forms.PictureBox();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			degenerateChoice = new System.Windows.Forms.PictureBox();
			label3 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			REMEMBERME = new System.Windows.Forms.Label();
			recomended = new System.Windows.Forms.CheckBox();
			custom = new System.Windows.Forms.CheckBox();
			pictureBox2 = new System.Windows.Forms.PictureBox();
			pictureBox3 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)closeWindow).BeginInit();
			((System.ComponentModel.ISupportInitialize)minimizeWindow).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			((System.ComponentModel.ISupportInitialize)degenerateChoice).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
			SuspendLayout();
			closeWindow.BackColor = System.Drawing.Color.Transparent;
			closeWindow.Image = (System.Drawing.Image)resources.GetObject("closeWindow.Image");
			closeWindow.Location = new System.Drawing.Point(857, 39);
			closeWindow.Name = "closeWindow";
			closeWindow.Size = new System.Drawing.Size(24, 24);
			closeWindow.TabIndex = 16;
			closeWindow.TabStop = false;
			closeWindow.Click += new System.EventHandler(closeWindow_Click);
			closeWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox3_MouseDown);
			closeWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(closeWindow_MouseUp);
			minimizeWindow.BackgroundImage = SRHDLauncher.Properties.Resources.uniTopBackground;
			minimizeWindow.Image = SRHDLauncher.Properties.Resources._2SubA;
			minimizeWindow.Location = new System.Drawing.Point(827, 39);
			minimizeWindow.Name = "minimizeWindow";
			minimizeWindow.Size = new System.Drawing.Size(24, 24);
			minimizeWindow.TabIndex = 15;
			minimizeWindow.TabStop = false;
			minimizeWindow.Click += new System.EventHandler(minimizeWindow_Click);
			minimizeWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox2_MouseDown);
			pictureBox1.BackColor = System.Drawing.Color.Transparent;
			pictureBox1.Image = SRHDLauncher.Properties.Resources._2OkEmpty;
			pictureBox1.Location = new System.Drawing.Point(91, 145);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(50, 27);
			pictureBox1.TabIndex = 14;
			pictureBox1.TabStop = false;
			pictureBox1.Click += new System.EventHandler(pictureBox1_Click_1);
			degenerateChoice.BackColor = System.Drawing.Color.Transparent;
			degenerateChoice.Image = SRHDLauncher.Properties.Resources._2ContinueA;
			degenerateChoice.Location = new System.Drawing.Point(217, 102);
			degenerateChoice.Name = "degenerateChoice";
			degenerateChoice.Size = new System.Drawing.Size(52, 54);
			degenerateChoice.TabIndex = 10;
			degenerateChoice.TabStop = false;
			degenerateChoice.Click += new System.EventHandler(degenerateChoice_Click);
			degenerateChoice.MouseDown += new System.Windows.Forms.MouseEventHandler(degenerateChoice_MouseDown);
			degenerateChoice.MouseUp += new System.Windows.Forms.MouseEventHandler(degenerateChoice_MouseUp);
			label3.BackColor = System.Drawing.Color.Transparent;
			label3.Cursor = System.Windows.Forms.Cursors.AppStarting;
			label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 204);
			label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			label3.Location = new System.Drawing.Point(6, 45);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(196, 20);
			label3.TabIndex = 13;
			label3.Text = "Произвольные настройки";
			label3.UseCompatibleTextRendering = true;
			label2.BackColor = System.Drawing.Color.Transparent;
			label2.Cursor = System.Windows.Forms.Cursors.AppStarting;
			label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 204);
			label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			label2.Location = new System.Drawing.Point(6, 68);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(188, 15);
			label2.TabIndex = 12;
			label2.Text = "Рекомендуемые настройки";
			label2.UseCompatibleTextRendering = true;
			label2.Click += new System.EventHandler(label2_Click);
			REMEMBERME.BackColor = System.Drawing.Color.Transparent;
			REMEMBERME.Cursor = System.Windows.Forms.Cursors.AppStarting;
			REMEMBERME.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			REMEMBERME.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 204);
			REMEMBERME.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			REMEMBERME.Location = new System.Drawing.Point(12, 118);
			REMEMBERME.Name = "REMEMBERME";
			REMEMBERME.Size = new System.Drawing.Size(203, 24);
			REMEMBERME.TabIndex = 17;
			REMEMBERME.Text = "Запомнить выбор";
			REMEMBERME.UseCompatibleTextRendering = true;
			REMEMBERME.Click += new System.EventHandler(REMEMBERME_Click);
			recomended.AutoSize = true;
			recomended.Location = new System.Drawing.Point(200, 70);
			recomended.Name = "recomended";
			recomended.Size = new System.Drawing.Size(15, 14);
			recomended.TabIndex = 18;
			recomended.UseVisualStyleBackColor = true;
			recomended.CheckedChanged += new System.EventHandler(recomended_CheckedChanged);
			recomended.Click += new System.EventHandler(recomended_Click);
			custom.AutoSize = true;
			custom.Location = new System.Drawing.Point(200, 47);
			custom.Name = "custom";
			custom.Size = new System.Drawing.Size(15, 14);
			custom.TabIndex = 19;
			custom.UseVisualStyleBackColor = true;
			custom.Click += new System.EventHandler(custom_Click);
			pictureBox2.BackColor = System.Drawing.Color.Transparent;
			pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
			pictureBox2.Location = new System.Drawing.Point(234, 13);
			pictureBox2.Name = "pictureBox2";
			pictureBox2.Size = new System.Drawing.Size(24, 24);
			pictureBox2.TabIndex = 21;
			pictureBox2.TabStop = false;
			pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox2_MouseDown_1);
			pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(pictureBox2_MouseUp);
			pictureBox3.BackColor = System.Drawing.Color.Transparent;
			pictureBox3.Image = SRHDLauncher.Properties.Resources._2SubA;
			pictureBox3.Location = new System.Drawing.Point(204, 13);
			pictureBox3.Name = "pictureBox3";
			pictureBox3.Size = new System.Drawing.Size(24, 24);
			pictureBox3.TabIndex = 20;
			pictureBox3.TabStop = false;
			pictureBox3.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox3_MouseDown_1);
			pictureBox3.MouseUp += new System.Windows.Forms.MouseEventHandler(pictureBox3_MouseUp);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			BackgroundImage = SRHDLauncher.Properties.Resources.messageBox;
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			base.ClientSize = new System.Drawing.Size(282, 210);
			base.Controls.Add(pictureBox2);
			base.Controls.Add(pictureBox3);
			base.Controls.Add(custom);
			base.Controls.Add(recomended);
			base.Controls.Add(REMEMBERME);
			base.Controls.Add(closeWindow);
			base.Controls.Add(minimizeWindow);
			base.Controls.Add(pictureBox1);
			base.Controls.Add(degenerateChoice);
			base.Controls.Add(label3);
			base.Controls.Add(label2);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "play";
			Text = "settings";
			base.TransparencyKey = System.Drawing.Color.FromArgb(0, 192, 0);
			base.Load += new System.EventHandler(settings_Load);
			base.Shown += new System.EventHandler(play_Shown);
			base.MouseDown += new System.Windows.Forms.MouseEventHandler(settings_MouseDown);
			((System.ComponentModel.ISupportInitialize)closeWindow).EndInit();
			((System.ComponentModel.ISupportInitialize)minimizeWindow).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			((System.ComponentModel.ISupportInitialize)degenerateChoice).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
