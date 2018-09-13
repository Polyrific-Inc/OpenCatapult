// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Cli
{
    public class TokenStore : ITokenStore
    {
        private readonly CatapultCliConfig _config;

        public TokenStore(CatapultCliConfig config)
        {
            _config = config;
        }

        public Task DeleteToken()
        {
            return Task.Run(() => File.Delete(GetTokenPath()));
        }

        public async Task<string> GetSavedToken()
        {
            var path = GetTokenPath();
            try
            {
                return await File.ReadAllTextAsync(path);
            }
            catch(DirectoryNotFoundException)
            {
                return null;
            }
            catch(FileNotFoundException)
            {
                return null;
            }            
        }

        public async Task SaveToken(string token)
        {
            var path = GetTokenPath();
            var folder = Path.GetDirectoryName(path);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            await File.WriteAllTextAsync(path, token);
        }
        
        private string GetTokenPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _config.AppDataFolderPath, "token.txt");
        }
    }
}
