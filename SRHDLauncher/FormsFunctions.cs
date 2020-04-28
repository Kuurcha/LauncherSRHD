using SRHDLauncher.Properties;
using System.Media;

namespace SRHDLauncher
{
	public static class FormsFunctions
	{
		public static myMessageBox CallMessageBox(string s, mainform form1)
		{
			myMessageBox myMessageBox = new myMessageBox(s, form1);
			myMessageBox.Show();
			return myMessageBox;
		}

		public static myMessageBox CallMessageBox(string s, update form2)
		{
			myMessageBox myMessageBox = new myMessageBox(s, form2);
			myMessageBox.Show();
			return myMessageBox;
		}

		public static myMessageBox CallMessageBox(string s, play form3)
		{
			myMessageBox myMessageBox = new myMessageBox(s, form3);
			myMessageBox.Show();
			return myMessageBox;
		}

		public static myMessageBox CallMessageBox(string s, settings form4)
		{
			myMessageBox myMessageBox = new myMessageBox(s, form4);
			myMessageBox.Show();
			return myMessageBox;
		}

		public static void PlayButtonSound()
		{
			using (SoundPlayer soundPlayer = new SoundPlayer(Resources.ButtonClick))
			{
				soundPlayer.Play();
			}
		}

		public static void PlayLeaveButtonSound()
		{
			using (SoundPlayer soundPlayer = new SoundPlayer(Resources.ButtonLeave))
			{
				soundPlayer.Play();
			}
		}

		public static void PlayInfoButtonSound()
		{
			using (SoundPlayer soundPlayer = new SoundPlayer(Resources.ButtonInfoClick))
			{
				soundPlayer.Play();
			}
		}
	}
}
