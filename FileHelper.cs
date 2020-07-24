using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using IWshRuntimeLibrary;

namespace Cat_Win8_Start_Menu
{
    class FileHelper
    {
        public static string GetShortcutTargetPath(string fileName)
        {
            var shell = new WshShell();
            IWshShortcut lnkPath = (IWshShortcut)shell.CreateShortcut(fileName);
            return lnkPath.TargetPath;
        }

        public static void RunEXE(string fileName)
        {
            Process process = new Process();//创建进程对象    
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName); // 括号里是(程序名,参数)
            process.StartInfo = startInfo;
            process.Start();
        }

        public static string GetSaftyFileName(string fileName, bool isDir)
        {
            var cut = fileName.Split('\\');
            var final = cut[cut.Length - 1];
            if (isDir == false)
                final = final.Split('.')[0];
            return final;
        }

        public static BitmapImage GetFileIcon(string fileName)
        {
            var final = fileName;

            if (fileName.Substring(fileName.Length - 4) == ".lnk")
            {
                var sc = GetShortcutTargetPath(fileName);
                if (System.IO.File.Exists(sc))
                    final = sc;
            }
            return BitmapToBitmapImage(Icon.ExtractAssociatedIcon(final).ToBitmap());
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}
