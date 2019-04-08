using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.ManagedFile;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class ManagedFileService : BaseService, IManagedFileService
    {
        public ManagedFileService(IApiClient api) : base(api)
        {
        }

        public async Task<ManagedFileDto> CreateManagedFile(string fileName, byte[] file)
        {
            return await this.Api.PostFile<ManagedFileDto>("file", fileName, file);
        }

        public async Task UpdateManagedFile(int managedFileId, string fileName, byte[] file)
        {
            await this.Api.PutFile($"file/{managedFileId}", fileName, file);
        }
    }
}
