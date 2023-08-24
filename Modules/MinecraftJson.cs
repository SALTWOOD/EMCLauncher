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
}
