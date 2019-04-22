// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Polyrific.Catapult.Api.Core.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Polyrific.Catapult.Api.Core.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <param name="firstName">First name of the user</param>
        /// <param name="lastName">Last name of the user</param>
        /// <param name="password">Password for the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>New user object</returns>
        Task<User> CreateUser(string email, string firstName, string lastName, string password, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">Edited user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateUser(User user, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task DeleteUser(int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Generate token which is used to confirm new user registration
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Confirmation token</returns>
        Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="userId">The id of user</param>
        /// /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The user entity</returns>
        Task<User> GetUserById(int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a user by email
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>the user entity</returns>
        Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Confirm registered email
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="token">Confirmation token</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get user id from claims principal
        /// </summary>
        /// <param name="principal">Claims principal</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Id of the user</returns>
        Task<int> GetUserId(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get user email from claims principal
        /// </summary>
        /// <param name="principal">Claims principal</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Email of the user</returns>
        Task<string> GetUserEmail(ClaimsPrincipal principal,
            CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Validate username + password
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="password">Password of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Validity status</returns>
        Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The user object</returns>
        Task<User> GetUser(string userName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get user list
        /// </summary>
        /// <param name="status">Status of the users</param>
        /// <param name="role">Role of the users</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<List<User>> GetUsers(string status = null, string role = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set role of a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="roleName">Name of the role</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task SetUserRole(string userId, string roleName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get role of a user
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Role name</returns>
        Task<string> GetUserRole(string userName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Suspend a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task Suspend(int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Reactivate a suspended user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task Reactivate(int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get reset password token
        /// </summary>
        /// <param name="userId">The id of user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The reset password token</returns>
        Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Reset password using the token
        /// </summary>
        /// <param name="userId">The id of user</param>
        /// <param name="token">The reset password token</param>
        /// <param name="newPassword">The new password</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Update the password by providing old password
        /// </summary>
        /// <param name="userId">The id of user</param>
        /// <param name="oldPassword">The old password of user</param>
        /// <param name="newPassword">The new password of user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Generate a password for the user
        /// </summary>
        /// <param name="length">Length of the password</param>
        /// <returns>The password</returns>
        Task<string> GeneratePassword(int length = 10);

        /// <summary>
        /// Update the user avatar
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="managedFileId">Id of the avatar file</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateAvatar(int userId, int? managedFileId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
