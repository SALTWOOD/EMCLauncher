using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EMCL.Utils;
using System.Threading;

namespace EMCL.Modules
{
    internal static class ModLogger
    {
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
        }

        public static void Log(string info, LogLevel level = LogLevel.Normal, string title = "出现错误")
        {
            string text = $"[{ModTime.GetTimeNow()}] {info}\r\n";

            if ((level <= LogLevel.Debug && Metadata.DEBUG) || level > LogLevel.Debug)
            {
                lock (loggerLock)
                {
                    logs.Append(text);
                }
                Console.WriteLine(text);
            }
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
    }
}
