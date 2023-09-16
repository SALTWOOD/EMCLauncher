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

        private MinecraftLauncherJson()
        {
            this.arguments = default!;
            this.assetIndex = default!;
            this.assets = default!;
            this.complianceLevel = default!;
            this.downloads = default!;
            this.id = default!;
            this.javaVersion = default!;
            this.libraries = default!;
            this.loggingClient = default!;
            this.loggingFile = default!;
            this.logging = default!;
            this.mainClass = default!;
            this.minimumLauncherVersion = default!;
            this.releaseTime = default!;
            this.time = default!;
            this.type = default!;
        }
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

        private Downloads()
        {
            this.client = default!;
            this.client_mappings = default!;
            this.server = default!;
            this.server_mappings = default!;
        }
    }

    public class Asset
    {
        public string id = "";
        public string sha1 = "";
        public long size = 0;
        public long totalSize = 0;
        public string url = "";
        public string path;

        private Asset()
        {
            this.id = default!;
            this.sha1 = default!;
            this.size = default!;
            this.totalSize = default!;
            this.url = default!;
            this.path = default!;
        }
    }

    public class Download
    {
        public Asset artifact;

        private Download()
        {
            this.artifact = default!;
        }
    }

    public class Library
    {
        public string name;
        public List<Dictionary<string,object>> rules;
        public string action;
        public Download downloads;

        private Library()
        {
            this.rules = default!;
            this.action = default!;
            this.name = default!;
            this.downloads = default!;
        }
    }

    public class JavaVersion
    {
        public string component;
        public string majorVersion;

        private JavaVersion()
        {
            this.component = default!;
            this.majorVersion = default!;
        }
    }

    public class Logging
    {
        public Client client;

        private Logging()
        {
            this.client = default!;
        }
    }

    public class Client
    {
        public string argument;
        public Asset file;
        public string type;

        public Client()
        {
            this.argument = default!;
            this.file = default!;
            this.type = default!;
        }
    }
}
