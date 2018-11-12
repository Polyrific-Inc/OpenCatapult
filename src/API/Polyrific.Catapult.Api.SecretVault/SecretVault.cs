// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.AspNetCore.DataProtection;
using Polyrific.Catapult.Api.Core.Security;
using Polyrific.Catapult.Shared.Common.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.SecretVault
{
    public class SecretVault : ISecretVault
    {
        private readonly IDataProtector _protector;
        private readonly ITextWriter _textWriter;

        private const string FolderName = "ExternalService";

        public SecretVault(IDataProtectionProvider provider, ITextWriter textWriter)
        {
            _protector = provider.CreateProtector("Catapult.LocalSecretVault");
            _textWriter = textWriter;
        }

        public async Task Add(string name, string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var protectedValue = _protector.Protect(value);
            await _textWriter.Write(FolderName, name, protectedValue);
        }

        public async Task Delete(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _textWriter.Delete(FolderName, name);
        }

        public async Task<string> Get(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var protectedValue = await _textWriter.Read(FolderName, name);

            if (!string.IsNullOrEmpty(protectedValue))
                return _protector.Unprotect(protectedValue);
            else
                return null;
        }

        public async Task Update(string name, string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _textWriter.Delete(FolderName, name);

            var protectedValue = _protector.Protect(value);
            await _textWriter.Write(FolderName, name, protectedValue);
        }

        public Task<string> Encrypt(string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(_protector.Protect(value));
        }

        public Task<string> Decrypt(string encryptedValue, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(_protector.Unprotect(encryptedValue));
        }
    }
}
