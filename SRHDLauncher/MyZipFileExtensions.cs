using System;
using System.IO;
using System.IO.Compression;

namespace SRHDLauncher
{
	public static class MyZipFileExtensions
	{
		public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName, IProgress<ZipProgress> progress)
		{
			source.ExtractToDirectory(destinationDirectoryName, progress, overwrite: false);
		}

		public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName, IProgress<ZipProgress> progress, bool overwrite)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (destinationDirectoryName == null)
			{
				throw new ArgumentNullException("destinationDirectoryName");
			}
			DirectoryInfo directoryInfo = Directory.CreateDirectory(destinationDirectoryName);
			string fullName = directoryInfo.FullName;
			int num = 0;
			foreach (ZipArchiveEntry entry in source.Entries)
			{
				num++;
				string fullPath = Path.GetFullPath(Path.Combine(fullName, entry.FullName));
				if (!fullPath.StartsWith(fullName, StringComparison.OrdinalIgnoreCase))
				{
					throw new IOException("File is extracting to outside of the folder specified.");
				}
				try
				{
					ZipProgress value = new ZipProgress(source.Entries.Count, num, entry.FullName);
					progress.Report(value);
				}
				catch (Exception)
				{
				}
				if (Path.GetFileName(fullPath).Length == 0)
				{
					if (entry.Length != 0)
					{
						throw new IOException("Directory entry with data.");
					}
					Directory.CreateDirectory(fullPath);
				}
				else
				{
					Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
					try
					{
						entry.ExtractToFile(fullPath, overwrite: true);
					}
					catch (Exception)
					{
					}
				}
			}
		}
	}
}
