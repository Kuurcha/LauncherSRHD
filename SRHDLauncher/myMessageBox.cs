using SRHDLauncher.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SRHDLauncher
{
	public class myMessageBox : Form
	{
		private string textBoxMesage;

		private mainform mainform;

		private update form2;

		private play form3;

		private settings form4;

		private const uint WM_SYSCOMMAND = 274u;

		private const uint DOMOVE = 61458u;

		private const uint DOSIZE = 61448u;

		private static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

		public static Font calcFont;

		private IContainer components = null;

		private PictureBox closeWindow;

		private PictureBox minimizeWindow;

		private Label message;

		private PictureBox okBtn;

		private PictureBox pictureBox1;

		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		[DllImport("user32", CharSet = CharSet.Auto)]
		internal static extern bool PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);

		[DllImport("user32", CharSet = CharSet.Auto)]
		internal static extern bool ReleaseCapture();

		public string fixTheMessage(string message)
		{
			return "\r\n\r\n" + message;
		}

		public myMessageBox(string message, mainform form1)
		{
			base.TopMost = true;
			form1.changeEnabledStatusButtons();
			mainform = form1;
			InitializeComponent();
			textBoxMesage = message;
			this.message.Text = fixTheMessage(message);
			base.MouseDown += myMessageBox_MouseDown;
			base.MouseDown += message_MouseDown;
			base.Closing += OnClosing;
		}

		private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			if (mainform != null)
			{
				mainform.changeEnabledStatusButtons();
			}
		}

		public void RepeatUntilBusy(WebClient client)
		{
			while (client.IsBusy)
			{
				Application.DoEvents();
			}
		}

		public myMessageBox(string message, update form2)
		{
			textBoxMesage = message;
			this.form2 = form2;
			InitializeComponent();
			base.Size = BackgroundImage.Size;
			this.message.Text = message;
			base.MouseDown += myMessageBox_MouseDown;
			base.MouseDown += message_MouseDown;
		}

		public myMessageBox(string message, play form3)
		{
			textBoxMesage = message;
			this.form3 = form3;
			InitializeComponent();
			base.Size = BackgroundImage.Size;
			this.message.Text = message;
			base.MouseDown += myMessageBox_MouseDown;
			base.MouseDown += message_MouseDown;
		}

		public myMessageBox(string message, settings form4)
		{
			textBoxMesage = message;
			this.form4 = form4;
			InitializeComponent();
			base.Size = BackgroundImage.Size;
			this.message.Text = message;
			base.MouseDown += myMessageBox_MouseDown;
			base.MouseDown += message_MouseDown;
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
			if (mainform != null)
			{
				mainform.answer = true;
				mainform.callUpdate(mainform.executabePath, textBoxMesage, reisntall: false, SR1HD: false);
			}
			if (form2 != null)
			{
				form2.answer = true;
			}
			if (form3 != null)
			{
				form3.answer = true;
			}
		}

		private void okBtn_Click(object sender, EventArgs e)
		{
			FormsFunctions.PlayButtonSound();
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			pictureBox1.Image = Resources._2CloseD;
		}

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			FormsFunctions.PlayInfoButtonSound();
			pictureBox1.Image = Resources._2CloseD;
			if (mainform != null)
			{
				mainform.answer = false;
			}
			if (form2 != null)
			{
				form2.updateRequired = true;
			}
			if (form3 != null)
			{
				form3.answer = false;
			}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(myMessageBox));
            this.closeWindow = new System.Windows.Forms.PictureBox();
            this.minimizeWindow = new System.Windows.Forms.PictureBox();
            this.message = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.closeWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.okBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // closeWindow
            // 
            this.closeWindow.BackgroundImage = global::SRHDLauncher.Properties.Resources.чорнота;
            this.closeWindow.Image = global::SRHDLauncher.Properties.Resources._2CrossA;
            this.closeWindow.Location = new System.Drawing.Point(750, -69);
            this.closeWindow.Name = "closeWindow";
            this.closeWindow.Size = new System.Drawing.Size(10, 10);
            this.closeWindow.TabIndex = 19;
            this.closeWindow.TabStop = false;
            // 
            // minimizeWindow
            // 
            this.minimizeWindow.BackgroundImage = global::SRHDLauncher.Properties.Resources.чорнота;
            this.minimizeWindow.Image = global::SRHDLauncher.Properties.Resources._2SubA;
            this.minimizeWindow.Location = new System.Drawing.Point(720, -69);
            this.minimizeWindow.Name = "minimizeWindow";
            this.minimizeWindow.Size = new System.Drawing.Size(10, 10);
            this.minimizeWindow.TabIndex = 18;
            this.minimizeWindow.TabStop = false;
            // 
            // message
            // 
            this.message.BackColor = System.Drawing.Color.Transparent;
            this.message.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.message.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.message.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.message.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.message.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.message.Location = new System.Drawing.Point(58, 48);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(180, 106);
            this.message.TabIndex = 17;
            this.message.Text = "\r\n\r\n\r\nОбнаружена обнаружено обнаружено обнаружен ядерный удар";
            this.message.UseCompatibleTextRendering = true;
            this.message.Click += new System.EventHandler(this.label1_Click);
            this.message.MouseDown += new System.Windows.Forms.MouseEventHandler(this.message_MouseDown);
            // 
            // okBtn
            // 
            this.okBtn.BackgroundImage = global::SRHDLauncher.Properties.Resources.uniTopBackground;
            this.okBtn.Image = global::SRHDLauncher.Properties.Resources._2OkA;
            this.okBtn.Location = new System.Drawing.Point(80, 167);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(48, 23);
            this.okBtn.TabIndex = 20;
            this.okBtn.TabStop = false;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            this.okBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.degenerateChoice_MouseDown);
            this.okBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.degenerateChoice_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::SRHDLauncher.Properties.Resources.uniTopBackground;
            this.pictureBox1.Image = global::SRHDLauncher.Properties.Resources._2CloseA;
            this.pictureBox1.Location = new System.Drawing.Point(157, 167);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(49, 21);
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // myMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackgroundImage = global::SRHDLauncher.Properties.Resources.messageBox;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(282, 205);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.closeWindow);
            this.Controls.Add(this.minimizeWindow);
            this.Controls.Add(this.message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "myMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Load += new System.EventHandler(this.myMessageBox_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.myMessageBox_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.closeWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.okBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
	}
}
