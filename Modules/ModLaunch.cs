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
        public static string GetLaunchArgs()
        {
            List<LaunchArg> args = new List<LaunchArg>();
            args.Add(new LaunchArg("Dfml.ignoreInvalidMinecraftCertificates", "True", "="));
            args.Add(new LaunchArg("Djava.library.path", $"\"{ModPath.pathMCFolder}versions/1.20.1/1.20.1-natives\"", "="));
            args.Add(new LaunchArg("Dminecraft.launcher.brand", $"{Metadata.name}", "="));
            args.Add(new LaunchArg("Dminecraft.launcher.version", $"{Metadata.protocol}", "="));
            args.Add(new LaunchArg("cp", $"\"{libraries}com/github/oshi/oshi-core/6.2.2/oshi-core-6.2.2.jar;{libraries}com/google/code/gson/gson/2.10/gson-2.10.jar;{libraries}com/google/guava/failureaccess/1.0.1/failureaccess-1.0.1.jar;{libraries}com/google/guava/guava/31.1-jre/guava-31.1-jre.jar;{libraries}com/ibm/icu/icu4j/71.1/icu4j-71.1.jar;{libraries}com/mojang/authlib/4.0.43/authlib-4.0.43.jar;{libraries}com/mojang/blocklist/1.0.10/blocklist-1.0.10.jar;{libraries}com/mojang/brigadier/1.1.8/brigadier-1.1.8.jar;{libraries}com/mojang/datafixerupper/6.0.8/datafixerupper-6.0.8.jar;{libraries}com/mojang/logging/1.1.1/logging-1.1.1.jar;{libraries}com/mojang/patchy/2.2.10/patchy-2.2.10.jar;{libraries}com/mojang/text2speech/1.17.9/text2speech-1.17.9.jar;{libraries}commons-codec/commons-codec/1.15/commons-codec-1.15.jar;{libraries}commons-io/commons-io/2.11.0/commons-io-2.11.0.jar;{libraries}commons-logging/commons-logging/1.2/commons-logging-1.2.jar;{libraries}io/netty/netty-buffer/4.1.82.Final/netty-buffer-4.1.82.Final.jar;{libraries}io/netty/netty-codec/4.1.82.Final/netty-codec-4.1.82.Final.jar;{libraries}io/netty/netty-common/4.1.82.Final/netty-common-4.1.82.Final.jar;{libraries}io/netty/netty-handler/4.1.82.Final/netty-handler-4.1.82.Final.jar;{libraries}io/netty/netty-resolver/4.1.82.Final/netty-resolver-4.1.82.Final.jar;{libraries}io/netty/netty-transport-classes-epoll/4.1.82.Final/netty-transport-classes-epoll-4.1.82.Final.jar;{libraries}io/netty/netty-transport-native-unix-common/4.1.82.Final/netty-transport-native-unix-common-4.1.82.Final.jar;{libraries}io/netty/netty-transport/4.1.82.Final/netty-transport-4.1.82.Final.jar;{libraries}it/unimi/dsi/fastutil/8.5.9/fastutil-8.5.9.jar;{libraries}net/java/dev/jna/jna-platform/5.12.1/jna-platform-5.12.1.jar;{libraries}net/java/dev/jna/jna/5.12.1/jna-5.12.1.jar;{libraries}net/sf/jopt-simple/jopt-simple/5.0.4/jopt-simple-5.0.4.jar;{libraries}org/apache/commons/commons-compress/1.21/commons-compress-1.21.jar;{libraries}org/apache/commons/commons-lang3/3.12.0/commons-lang3-3.12.0.jar;{libraries}org/apache/httpcomponents/httpclient/4.5.13/httpclient-4.5.13.jar;{libraries}org/apache/httpcomponents/httpcore/4.4.15/httpcore-4.4.15.jar;{libraries}org/apache/logging/log4j/log4j-api/2.19.0/log4j-api-2.19.0.jar;{libraries}org/apache/logging/log4j/log4j-core/2.19.0/log4j-core-2.19.0.jar;{libraries}org/apache/logging/log4j/log4j-slf4j2-impl/2.19.0/log4j-slf4j2-impl-2.19.0.jar;{libraries}org/joml/joml/1.10.5/joml-1.10.5.jar;{libraries}org/lwjgl/lwjgl-glfw/3.3.1/lwjgl-glfw-3.3.1.jar;{libraries}org/lwjgl/lwjgl-glfw/3.3.1/lwjgl-glfw-3.3.1-natives-windows.jar;{libraries}org/lwjgl/lwjgl-glfw/3.3.1/lwjgl-glfw-3.3.1-natives-windows-arm64.jar;{libraries}org/lwjgl/lwjgl-glfw/3.3.1/lwjgl-glfw-3.3.1-natives-windows-x86.jar;{libraries}org/lwjgl/lwjgl-jemalloc/3.3.1/lwjgl-jemalloc-3.3.1.jar;{libraries}org/lwjgl/lwjgl-jemalloc/3.3.1/lwjgl-jemalloc-3.3.1-natives-windows.jar;{libraries}org/lwjgl/lwjgl-jemalloc/3.3.1/lwjgl-jemalloc-3.3.1-natives-windows-arm64.jar;{libraries}org/lwjgl/lwjgl-jemalloc/3.3.1/lwjgl-jemalloc-3.3.1-natives-windows-x86.jar;{libraries}org/lwjgl/lwjgl-openal/3.3.1/lwjgl-openal-3.3.1.jar;{libraries}org/lwjgl/lwjgl-openal/3.3.1/lwjgl-openal-3.3.1-natives-windows.jar;{libraries}org/lwjgl/lwjgl-openal/3.3.1/lwjgl-openal-3.3.1-natives-windows-arm64.jar;{libraries}org/lwjgl/lwjgl-openal/3.3.1/lwjgl-openal-3.3.1-natives-windows-x86.jar;{libraries}org/lwjgl/lwjgl-opengl/3.3.1/lwjgl-opengl-3.3.1.jar;{libraries}org/lwjgl/lwjgl-opengl/3.3.1/lwjgl-opengl-3.3.1-natives-windows.jar;{libraries}org/lwjgl/lwjgl-opengl/3.3.1/lwjgl-opengl-3.3.1-natives-windows-arm64.jar;{libraries}org/lwjgl/lwjgl-opengl/3.3.1/lwjgl-opengl-3.3.1-natives-windows-x86.jar;{libraries}org/lwjgl/lwjgl-stb/3.3.1/lwjgl-stb-3.3.1.jar;{libraries}org/lwjgl/lwjgl-stb/3.3.1/lwjgl-stb-3.3.1-natives-windows.jar;{libraries}org/lwjgl/lwjgl-stb/3.3.1/lwjgl-stb-3.3.1-natives-windows-arm64.jar;{libraries}org/lwjgl/lwjgl-stb/3.3.1/lwjgl-stb-3.3.1-natives-windows-x86.jar;{libraries}org/lwjgl/lwjgl-tinyfd/3.3.1/lwjgl-tinyfd-3.3.1.jar;{libraries}org/lwjgl/lwjgl-tinyfd/3.3.1/lwjgl-tinyfd-3.3.1-natives-windows.jar;{libraries}org/lwjgl/lwjgl-tinyfd/3.3.1/lwjgl-tinyfd-3.3.1-natives-windows-arm64.jar;{libraries}org/lwjgl/lwjgl-tinyfd/3.3.1/lwjgl-tinyfd-3.3.1-natives-windows-x86.jar;{libraries}org/lwjgl/lwjgl/3.3.1/lwjgl-3.3.1.jar;{libraries}org/lwjgl/lwjgl/3.3.1/lwjgl-3.3.1-natives-windows.jar;{libraries}org/lwjgl/lwjgl/3.3.1/lwjgl-3.3.1-natives-windows-arm64.jar;{libraries}org/lwjgl/lwjgl/3.3.1/lwjgl-3.3.1-natives-windows-x86.jar;{libraries}org/slf4j/slf4j-api/2.0.1/slf4j-api-2.0.1.jar;{ModPath.pathMCFolder}versions/1.20.1/1.20.1.jar\""));
            args.Add(new LaunchArg("Xms", $"1G",""));
            args.Add(new LaunchArg("Xmx", $"6G",""));
            args.Add(new LaunchArg("jar", $""));
            args.Add(new LaunchArg("net.minecraft.client.main.Main"));
            args.Add(new LaunchArg("-username",$""));
            args.Add(new LaunchArg("-version",$"1.20.1"));
            args.Add(new LaunchArg("-gameDir",$"\"{ModPath.pathMCFolder}\""));
            args.Add(new LaunchArg("-assetsDir",$"\"{ModPath.pathMCFolder}assets\""));
            args.Add(new LaunchArg("-assetIndex",$"5"));
            args.Add(new LaunchArg("-uuid",$""));
            args.Add(new LaunchArg("-accessToken",$""));
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
