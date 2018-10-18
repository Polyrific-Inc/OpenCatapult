// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class VersionService : BaseService, IVersionService
    {
        public VersionService(IApiClient api) : base(api)
        {
        }

        public async Task<string> GetApiVersion()
        {
            var apiVersion = await Api.Get<JObject>("version");

            return (string)apiVersion.SelectToken("version");
        }
    }
}
