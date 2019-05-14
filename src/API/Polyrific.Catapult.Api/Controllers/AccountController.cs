// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Exceptions;
using Polyrific.Catapult.Api.Core.Services;
using Polyrific.Catapult.Api.Identity;
using Polyrific.Catapult.Shared.Common;
using Polyrific.Catapult.Shared.Common.Notification;
using Polyrific.Catapult.Shared.Dto.Constants;
using Polyrific.Catapult.Shared.Dto.User;

namespace Polyrific.Catapult.Api.Controllers
{
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IExternalAccountTypeService _externalAccountTypeService;
        private readonly IMapper _mapper;
        private readonly INotificationProvider _notificationProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AccountController(
            IUserService service,
            IExternalAccountTypeService externalAccountTypeService,
            IMapper mapper, 
            INotificationProvider notificationProvider,
            IConfiguration configuration,
            ILogger<AccountController> logger)
        {
            _userService = service;
            _externalAccountTypeService = externalAccountTypeService;
            _mapper = mapper;
            _notificationProvider = notificationProvider;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Register the user
        /// </summary>
        /// <param name="dto">The register request body</param>
        /// <returns>The user id and confirmation token</returns>
        [HttpPost("Register")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> RegisterUser(RegisterUserDto dto)
        {
            _logger.LogRequest("Registering user. Request body: {@dto}", dto);

            User user = null;

            try
            {
                var temporaryPassword = await _userService.GeneratePassword();
                user = await _userService.CreateUser(dto.Email, dto.FirstName, dto.LastName, dto.RoleName, dto.ExternalAccountIds, temporaryPassword, _configuration[ConfigurationKey.WebUrl]);                
            }
            catch (UserCreationFailedException uex)
            {
                _logger.LogWarning(uex, "User creation failed");
                return BadRequest(uex.GetExceptionMessageList());
            }

            user = await _userService.GetUserById(user.Id);

            var result = _mapper.Map<UserDto>(user);

            _logger.LogResponse("User registered. Response body: {@result}", result);

            return Ok(result);
        }

        /// <summary>
        /// Confirm the email using the token
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="token">The confirmation token</param>
        /// <returns></returns>
        [HttpGet("{userId}/Confirm")]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            _logger.LogRequest("Confirming email for user {userId}", userId);

            await _userService.ConfirmEmail(userId, token);

            _logger.LogResponse("Email confirmed");

            return Ok("Email confirmed.");
        }

        /// <summary>
        /// Get all users, optionally filterred it by status
        /// </summary>
        /// <param name="status">Status filter [active | suspended]</param>
        /// <param name="role">Role of the users [Administrator | Basic | Guest]</param>
        /// <returns>The list of user</returns>
        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> GetUsers(string status, string role)
        {
            _logger.LogRequest("Getting users. Filtered by status = {status} and role = {role}", status, role);

            var users = await _userService.GetUsers(status, role);

            var results = _mapper.Map<List<UserDto>>(users);

            _logger.LogResponse("Users retrieved. Reponse body: {@results}", results);

            return Ok(results);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>The user object</returns>
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int userId)
        {
            _logger.LogRequest("Getting user {userId}", userId);

            var currentUserId = User.GetUserId();
            if (currentUserId != userId && !User.IsInRole(UserRole.Administrator))
            {
                _logger.LogWarning("User {currentUserId} is not authorized to access the endpoint", currentUserId);
                return Unauthorized();
            }

            var user = await _userService.GetUserById(userId);

            var result = _mapper.Map<UserDto>(user);

            _logger.LogResponse("User {userId} retrieved. Reponse body: {@result}", userId, result);

            return Ok(result);
        }

        [HttpGet("currentuser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            _logger.LogRequest("Getting current user");

            var currentUserId = User.GetUserId();

            var user = await _userService.GetUserById(currentUserId);

            var result = _mapper.Map<UserDto>(user);

            _logger.LogResponse("Current user retrieved. Reponse body: {@result}", result);

            return Ok(result);
        }

        /// <summary>
        /// Get user by userName
        /// </summary>
        /// <param name="userName">userName of the user</param>
        /// <returns>The user object </returns>
        [HttpGet("name/{userName}")]
        [Authorize]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            _logger.LogRequest("Getting user {userName}", userName);

            if (User.Identity.Name.ToLower() != userName.ToLower() && !User.IsInRole(UserRole.Administrator))
            {
                _logger.LogWarning("User {Name} is not authorized to access the endpoint", User.Identity.Name);
                return Unauthorized();
            }

            var user = await _userService.GetUser(userName);

            var result = _mapper.Map<UserDto>(user);

            _logger.LogResponse("User {userName} retrieved. Reponse body: {@result}", userName, result);

            return Ok(result);
        }

        /// <summary>
        /// Update the user profile
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="updatedUser">The request body for the updated user</param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDto updatedUser)
        {
            try
            {
                _logger.LogRequest("Updating user {userId}. Request body: {@updatedUser}", userId, updatedUser);

                var currentUserId = User.GetUserId();
                if (currentUserId != userId && !User.IsInRole(UserRole.Administrator))
                {
                    _logger.LogWarning("User {currentUserId} is not authorized to access the endpoint", currentUserId);
                    return Unauthorized();
                }

                if (userId != updatedUser.Id)
                {
                    _logger.LogWarning("User Id doesn't match.");
                    return BadRequest("User Id doesn't match.");
                }

                var user = _mapper.Map<User>(updatedUser);

                await _userService.UpdateUser(user);

                _logger.LogResponse("User {userId} updated", userId);

                return Ok();
            }
            catch (DuplicateUserNameException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update the user avatar
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="managedFileId">Id of the avatar file</param>
        /// <returns></returns>
        [HttpPut("{userId}/avatar")]
        [Authorize]
        public async Task<IActionResult> UpdateAvatar(int userId, int? managedFileId)
        {
            _logger.LogRequest("Updating user {userId} avatar into file {managedFileId}", userId, managedFileId);

            var currentUserId = User.GetUserId();
            if (currentUserId != userId && !User.IsInRole(UserRole.Administrator))
            {
                _logger.LogWarning("User {currentUserId} is not authorized to access the endpoint", currentUserId);
                return Unauthorized();
            }
            
            await _userService.UpdateAvatar(userId, managedFileId);

            _logger.LogResponse("Avatar for User {userId} updated", userId);

            return Ok();
        }

        /// <summary>
        /// Suspend a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        [HttpPost("{userId}/suspend")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> SuspendUser(int userId)
        {
            _logger.LogRequest("Suspending user {userId}", userId);

            await _userService.Suspend(userId);

            _logger.LogResponse("User {userId} suspended", userId);

            return Ok("User suspended");
        }

        /// <summary>
        /// Reactivate a suspended user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        [HttpPost("{userId}/activate")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> ReactivateUser(int userId)
        {
            _logger.LogRequest("Reactivating user {userId}", userId);

            await _userService.Reactivate(userId);

            _logger.LogResponse("User {userId} activated", userId);

            return Ok("User activated");
        }

        /// <summary>
        /// Update user's password
        /// </summary>
        /// <param name="dto">The request body for update password</param>
        /// <returns></returns>
        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto dto)
        {
            _logger.LogRequest("Updating current user ({Name}) password.", User.Identity.Name);

            try
            {
                var currentUserId = User.GetUserId();

                await _userService.UpdatePassword(currentUserId, dto.OldPassword, dto.NewPassword);

                _logger.LogResponse("current user ({Name}) password updated");

                return Ok("Password updated");
            }
            catch (UpdatePasswordFailedException uex)
            {
                _logger.LogWarning(uex, "Update password failed");
                return BadRequest(uex.GetExceptionMessageList());
            }
        }

        /// <summary>
        /// Request reset password token
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <returns>The reset password token</returns>
        [HttpGet("name/{username}/resetpassword")]
        public async Task<IActionResult> ResetPassword(string username)
        {
            _logger.LogRequest("Requesting reset password token for user {username}", username);

            var user = await _userService.GetUser(username);

            if (user == null)
            {
                _logger.LogWarning("User {username} was not found.", username);
                return Ok();
            }                

            var token = await _userService.GetResetPasswordToken(user.Id);

            var originUrl = Request.Headers["Origin"];

            if (!string.IsNullOrEmpty(originUrl))
            {

                await _notificationProvider.SendNotification(new SendNotificationRequest
                {
                    MessageType = NotificationConfig.ResetPasswordWeb,
                    Emails = new List<string>
                        {
                            user.Email
                        }
                }, new Dictionary<string, string>
                    {
                        {MessageParameter.ResetPasswordLink, $"{originUrl}/reset-password?username={username}&token={HttpUtility.UrlEncode(token)}"}
                    });
            }
            else
            {
                await _notificationProvider.SendNotification(new SendNotificationRequest
                {
                    MessageType = NotificationConfig.ResetPassword,
                    Emails = new List<string>
                        {
                            user.Email
                        }
                }, new Dictionary<string, string>
                    {
                        {MessageParameter.ResetPasswordToken, token}
                    });
            }

            _logger.LogResponse("Password reset notification for user {username} sent", username);

            return Ok();
        }

        /// <summary>
        /// Reset the password to a new one
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="dto">The request body for reset password</param>
        /// <returns></returns>
        [HttpPost("name/{username}/resetpassword")]
        public async Task<IActionResult> ResetPassword(string username, ResetPasswordDto dto)
        {
            _logger.LogRequest("Resetting password for user {username}", username);

            try
            {
                var user = await _userService.GetUser(username);
                if (user == null)
                {
                    _logger.LogWarning("User {username} was not found.", username);
                    return BadRequest("Reset password failed");
                }

                await _userService.ResetPassword(user.Id, dto.Token, dto.NewPassword);

                _logger.LogResponse("Password for user {username} reset", username);

                return Ok("Reset password success");
            }
            catch (ResetPasswordFailedException rex)
            {
                _logger.LogWarning(rex, "Reset password failed");
                return BadRequest(rex.GetExceptionMessageList());
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            _logger.LogRequest("Removing user {userId}", userId);

            try
            {
                await _userService.DeleteUser(userId);
            }
            catch (UserDeletionFailedException uex)
            {
                _logger.LogWarning(uex, "User deletion failed");
                return BadRequest(uex.GetExceptionMessageList());
            }

            _logger.LogResponse("User {userId} removed", userId);

            return NoContent();
        }

        /// <summary>
        /// Set the role of a user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="dto">The request body for setting user role</param>
        /// <returns></returns>
        [HttpPost("{userId}/role")]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> SetUserRole(int userId, SetUserRoleDto dto)
        {
            _logger.LogRequest("Setting role for user {userId}. Request body: {@dto}", userId, dto);

            if (userId != dto.UserId)
            {
                _logger.LogWarning("User Id doesn't match.");
                return BadRequest("User Id doesn't match.");
            }

            try
            {
                await _userService.SetUserRole(userId, dto.RoleName);
            }
            catch (InvalidRoleException irEx)
            {
                _logger.LogWarning(irEx, "Invalid role");
                return BadRequest(irEx.Message);
            }

            _logger.LogResponse("User {userId} role set. Response body: {@dto}", userId, dto);

            return Ok(dto);
        }

        /// <summary>
        /// Get the list of external account types
        /// </summary>
        /// <returns></returns>
        [HttpGet("external-type")]
        [Authorize]
        public async Task<IActionResult> GetExternalAccountTypes()
        {
            _logger.LogRequest("Getting the list of external account types");

            var entities = await _externalAccountTypeService.GetExternalAccountTypes();

            var results = _mapper.Map<List<ExternalAccountTypeDto>>(entities);

            _logger.LogResponse("List of external account types retrieved. Response body: {@dto}", results);

            return Ok(results);
        }
    }
}
