using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EMCL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("./args.txt");//目前直接读取程序目录下args.txt内的内容
            string args = sr.ReadToEnd();
            Thread t = new Thread(() => launchGame("C:\\Program Files\\Java\\jdk-17.0.5\\bin\\javaw.exe",args));//创建MC启动线程
            t.Start();
        }
        
        public int launchGame(string javaPath, string launchArgs)
        {

            Process mc = new Process();
            mc.StartInfo.FileName = javaPath;//使用传入的Java Path
            mc.StartInfo.Arguments = launchArgs;//使用传入的参数
            mc.StartInfo.UseShellExecute = false;//不使用命令行启动
            mc.StartInfo.RedirectStandardOutput = true;
            mc.StartInfo.RedirectStandardError = true;
            mc.StartInfo.RedirectStandardInput = true;
            mc.Start();//MC，启动！
            return 0;
        }
    }
}
