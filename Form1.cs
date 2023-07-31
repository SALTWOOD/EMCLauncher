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
            try
            {
                StreamReader sr = new StreamReader("./args.txt");//目前直接读取程序目录下args.txt内的内容
                string args = sr.ReadToEnd();
                StreamReader jr = new StreamReader("./java.txt");//目前直接读取程序目录下args.txt内的内容
                string java = jr.ReadToEnd();
                sr.Close();
                jr.Close();
                Thread t = new Thread(() => launchGame(java,args));//创建MC启动线程
                t.Start();
                MessageBox.Show("启动成功","提示",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType()}\n{ex.Message}\n{ex.StackTrace}\n\n现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！");
                if (MessageBox.Show($"{ex.GetType()}\n{ex.Message}\n{ex.StackTrace}\n\n现在反馈问题吗？如果不反馈，这个问题可能永远无法解决！","无法处理的异常",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://github.com/SALTWOOD/EMCLauncher/issues/new/choose");
                }
            }
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
