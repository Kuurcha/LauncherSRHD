using SRHDLauncher.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public class langChoose : Form
	{
		private string textBoxMesage;

		private mainform mainform;

		private const uint WM_SYSCOMMAND = 274u;

		private const uint DOMOVE = 61458u;

		private const uint DOSIZE = 61448u;

		private IContainer components = null;

		private PictureBox closeWindow;

		private PictureBox minimizeWindow;

		private Label message;

		private PictureBox okBtn;

		private ComboBox comboBox1;

		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);

		[DllImport("user32", CharSet = CharSet.Auto)]
		public static extern bool ReleaseCapture();

		public langChoose(string message, mainform form1)
		{
			mainform = form1;
			InitializeComponent();
			base.MouseDown += myMessageBox_MouseDown;
			base.MouseDown += message_MouseDown;
			base.Closing += OnClosing;
			Show();
		}

		private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			if (comboBox1.Text == "Русский")
			{
				mainform.Lang = "ru";
				mainform.setRu();
			}
			if (comboBox1.Text == "English")
			{
				mainform.Lang = "eng";
				mainform.setEng();
			}
			mainform.Show();
		}

		public void RepeatUntilBusy(WebClient client)
		{
			while (client.IsBusy)
			{
				Application.DoEvents();
			}
		}

		private void myMessageBox_Load(object sender, EventArgs e)
		{
		}

		private void minimize_Click(object sender, EventArgs e)
		{
		}

		private void minimize_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void minimize_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void close_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void close_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void close_Click(object sender, EventArgs e)
		{
		}

		private void label1_Click(object sender, EventArgs e)
		{
		}

		private void degenerateChoice_MouseDown(object sender, MouseEventArgs e)
		{
			okBtn.Image = Resources._2OkD;
		}

		private void degenerateChoice_MouseUp(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayInfoButtonSound();
			okBtn.Image = Resources._2OkA;
			Close();
			mainform.BackgroundImage = Resources.BGGreen;
			mainform.changeVisibleState();
			mainform.mainMenuConstruct();
			mainform.initialise();
			if (comboBox1.Text == "English")
			{
				mainform.setEng();
			}
			if (comboBox1.Text == "Русский")
			{
				mainform.setRu();
			}
		}

		private void okBtn_Click(object sender, EventArgs e)
		{
			FormsFunctions.PlayButtonSound();
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayInfoButtonSound();
			Close();
		}

		private void myMessageBox_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			PostMessage(base.Handle, 274u, 61458u, 0u);
		}

		private void message_MouseDown(object sender, MouseEventArgs e)
		{
			ReleaseCapture();
			PostMessage(base.Handle, 274u, 61458u, 0u);
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			FormsFunctions.PlayButtonSound();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SRHDLauncher.langChoose));
			closeWindow = new System.Windows.Forms.PictureBox();
			minimizeWindow = new System.Windows.Forms.PictureBox();
			message = new System.Windows.Forms.Label();
			okBtn = new System.Windows.Forms.PictureBox();
			comboBox1 = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)closeWindow).BeginInit();
			((System.ComponentModel.ISupportInitialize)minimizeWindow).BeginInit();
			((System.ComponentModel.ISupportInitialize)okBtn).BeginInit();
			SuspendLayout();
			closeWindow.BackgroundImage = SRHDLauncher.Properties.Resources.чорнота;
			closeWindow.Image = SRHDLauncher.Properties.Resources._2CrossA;
			closeWindow.Location = new System.Drawing.Point(750, -69);
			closeWindow.Name = "closeWindow";
			closeWindow.Size = new System.Drawing.Size(10, 10);
			closeWindow.TabIndex = 19;
			closeWindow.TabStop = false;
			minimizeWindow.BackgroundImage = SRHDLauncher.Properties.Resources.чорнота;
			minimizeWindow.Image = SRHDLauncher.Properties.Resources._2SubA;
			minimizeWindow.Location = new System.Drawing.Point(720, -69);
			minimizeWindow.Name = "minimizeWindow";
			minimizeWindow.Size = new System.Drawing.Size(10, 10);
			minimizeWindow.TabIndex = 18;
			minimizeWindow.TabStop = false;
			message.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			message.Cursor = System.Windows.Forms.Cursors.AppStarting;
			message.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			message.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 204);
			message.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			message.Image = SRHDLauncher.Properties.Resources.messageBoxBackground;
			message.Location = new System.Drawing.Point(63, 64);
			message.Name = "message";
			message.Size = new System.Drawing.Size(157, 23);
			message.TabIndex = 17;
			message.Text = "Choose your language";
			message.UseCompatibleTextRendering = true;
			message.Click += new System.EventHandler(label1_Click);
			message.MouseDown += new System.Windows.Forms.MouseEventHandler(message_MouseDown);
			okBtn.BackgroundImage = SRHDLauncher.Properties.Resources.uniTopBackground;
			okBtn.Image = SRHDLauncher.Properties.Resources._2OkA;
			okBtn.Location = new System.Drawing.Point(113, 167);
			okBtn.Name = "okBtn";
			okBtn.Size = new System.Drawing.Size(52, 26);
			okBtn.TabIndex = 20;
			okBtn.TabStop = false;
			okBtn.Click += new System.EventHandler(okBtn_Click);
			okBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(degenerateChoice_MouseDown);
			okBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(degenerateChoice_MouseUp);
			comboBox1.FormattingEnabled = true;
			comboBox1.Items.AddRange(new object[2]
			{
				"Русский",
				"English"
			});
			comboBox1.Location = new System.Drawing.Point(81, 90);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new System.Drawing.Size(121, 21);
			comboBox1.TabIndex = 21;
			comboBox1.Text = "Русский";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			BackgroundImage = SRHDLauncher.Properties.Resources.messageBox;
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			base.ClientSize = new System.Drawing.Size(282, 205);
			base.Controls.Add(comboBox1);
			base.Controls.Add(okBtn);
			base.Controls.Add(closeWindow);
			base.Controls.Add(minimizeWindow);
			base.Controls.Add(message);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "langChoose";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			base.TransparencyKey = System.Drawing.Color.FromArgb(0, 192, 0);
			base.Load += new System.EventHandler(myMessageBox_Load);
			base.MouseDown += new System.Windows.Forms.MouseEventHandler(myMessageBox_MouseDown);
			((System.ComponentModel.ISupportInitialize)closeWindow).EndInit();
			((System.ComponentModel.ISupportInitialize)minimizeWindow).EndInit();
			((System.ComponentModel.ISupportInitialize)okBtn).EndInit();
			ResumeLayout(false);
		}
	}
}
