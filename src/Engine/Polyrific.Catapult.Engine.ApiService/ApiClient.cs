// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Polyrific.Catapult.Engine.ApiService
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<T> Get<T>(string path)
        {
            var result = await _httpClient.GetStringAsync(path);
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<bool> Head(string path)
        {
            var result = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, path),
                HttpCompletionOption.ResponseHeadersRead, default(CancellationToken));

            return result.IsSuccessStatusCode;
        }
    }
}