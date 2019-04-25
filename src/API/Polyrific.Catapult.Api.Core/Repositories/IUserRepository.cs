// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The user object</returns>
        Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>the user object</returns>
        Task<User> GetByEmail(string email, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create a new user with password
        /// </summary>
        /// <param name="entity">New user</param>
        /// <param name="password">The password</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Id of the new created user</returns>
        Task<int> Create(User entity, string password, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Generate token which is used to confirm registered email
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Confirmation token</returns>
        Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Confirm a registered user's email
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="token">Confirmation token</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get a user from claims principal
        /// </summary>
        /// <param name="principal">Claims principal</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The user</returns>
        Task<User> GetByPrincipal(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Validate username + password
        /// </summary>
        /// <param name="userName">Username of the user</param>
        /// <param name="password">Password of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>Validity status</returns>
        Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Set user role
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
        /// Get the list of users
        /// </summary>
        /// <param name="isActive">Indicates whether user is active</param>
        /// <param name="role">Role of the user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns>The list of user</returns>
        Task<List<User>> GetUsers(bool? isActive, string role, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get the list of users by ids
        /// </summary>
        /// <param name="ids">Array of user ids</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task<List<User>> GetUsersByIds(int[] ids, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Suspend the user
        /// </summary>
        /// <param name="userId">The id of user</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task Suspend(int userId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Reactivate a suspended user
        /// </summary>
        /// <param name="userId">The id of user</param>
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
        /// Update the user avatar
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="managedFileId">Id of the avatar file</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled</param>
        /// <returns></returns>
        Task UpdateAvatar(int userId, int? managedFileId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
