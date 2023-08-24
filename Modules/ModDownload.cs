using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static EMCL.Modules.MinecraftJson;

namespace EMCL.Modules
{
    public static class ModDownload
    {
        //public static HttpClient client = new HttpClient();
        private static string _downloadSrc = "launchermeta.mojang.com";
        public static string DownloadSrc { get { return _downloadSrc; } }

        public static async Task<string> GetAsync(string uri)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }

        public static MinecraftVersionList? GetMinecraftVersionList()
        {
            MinecraftVersionList? versions = null!;
            try
            {
                Task<string> responce = GetAsync($"https://{ModDownload.DownloadSrc}/mc/game/version_manifest_v2.json");
                string result = responce.Result;
                versions = JsonConvert.DeserializeObject<MinecraftVersionList>(result);
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
        }

        public static MinecraftVersionInfo? GetMinecraftVersionInfo(string uri)
        {
            MinecraftVersionInfo? versions = null!;
            try
            {
                Task<string> responce = GetAsync($"https://{ModDownload.DownloadSrc}/{uri}");
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
        }
    }
}