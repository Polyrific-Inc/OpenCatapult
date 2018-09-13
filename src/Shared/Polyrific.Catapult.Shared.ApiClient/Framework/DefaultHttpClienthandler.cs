// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Net;
using System.Net.Http;

namespace Polyrific.Catapult.Shared.ApiClient.Framework
{
    public class DefaultHttpClientHandler : HttpClientHandler
    {
        public DefaultHttpClientHandler() =>
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
    }
}