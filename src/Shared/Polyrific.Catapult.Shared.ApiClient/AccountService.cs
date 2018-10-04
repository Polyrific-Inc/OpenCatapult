// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Polyrific.Catapult.Shared.Dto.User;
using Polyrific.Catapult.Shared.Service;

namespace Polyrific.Catapult.Shared.ApiClient
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(IApiClient api) : base(api)
        {
        }

        public async Task<string> ConfirmEmail(int userId, string token)
        {
            var path = $"account/{userId}/confirm";

            return await Api.Get<string>(path);
        }

        public async Task<UserDto> GetUser(int userId)
        {
            var path = $"account/{userId}";

            return await Api.Get<UserDto>(path);
        }

        public async Task<UserDto> GetCurrentUser()
        {
            var path = $"account/currentuser";

            return await Api.Get<UserDto>(path);
        }

        public async Task<UserDto> GetUserByUserName(string userName)
        {
            var path = $"account/name/{userName}";

            return await Api.Get<UserDto>(path);
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var path = $"account/email/{email}";

            return await Api.Get<UserDto>(path);
        }

        public async Task<List<UserDto>> GetUsers(string status)
        {
            var path = "account";

            return await Api.Get<List<UserDto>>(path);
        }

        public async Task<RegisterUserResultDto> RegisterUser(RegisterUserDto dto)
        {
            var path = "account/register";

            return await Api.Post<RegisterUserDto, RegisterUserResultDto>(path, dto);
        }

        public async Task RemoveUser(int userId)
        {
            var path = $"account/{userId}";

            await Api.Delete(path);
        }

        public async Task SetUserRole(int userId, SetUserRoleDto dto)
        {
            var path = $"account/{userId}/role";

            await Api.Post<SetUserRoleDto, SetUserRoleDto>(path, dto);
        }

        public async Task SuspendUser(int userId)
        {
            var path = $"account/{userId}/suspend";

            await Api.Post<object>(path, null);
        }

        public async Task ReactivateUser(int userId)
        {
            var path = $"account/{userId}/activate";

            await Api.Post<object>(path, null);
        }

        public async Task UpdateUser(int userId, UpdateUserDto dto)
        {
            var path = $"account/{userId}";

            await Api.Put(path, dto);
        }

        public async Task UpdatePassword(int userId, UpdatePasswordDto dto)
        {
            var path = $"account/{userId}/password";

            await Api.Post(path, dto);
        }
    }
}
