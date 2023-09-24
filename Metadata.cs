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
        public static string fullName = "Easy-Minecraft C# Launcher";
        public static string version = "0.0.5";

        public static string title = $"{name} v{version}";
        public static string fullTitle = $"{fullName} (version {version})";

        public static bool DEBUG = false;

        public static int protocol = 0x00_00_00_03;
    }
}
