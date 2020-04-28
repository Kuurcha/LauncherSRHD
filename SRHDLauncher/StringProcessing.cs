using System.Text;

namespace SRHDLauncher
{
	internal static class StringProcessing
	{
		/// <summary>
		/// Замена страшной конструкции с выбором языков, сразу возвращает значение одной строчкой
		/// </summary>
		/// <param name="lang">Переменная, отвечающая за выбранный в лаунчере язык</param>
		/// <param name="messageRu">Сообщение на русском</param>
		/// <param name="messageEng">Сообщение на английском</param>
		/// <returns>Текст на нужном языке</returns>
		public static string getMessage(string lang, string messageRu, string messageEng)
		{
			if (lang == "ru") return messageRu;
			if (lang == "eng") return messageEng;
			return "lang chosen wrong";
		}
		public static string StepUp(string path)
		{
			if (path != null)
			{
				int length = path.Length;
				if (length > 3)
				{
					for (int num = path.Length - 1; num > 0; num--)
					{
						if (path[num] == '\\')
						{
							path = path.Remove(num);
							break;
						}
					}
				}
			}
			return path;
		}

		public static string getAppName(string path)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (path != null)
			{
				int length = path.Length;
				if (length > 3)
				{
					int num = path.Length - 1;
					while (num > 0 && path[num] != '\\')
					{
						num--;
					}
					for (int i = num + 1; i < path.Length; i++)
					{
						stringBuilder.Append(path[i]);
					}
				}
			}
			return stringBuilder.ToString();
		}

		public static string StepDown(string path)
		{
			int num = 0;
			if (path != null)
			{
				int length = path.Length;
				if (length > 3)
				{
					for (num = 0; num < path.Length - 1; num++)
					{
						if (path[num] == '\\')
						{
							path = path.Substring(num + 1);
							break;
						}
					}
					if (num == path.Length - 1)
					{
						path = "";
					}
				}
			}
			return path;
		}
	}
}
