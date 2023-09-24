using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NAudio.SoundFont;
using Newtonsoft.Json;
using static EMCL.Modules.MinecraftJson;

namespace EMCL.Modules
{
    public static class ModDownload
    {
        public enum DownloadSourceEnum
        {
            OFFICIAL,
            BMCLAPI
        }

        public static string Source
        {
            get
            {
                switch (source)
                {
                    default:
                    case DownloadSourceEnum.OFFICIAL:
                        {
                            return "https://launchermeta.mojang.com";
                        }
                    case DownloadSourceEnum.BMCLAPI:
                        {
                            return "https://bmclapi2.bangbang93.com/";
                        }
                }
            }
        }

        public static DownloadSourceEnum source = DownloadSourceEnum.BMCLAPI;

        public static string CheckIfRedirect(string url)
        {
            string result = url;
            switch (source)
            {
                default:
                case DownloadSourceEnum.OFFICIAL:
                    {
                        return url;
                    }
                case DownloadSourceEnum.BMCLAPI:
                    {
                        result = result.Replace("https://libraries.minecraft.net", "https://bmclapi2.bangbang93.com/maven");
                        result = result.Replace("http://resources.download.minecraft.net", "https://bmclapi2.bangbang93.com/assets");
                        result = result.Replace("https://launchermeta.mojang.com", "https://bmclapi2.bangbang93.com");
                        result = result.Replace("https://launcher.mojang.com", "https://bmclapi2.bangbang93.com");
                        return result;
                    }
            }
        }

        public static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("EMCL/dt-0.0.4 (Windows NT; Win64; x64) WPF_Application System.Net.Http.HttpClient");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            client.DefaultRequestHeaders.AcceptEncoding.Clear();
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("identity");
            client.DefaultRequestHeaders.Connection.Clear();
            client.DefaultRequestHeaders.Connection.ParseAdd("close");
            return client;
        }

        //public static HttpClient client = ModDownload.GetHttpClient();

        public static async Task<string> GetAsync(string uri)
        {
            string jsonResponse = "";
            try
            {
                HttpClient client = ModDownload.GetHttpClient();
                HttpResponseMessage response = client.GetAsync(uri).Result;
                response.EnsureSuccessStatusCode();
                jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            catch (Exception ex)
            {
                ModLogger.Log($"[Download] 出错了！");
                ModLogger.Log(ex, "下载时出现错误");
            }
            return jsonResponse;
        }

        public static async Task<byte[]> GetByteArrayAsync(string uri)
        {
            byte[] resp = new byte[1024];
            try
            {
                HttpClient client = ModDownload.GetHttpClient();
                return await client.GetByteArrayAsync(uri);
            }
            catch (Exception ex)
            {
                ModLogger.Log($"[Download] 出错了！");
                ModLogger.Log(ex, "下载时出现错误");
            }
            return resp;
        }

        public static MinecraftVersionList? GetMinecraftVersionList()
        {
            MinecraftVersionList? versions = null!;
            try
            {
                Task<string> responce = GetAsync(CheckIfRedirect("https://launchermeta.mojang.com/mc/game/version_manifest_v2.json"));
                string result = responce.Result;
                versions = JsonConvert.DeserializeObject<MinecraftVersionList>(result);
            }
            catch (Exception ex)
            {
                ModLogger.Log($"[Version] 从远端服务器获取 Minecraft 版本列表时出现错误！");
                ModLogger.Log(ex, "未处理的异常");
                return null;
            }
            if (versions != null)
            {
                return versions;
            }
            else
            {
                return null;
            }
        }

        /*
        public static MinecraftVersionInfo? GetMinecraftVersionInfo(string uri)
        {
            MinecraftVersionInfo? versions = null!;
            try
            {
                Task<string> responce = GetAsync($"https://bmclapi2.bangbang93.com/{uri}");
                string result = responce.Result;
                versions = JsonConvert.DeserializeObject<MinecraftVersionInfo>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
            if (versions != null)
            {
                return versions;
            }
            else
            {
                return null;
            }
        }*/

        public static async Task DownloadMinecraft(MinecraftVersionInfo version)
        {
            ModLogger.Log($"[Download] 收到 Minecraft {version.id} 的下载请求，准备下载...");
            ModLogger.Log($"[Download] 开始下载 Minecraft 本体");
            Directory.CreateDirectory($"{ModPath.pathMCFolder}versions/{version.id}");
            string jsonString;
            using (StreamWriter sw = new StreamWriter($"{ModPath.pathMCFolder}versions/{version.id}/{version.id}.json"))
            {
                jsonString = await GetAsync(CheckIfRedirect(version.url));
                sw.Write(jsonString);
            }
            ModLogger.Log($"[Download] Minecraft (Json) 文件下载完毕！\r\n    {ModPath.pathMCFolder}versions/{version.id}/{version.id}.json");
            MinecraftLauncherJson json = JsonConvert.DeserializeObject<MinecraftLauncherJson>(jsonString)!;
            ModLogger.Log($"[Download] Minecraft Json 解析完毕！");
            ModLogger.Log($"[Download] 开始下载 Minecraft (Java Archive File) ...");
            try
            {
                byte[] bytes = await GetByteArrayAsync(CheckIfRedirect(json.downloads.client.url));
                WriteByteArrayToFile(bytes, $"{ModPath.pathMCFolder}versions/{version.id}/{version.id}.jar");
            }
            catch (Exception ex)
            {
                ModLogger.Log($"[Download] 未捕获的异常！");
                ModLogger.Log(ex,"下载时出现错误");
            }
            ModLogger.Log($"[Download] Minecraft (Java Archive File) 文件下载完毕！\r\n    {ModPath.pathMCFolder}versions/{version.id}/{version.id}.jar");
            ModLogger.Log($"[Download] 开始补全 Minecraft 依赖库...共 {json.libraries.Count} 个依赖");
            int counter = 0;
            List<Action> actions = new List<Action>();
            foreach (MinecraftLauncherJson.Library i in json.libraries)
            {
                actions.Add(() =>
                {
                    int retryCounter = 0;
                    while (true)
                    {
                        try
                        {
                            Directory.CreateDirectory($"{ModPath.pathMCFolder}libraries/{ModString.RegexMatch(i.downloads.artifact.path, "(.+)/")}");
                            WriteByteArrayToFile(GetByteArrayAsync(CheckIfRedirect(i.downloads.artifact.url)).Result, $"{ModPath.pathMCFolder}libraries/{i.downloads.artifact.path}");
                            counter++;
                            ModLogger.Log($"[Download] 依赖库 {i.downloads.artifact.path} 下载完毕！{counter} 个/共 {json.libraries.Count} 个");
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (retryCounter >= 5)
                            {
                                throw new Exception("下载达到最大重试次数");
                            }
                            retryCounter++;
                            ModLogger.Log(ex, "未处理的异常");
                            ModLogger.Log($"[Download] 下载失败，第 {retryCounter} 次重试！");
                        }
                        finally { }
                    }
                });
            }
            ModThread.RunActions(actions, () =>
            {
                ModLogger.Log($"[Download] Minecraft 依赖库补全完毕！");
            });
        }

        public static bool WriteByteArrayToFile(byte[] byteArray, string fileName)
        {
            bool result = false;
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}