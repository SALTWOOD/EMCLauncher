using log4net;
using log4net.Appender;
using System;
using System.IO;

namespace EMCL
{
    public class Logger
    {
        private static string filepath = AppDomain.CurrentDomain.BaseDirectory + "/EMCL/Logs/";
        private static readonly log4net.ILog logComm = log4net.LogManager.GetLogger(nameof(Logger));
        private static readonly string time;

        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            time = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
        }

        /// <summary>
        /// 输出系统日志
        /// </summary>
        /// <param name="msg">信息内容</param>
        /// <param name="source">信息来源</param>
        private static void WriteLog(string msg, Action<object> action)
        {
            string filename = $"AppLog_{action.Method.Name}_{time}.log";
            var repository = LogManager.GetRepository();
            var appenders = repository.GetAppenders();
            if (appenders.Length > 0)
            {
                RollingFileAppender targetApder = null!;
                foreach (var Apder in appenders)
                {
                    if (Apder.Name == nameof(Logger))
                    {
                        targetApder = (Apder as RollingFileAppender)!;
                        break;
                    }
                }
                if (targetApder.Name == nameof(Logger))//如果是文件输出类型日志，则更改输出路径
                {
                    if (targetApder != null)
                    {
                        if (!targetApder.File.Contains(filename))
                        {
                            targetApder.File = "EMCL/Logs/" + filename;
                            targetApder.ActivateOptions();
                        }
                    }
                }
            }
            action(msg);
        }
        public static void WriteError(string msg)
        {
            WriteLog(msg, logComm.Error);
        }
        public static void WriteInfo(string msg)
        {
            WriteLog(msg, logComm.Info);
        }
        public static void WriteWarn(string msg)
        {
            WriteLog(msg, logComm.Warn);
        }
    }
}
