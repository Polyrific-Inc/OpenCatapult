// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Shared.Dto.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Shared.Service
{
    public interface IAccountService
    {
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="dto">DTO containing new user details</param>
        /// <returns>DTO containing registration result</returns>
        Task<RegisterUserResultDto> RegisterUser(RegisterUserDto dto);

        /// <summary>
        /// Confirm user email which was used for registration
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="token">Token code which was generated after registration</param>
        /// <returns>Confirmation message</returns>
        Task<string> ConfirmEmail(int userId, string token);

        /// <summary>
        /// Get list of users by their status
        /// </summary>
        /// <param name="status">Status of the user</param>
        /// <returns>List of users</returns>
        Task<List<UserDto>> GetUsers(string status);

        /// <summary>
        /// Get a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>User object</returns>
        Task<UserDto> GetUser(int userId);

        /// <summary>
        /// Get a user by userName
        /// </summary>
        /// <param name="userName">userName of the user</param>
        /// <returns>User object</returns>
        Task<UserDto> GetUserByUserName(string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserDto> GetUserByEmail(string email);

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="dto">DTO containing updated user details</param>
        /// <returns></returns>
        Task UpdateUser(int userId, UpdateUserDto dto);

        /// <summary>
        /// Suspend a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        Task SuspendUser(int userId);

        /// <summary>
        /// Reactivate a suspended user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task ReactivateUser(int userId);

        /// <summary>
        /// Remove a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        Task RemoveUser(int userId);

        /// <summary>
        /// Set role of a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="dto">DTO containing required details</param>
        /// <returns></returns>
        Task SetUserRole(int userId, SetUserRoleDto dto);
    }
}