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
    }
}
