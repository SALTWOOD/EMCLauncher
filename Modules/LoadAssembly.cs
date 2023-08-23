using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks; 

namespace EMCL.Modules
{
    //public static byte[] GetResources(string ResourceName)
    //{
    //Log("[System] 获取资源：" + ResourceName);
    //byte[] Raw = My.Resources.ResourceManager.GetObject(ResourceName);
    //return Raw;
    //}

    public static class LoadAssembly
    {
        private static Assembly assemblyNAudio = null!;
        private static Assembly assemblyJson = null!;
        private static Assembly assemblyNAudioLock = null!;
        private static Assembly assemblyJsonLock = null!;
        public static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assets = Assembly.GetAssembly(typeof(App))!;
            if (args.Name.StartsWith("NAudio"))
            {
                lock (assemblyJsonLock)
                {
                    if (assemblyJson == null)
                    {
                        Stream json = assets.GetManifestResourceStream("NAudio.dll")!;
                        ModLogger.Log("[Start] 加载 DLL：NAudio");
                        byte[] bytes;
                        // 从嵌入的资源文件中获取字节数组
                        using (BinaryReader br = new BinaryReader(json))
                        {
                            bytes = br.ReadBytes((int)json.Length);
                        }
                        // 从字节数组中加载程序集
                        assemblyJson = Assembly.Load(bytes);
                    }
                    return assemblyJson;
                }
            }
            else if (args.Name.StartsWith("Newtonsoft.Json"))
            {
                lock (assemblyJsonLock)
                {
                    if (assemblyJson == null)
                    {
                        Stream json = assets.GetManifestResourceStream("Newtonsoft.Json.dll")!;
                        ModLogger.Log("[Start] 加载 DLL：Json");
                        byte[] bytes;
                        // 从嵌入的资源文件中获取字节数组
                        using (BinaryReader br = new BinaryReader(json))
                        {
                            bytes = br.ReadBytes((int)json.Length);
                        }
                        // 从字节数组中加载程序集
                        assemblyJson = Assembly.Load(bytes);
                    }
                    return assemblyJson;
                }
            }
            else return null!;
        }
    }
}
