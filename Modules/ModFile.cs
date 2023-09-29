using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EMCL.Modules
{
    //public static byte[] GetResources(string ResourceName)
    //{
    //Log("[System] 获取资源：" + ResourceName);
    //byte[] Raw = My.Resources.ResourceManager.GetObject(ResourceName);
    //return Raw;
    //}
    public static class ModFile
    {
        public static readonly Assembly assembly = Assembly.GetExecutingAssembly();

        public static string GetInternalFile(string path)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"EMCL.{path.Replace('/','.')}";
            Stream? stream = _assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                return new StreamReader(stream).ReadToEnd();
            }
            else
            {
                return "";
            }
        }

        public static Assembly AssemblyResolve(string arg)
        {
            Assembly assets = Assembly.GetAssembly(typeof(App))!;
            Assembly? assembly = null;
            if (assembly == null)
            {
                Stream json = assets.GetManifestResourceStream(arg)!;
                ModLogger.Log($"[Library] 加载 DLL：{arg}");
                byte[] bytes;
                // 从嵌入的资源文件中获取字节数组
                using (BinaryReader br = new BinaryReader(json))
                {
                    bytes = br.ReadBytes((int)json.Length);
                }
                // 从字节数组中加载程序集
                assembly = Assembly.Load(bytes);
            }
            return assembly;
        }

        public static void RemoveOutdatedLogs(string fillter = "*.log", string location = "EMCL/Logs", int days = 7, int maxCount = 15)
        {
            int count = 0;
            foreach (string dir in Directory.GetDirectories($"{ModPath.path}{location}"))
            {
                DirectoryInfo subDirectory = new DirectoryInfo(dir);
                foreach (FileInfo file in subDirectory.GetFiles(fillter))
                {
                    if (file.Exists)
                    {
                        if (file.LastWriteTime < DateTime.Now.AddDays(-days) && subDirectory.GetFiles(fillter).Length > 15)
                        {
                            file.Delete();
                        }
                    }
                }

            }
            string[] logDirectory = Directory.GetFiles($"{ModPath.path}{location}", fillter);
            foreach (string file in logDirectory)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Exists)
                {
                    if (fileInfo.LastWriteTime < DateTime.Now.AddDays(-days) || logDirectory.Length > maxCount)
                    {
                        fileInfo.Delete();
                    }
                }
            }
            if (count > 0)
            {
                ModLogger.Log($"[Logger] 成功清理 {count} 条过时的日志文件。");
            }
        }
    }
}
