using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EMCL
{
    public static class Metadata
    {
        public static string name = "EMCL";
        public static string fullName = "Easy-Minecraft Launcher";
        public static string version = "dt-0.0.3";

        public static string title = $"{name} {version}";
        public static string fullTitle = $"{fullName} (version {version})";

        public static bool DEBUG = false;

        public static int protocol = 0x00_00_00_01;
    }
}
