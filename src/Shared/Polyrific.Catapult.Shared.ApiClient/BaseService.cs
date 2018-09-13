// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.ApiClient
{
    public abstract class BaseService
    {
        protected BaseService(IApiClient api)
        {
            Api = api;
        }

        protected IApiClient Api { get; }
    }
}