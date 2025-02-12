using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace DbUpgrade.Util
{
    public static class FileProcessor
    {
        public static readonly string[] packageExtensions = [".zip", ".rar", ".7z", ".tar", ".tar.gz", ".gz", ".bz2", ".xz"];
        
        /// <summary>
        /// 递归解压压缩包，并返回本次升级的版本号
        /// </summary>
        /// <param name="paths">路径集合</param>
        public static (string, List<string>) ExtractPackagesAndGetVersion(string[] paths, bool isRecursion = false)
        {
            List<string> extractedDirectories = [];
            string version = "";
            try
            {
                for (var i = 0; i < paths.Length; i++)
                {
                    var path = paths[i];
                    // 如果是目录，递归调用
                    if (Directory.Exists(path))
                    {
                        var fileSystems = Directory.GetFileSystemEntries(path);
                        ExtractPackagesAndGetVersion(fileSystems, true);
                    }
                    // 如果是文件，且类型是压缩包，解压其到当前文件所在目录
                    if (File.Exists(path) && Path.GetExtension(path) is string extension && packageExtensions.Contains(extension))
                    {
                        // 最外层的压缩包，按约定是以版本号命名
                        if (i == 0 && !isRecursion)
                        {
                            version = RegexDeclare.Version().Match(Path.GetFileName(path)).Value;
                        }
                        // 记录解压后的目录路径
                        Console.WriteLine($"文件路径：{path}");
                        Console.WriteLine($"解压目录路径：{Path.GetFileNameWithoutExtension(path)}");
                        extractedDirectories.Add(Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path));
                        ExtractPackage(path);
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleUtil.Error(ex.ToString(), ConsoleColor.Red);
            }
            return (version, extractedDirectories);
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="path">路径</param>
        private static void ExtractPackage(string path)
        {
            if (Path.GetExtension(path) == ".rar")
            {
                using var archive = RarArchive.Open(path);
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(Path.GetDirectoryName(path)!, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
            else
            {
                // 解压到压缩包所在目录
                ZipFile.ExtractToDirectory(path, Path.GetDirectoryName(path)!, Encoding.GetEncoding(936), true);
            }
        }

        /// <summary>
        /// 删除目录，及目录下的所有文件
        /// </summary>
        /// <param name="extractedDirectories">目录集合</param>
        public static void DeleteDirectories(List<string> extractedDirectories)
        {
            extractedDirectories.ForEach(m => Directory.Delete(m, true));
            extractedDirectories.Clear();
        }
    }
}
