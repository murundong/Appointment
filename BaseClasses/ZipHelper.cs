using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
    public class ZipHelper
    {
        public static string Compress(string filePath, string zipPath = "", string zipFileName = "")
        {
            try
            {
                var fileName = Path.GetFileName(filePath);
                if (string.IsNullOrWhiteSpace(zipFileName))
                    zipFileName = Path.GetFileNameWithoutExtension(filePath);
                if (string.IsNullOrWhiteSpace(zipPath))
                {
                    var dir = Path.GetDirectoryName(filePath);
                    zipPath = string.Format("{0}\\{1}{2}.zip", dir, zipFileName,DateTime.Now.Ticks.ToString("x"));
                }
                if (File.Exists(zipPath))
                    File.Delete(zipPath);
                using (ZipArchive zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                {
                    var zipInfo = zipArchive.CreateEntryFromFile(filePath, fileName);
                }
            }
            catch
            {
                zipPath = string.Empty;
            }
            return zipPath;
        }
    }
}
