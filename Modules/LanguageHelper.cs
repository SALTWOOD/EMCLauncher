using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EMCL.Modules;
using Newtonsoft.Json;

namespace EMCL.Modules
{
    public class Language
    {
        public string languageAuthor = "";
        public string languageName = "";
        public string languageVersion = "";
        public List<List<string>> translations = new List<List<string>>();
    }

    public static class LanguageHelper
    {
        public static Language language = new Language();

        public static void Initialize(string name)
        {
            string s = ModFile.GetInternalFile("Languages/zh-cn.json");
            Language? temp = JsonConvert.DeserializeObject<Language>(s);
            if (temp != null)
            {
                language = temp;
            }
        }

        public static string Get(string translation)
        {
            try
            {
                return language.translations.Where(l => (l.First() == translation)).First().Last();
            }
            catch
            {
                return translation;
            }
        }
    }
}
