﻿using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AzureAppService
{
    public class KuduDeployUtils : IDeployUtils
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public KuduDeployUtils(ILogger logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public async Task<bool> ExecuteDeployWebsiteAsync(string url, string username, string password, string artifactLocation)
        {
            _httpClient.BaseAddress = new Uri(GetDeployUrl(url));

            var authValue = Encoding.ASCII.GetBytes($"{username}:{password}");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(authValue));

            if (File.Exists(artifactLocation))
            {
                using (var stream = File.OpenRead(artifactLocation))
                {
                    var result = await _httpClient.PostAsync("/api/zipdeploy", new ProgressableStreamContent(stream, _logger));

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        return true;
                }
            }

            return false;
        }

        private string GetDeployUrl(string gitUrl)
        {
            // enforce https
            gitUrl = gitUrl.Replace("http://", "https://");
            var url = !gitUrl.Contains("https://") ? $"https://{gitUrl}" : gitUrl;
            return url;
        }
    }
}
