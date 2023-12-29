using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMCL.Modules
{
    internal static class ModLaunch
    {
        public static string libraries = $"{ModPath.pathMCFolder}libraries/";
        public static string GetLaunchArgs(string version, IEnumerable<string> dependencies)
        {
            List<LaunchArg> args = new List<LaunchArg>();
            args.Add(new LaunchArg("Dfml.ignoreInvalidMinecraftCertificates", "True", "="));
            args.Add(new LaunchArg("Djava.library.path", $"\"{ModPath.pathMCFolder}versions/{version}/{version}-natives\"", "="));
            args.Add(new LaunchArg("Dminecraft.launcher.brand", $"{Metadata.name}", "="));
            args.Add(new LaunchArg("Dminecraft.launcher.version", $"{Metadata.protocol}", "="));
            args.Add(new LaunchArg("cp", string.Join(";", dependencies)));
            args.Add(new LaunchArg("Xms", $"1G",""));
            args.Add(new LaunchArg("Xmx", $"6G",""));
            //args.Add(new LaunchArg("jar", $""));
            args.Add(new LaunchArg("net.minecraft.client.main.Main"));
            args.Add(new LaunchArg("-username",$""));
            args.Add(new LaunchArg("-version", version));
            args.Add(new LaunchArg("-gameDir",$"\"{ModPath.pathMCFolder}\""));
            args.Add(new LaunchArg("-assetsDir",$"\"{ModPath.pathMCFolder}assets\""));
            args.Add(new LaunchArg("-assetIndex",$"5"));
            args.Add(new LaunchArg("-uuid",$"asdasdasd"));
            args.Add(new LaunchArg("-accessToken",$"asdds"));
            //args.Add(new LaunchArg("-userProperties","{}"));
            args.Add(new LaunchArg("-userType","msa"));
            args.Add(new LaunchArg("-versionType",$"\"{Metadata.name}\""));
            args.Add(new LaunchArg("-width", "854"));
            args.Add(new LaunchArg("-height", "480"));
            string result = "";
            foreach (LaunchArg arg in args)
            {
                result += $"{arg.ToArg()} ";
            }
            return result;
        }
    }

    public class LaunchArg
    {
        private string _key;
        private string _value;
        private string _sep;
        protected bool isValueOnly;

        public string key
        {
            get { return this._key; }
            set { this._key = value; }
        }
        public string value
        {
            get { return this._value; }
            set { this._value = value; }
        }
        public string sep
        {
            get { return this._sep; }
            set { this._sep = value; }
        }
        public LaunchArg(string key, string value, string sep = " ")
        {
            this.isValueOnly = false;
            this._key = key;
            this._value = value;
            this._sep = sep;
        }

        public LaunchArg(string value)
        { 
            this.isValueOnly = true;
            this._key = default!;
            this._value = value;
            this._sep = default!;
        }

        public override string ToString()
        {
            if (this.key == "-accessToken") { return $"-{this.key}{this.sep}ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            return this.isValueOnly ? this.value : $"-{this.key}{this.sep}{this.value}";
        }

        public string ToArg()
        {
            return this.isValueOnly ? this.value : $"-{this.key}{this.sep}{this.value}";
        }
    }
}
