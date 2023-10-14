using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EMCL.Constants;
using System.Threading;
using System.Windows;

namespace SaltLib.Modules
{
    internal static class ModLogger
    {
        #region 已废弃
        /*
        public static StringBuilder logs = new StringBuilder();
        public static StreamWriter? logger = null;
        public static object loggerLock = new object();
        public static object loggerFlushLock = new object();

        public static void LoggerStart()
        {
            ModThread.RunThread(() =>
            {
                string loggerName = $"{ModPath.path}EMCL/Logs/{DateTime.Now.ToString("yy-MM-dd_HH-mm-ss")}.log";
                bool isSuccess = true;
                try
                {
                    System.IO.File.Create(loggerName).Dispose();
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                    isSuccess = false;
                    //Hint("可能同时开启了多个 EMCL，程序可能会出现问题！", HintType.Critical)
                    ModLogger.Log(ex, "日志初始化失败（疑似文件占用问题）");
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    ModLogger.Log(ex, "日志初始化失败", LogLevel.Hint);
                }
                try
                {
                    logger = new StreamWriter(loggerName, true) { AutoFlush = true };
                }
                catch (Exception ex)
                {
                    logger = null;
                    ModLogger.Log(ex, "日志写入失败", LogLevel.Hint);
                }
                try
                {
                    while (true)
                    {
                        if (ModRun.isExited)
                        {
                            ModLogger.Log("[Thread]<LoggerThread> 日志记录线程关闭中！");
                            LoggerFlush();
                            logs = new StringBuilder();
                            ModLogger.Log("[Thread]<LoggerThread> 日志记录线程关闭成功！");
                            LoggerFlush();
                            return;
                        }
                        if (isSuccess)
                        {
                            LoggerFlush();
                        }
                        else
                        {
                            logs = new StringBuilder();//清空 LogList 避免内存爆炸
                        }
                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    Log(ex, "LoggerThread 异常退出", LogLevel.Fatal);
                }
            }, "Logger", ThreadPriority.BelowNormal);
        }

        public static void LoggerFlush()
        {
            if (logger == null) return;
            string? log = null;
            lock (loggerFlushLock)
            {
                if (logs.Length > 0)
                {
                    StringBuilder cache = new StringBuilder();
                    cache = logs;
                    logs = new StringBuilder();
                    log = cache.ToString();
                }
            }

            if (log != null)
            {
                logger.Write(log);
            }
        }*/
        #endregion 已废弃

        /// <summary>
        /// 用于记录日志。
        /// </summary>
        /// <remarks>
        /// 在调用后将传入的信息存入 AppLog_yyyyMMdd_HHmmss.log
        /// </remarks>
        /// <param name="info"></param>
        /// <param name="level"></param>
        /// <param name="title"></param>
        public static void Log(string info, LogLevel level = LogLevel.Normal, string title = "出现错误")
        {
            string text = info;
            switch (level)
            {
                case LogLevel.Debug when Metadata.DEBUG:
                case LogLevel.Normal:
                case LogLevel.Information:
                    Logger.WriteInfo(text);
                    break;
                case LogLevel.Hint:
                    Logger.WriteInfo(text);
                    //TODO
                    break;
                case LogLevel.Message:
                    Logger.WriteInfo(text);
                    //TODO
                    break;
                case LogLevel.Error:
                    Logger.WriteWarn(text);
                    //TODO
                    break;
                case LogLevel.Fatal:
                    Logger.WriteError(text);
                    if (MainWindow._mainWindow != null) MainWindow._mainWindow.AppExit();
                    break;
            }
            Console.WriteLine(text);
            string repText = ModString.RegexReplace(info, "", "\\[[^\\]]+?\\] ");
        }

        public static void Log(Exception ex, string info, LogLevel level = LogLevel.Normal, string title = "出现错误")
        {
            ModLogger.Log($"[System] 捕获到异常！{info}\r\n{ex.GetType()}:{ex.Message}\r\n{ex.StackTrace}", LogLevel.Error);
        }

        public static void Log(string info, LogLevel level = LogLevel.Normal, string title = "出现错误", params Exception[] exs)
        {
            List<string> errors = new List<string>();
            foreach (Exception i in exs)
            {
                errors.Add($"{i.GetType()}:{i.Message}\r\n{i.StackTrace}\r\n\r\n");
            }
            ModLogger.Log($"[Test] {string.Join("\r\n\r\n\r\n", errors)}");
            ModLogger.Log($"[Test] {string.Join("\r\n\r\n\r\n", exs.Select(ex => ex.GetType().FullName))}");
            ModLogger.Log($"[System] 捕获到多重异常！{info}\r\n{string.Join("\r\n", errors.ToList())}", LogLevel.Error);
        }

        public enum LogLevel
        {
            Debug = 1,
            Normal = 2,
            Information = 3,
            Hint = 4,
            Message = 5,
            Error = 6,
            Fatal = 7
        }
    }
}
