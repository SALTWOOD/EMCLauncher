using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EMCL.Constants;

namespace EMCL.Modules
{
    public static class ModConfig
    {
        #region 配置读取
        public static void WriteConfig(Config config)
        {
            StreamWriter sw = new StreamWriter($"{ModPath.path}EMCL/settings.json");
            sw.Write(JsonConvert.SerializeObject(config));
            sw.Close();
        }

        public static Config ReadConfig()
        {
            Config result;
            using (StreamReader sr = new StreamReader($"{ModPath.path}EMCL/settings.json"))
            {
                result = JsonConvert.DeserializeObject<Config>(sr.ReadToEnd())!;
            }
            return result;
        }
        #endregion

        public class Config
        {
            public List<List<object>>? java = new();
            public long tempTime = 0;
            public bool forceDisableJavaAutoSearch = false;
            public string language = "zh-cn";
        }
    }
}
