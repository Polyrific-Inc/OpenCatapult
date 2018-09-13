// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Engine.ApiService
{
    public abstract class BaseService
    {
        protected BaseService(ApiClient api)
        {
            Api = api;
        }

        protected ApiClient Api { get; }
    }
}