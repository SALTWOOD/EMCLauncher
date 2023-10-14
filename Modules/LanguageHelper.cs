using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SaltLib.Modules;
using Newtonsoft.Json;

namespace SaltLib.Modules
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
        public static Language fallbackLanguage = new Language();

        public static void Initialize(string name)
        {
            string s = ModFile.GetInternalFile($"Languages/{name}.json");
            Language? temp = JsonConvert.DeserializeObject<Language>(s);
            if (temp != null)
            {
                language = temp;
            }
            s = ModFile.GetInternalFile("Languages/en-us.json");
            temp = JsonConvert.DeserializeObject<Language>(s);
            if (temp != null)
            {
                fallbackLanguage = temp;
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
                return GetByLanguage(fallbackLanguage, translation);
            }
        }

        public static string Get(string translation, params string[] args)
        {
            try
            {
                string format = language.translations.Where(l => (l.First() == translation)).First().Last();
                return string.Format(format, args);
            }
            catch
            {
                return GetByLanguage(fallbackLanguage, translation);
            }
        }

        public static string GetByLanguage(Language l, string translation, bool retry = false)
        {
            try
            {
                return l.translations.Where(l => (l.First() == translation)).First().Last();
            }
            catch
            {
                if (retry)
                {
                    return GetByLanguage(l, translation);
                }
                else
                {
                    return translation;
                }
            }
        }
    }
}
