using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMCL.Modules
{
    public static class MinecraftJson
    {
        public class MinecraftVersionList
        {
            public Dictionary<string, string> latest;
            public List<MinecraftVersionInfo> versions;

            private MinecraftVersionList()
            {
                this.latest = null!;
                this.versions = null!;
            }
        }

        public class MinecraftVersionInfo
        {
            public string id;
            public string type;
            public string url;
            public string time;
            public string releaseTime;
            public string sha1;
            public int complianceLevel;

            private MinecraftVersionInfo()
            {
                this.id = null!;
                this.type = null!;
                this.url = null!;
                this.time = null!;
                this.releaseTime = null!;
                this.sha1 = null!;
                this.complianceLevel = 0;
            }
        }
    }

#pragma warning disable CS8618
    public class MinecraftLauncherJson
    {
        public Arguments arguments;
        public Asset assetIndex;
        public int assets;
        public int complianceLevel;
        public Downloads downloads;
        public string id;
        public JavaVersion javaVersion;
        public List<Library> libraries;
        public List<string> loggingClient;
        public List<string> loggingFile;
        public Logging logging;
        public string mainClass;
        public int minimumLauncherVersion;
        public string releaseTime;
        public string time;
        public string type;

        public class Download
        {
            public Asset artifact;
        }

        public class Library
        {
            public string name;
            public List<Dictionary<string, object>> rules;
            public string action;
            public Download downloads;
        }

        public class JavaVersion
        {
            public string component;
            public string majorVersion;
        }

        public class Logging
        {
            public Client client;
        }

        public class Client
        {
            public string argument;
            public Asset file;
            public string type;
        }

        public class Arguments
        {
            public List<object> game;
            public List<object> jvm;

            private Arguments()
            {
                this.game = default!;
                this.jvm = default!;
            }
        }

        public class Downloads
        {
            public Asset client;
            public Asset client_mappings;
            public Asset server;
            public Asset server_mappings;
        }

        public class Asset
        {
            public string id = "";
            public string sha1 = "";
            public long size = 0;
            public long totalSize = 0;
            public string url = "";
            public string path;
        }
    }
}
#pragma warning restore CS8618
