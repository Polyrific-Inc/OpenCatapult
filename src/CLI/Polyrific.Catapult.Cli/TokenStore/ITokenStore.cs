// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Threading.Tasks;

namespace Polyrific.Catapult.Cli
{
    public interface ITokenStore
    {
        Task SaveToken(string token);
        Task<string> GetSavedToken();
        Task DeleteToken();
    }
}
