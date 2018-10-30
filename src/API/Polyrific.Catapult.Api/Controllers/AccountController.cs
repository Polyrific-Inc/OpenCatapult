// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private readonly INotificationProvider _notificationProvider;
        private readonly ILogger _logger;

        public AccountController(IUserService service, IMapper mapper, INotificationProvider notificationProvider, ILogger<AccountController> logger)
        {
            _userService = service;
            _mapper = mapper;
            _notificationProvider = notificationProvider;
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
            _logger.LogInformation("Registering user. Request body: {@dto}", dto);

            int userId = 0;

            try
            {
                var temporaryPassword = await _userService.GeneratePassword();
                var createdUser = await _userService.CreateUser(dto.Email, dto.FirstName, dto.LastName, temporaryPassword);
                if (createdUser != null)
                {
                    userId = createdUser.Id;

                    var token = await _userService.GenerateConfirmationToken(createdUser.Id);
                    string confirmToken = HttpUtility.UrlEncode(token);

                    // TODO: We might need to change the confirm url into the web UI url, when it's ready
                    var confirmUrl = $"{this.Request.Scheme}://{Request.Host}/account/{userId}/confirm?token={confirmToken}";
                    await _notificationProvider.SendNotification(new SendNotificationRequest
                    {
                        MessageType = NotificationConfig.RegistrationCompleted,
                        Emails = new List<string>
                        {
                            dto.Email
                        }
                    }, new Dictionary<string, string>
                    {
                        {MessageParameter.ConfirmUrl, confirmUrl},
                        {MessageParameter.TemporaryPassword, temporaryPassword}
                    });
                }
            }
            catch (UserCreationFailedException uex)
            {
                _logger.LogWarning(uex, "User creation failed");
                return BadRequest(uex.GetExceptionMessageList());
            }

            var user = await _userService.GetUserById(userId);

            var result = _mapper.Map<UserDto>(user);

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
            _logger.LogInformation("Confirming email for user {userId}", userId);

            await _userService.ConfirmEmail(userId, token);

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
            _logger.LogInformation("Getting users. Filtered by status = {status} and role = {role}", status, role);

            var users = await _userService.GetUsers(status, role);

            var results = _mapper.Map<List<UserDto>>(users);

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
            _logger.LogInformation("Getting user {userId}", userId);

            var currentUserId = User.GetUserId();
            if (currentUserId != userId && !User.IsInRole(UserRole.Administrator))
            {
                _logger.LogWarning("User {currentUserId} is not authorized to access the endpoint", currentUserId);
                return Unauthorized();
            }

            var user = await _userService.GetUserById(userId);

            var result = _mapper.Map<UserDto>(user);

            return Ok(result);
        }

        [HttpGet("currentuser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            _logger.LogInformation("Getting current user");

            var currentUserId = User.GetUserId();

            var user = await _userService.GetUserById(currentUserId);

            var result = _mapper.Map<UserDto>(user);

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
            _logger.LogInformation("Getting user {userName}", userName);

            if (User.Identity.Name.ToLower() != userName.ToLower() && !User.IsInRole(UserRole.Administrator))
            {
                _logger.LogWarning("User {Name} is not authorized to access the endpoint", User.Identity.Name);
                return Unauthorized();
            }

            var user = await _userService.GetUser(userName);

            var result = _mapper.Map<UserDto>(user);

            return Ok(result);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns>the user object</returns>
        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            _logger.LogInformation("Getting user {email}", email);

            var currentUserEmail = await _userService.GetUserEmail(User);
            if (currentUserEmail.ToLower() != email.ToLower() && !User.IsInRole(UserRole.Administrator))
            {
                _logger.LogWarning("User {currentUserEmail} is not authorized to access the endpoint", currentUserEmail);
                return Unauthorized();
            }

            var user = await _userService.GetUserByEmail(email);

            var result = _mapper.Map<UserDto>(user);

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
            _logger.LogInformation("Updating user {userId}. Request body: {@updatedUser}", userId, updatedUser);

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
            _logger.LogInformation("Suspending user {userId}", userId);

            await _userService.Suspend(userId);

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
            _logger.LogInformation("Reactivating user {userId}", userId);

            await _userService.Reactivate(userId);

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
            _logger.LogInformation("Updating current user's password");

            try
            {
                var currentUserId = User.GetUserId();

                await _userService.UpdatePassword(currentUserId, dto.OldPassword, dto.NewPassword);

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
        /// <param name="userId">Id of the user</param>
        /// <returns>The reset password token</returns>
        [HttpGet("{userId}/resetpassword")]
        public async Task<IActionResult> ResetPassword(int userId)
        {
            _logger.LogInformation("Requesting reset password token for user {userId}", userId);

            var token = await _userService.GetResetPasswordToken(userId);
            var user = await _userService.GetUserById(userId);

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

            return Ok();
        }

        /// <summary>
        /// Reset the password to a new one
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="dto">The request body for reset password</param>
        /// <returns></returns>
        [HttpPost("{userId}/resetpassword")]
        public async Task<IActionResult> ResetPassword(int userId, ResetPasswordDto dto)
        {
            _logger.LogInformation("Resetting password for user {userId}", userId);

            try
            {
                await _userService.ResetPassword(userId, dto.Token, dto.NewPassword);

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
            _logger.LogInformation("Removing user {userId}", userId);

            try
            {
                await _userService.DeleteUser(userId);
            }
            catch (UserDeletionFailedException uex)
            {
                _logger.LogWarning(uex, "User deletion failed");
                return BadRequest(uex.GetExceptionMessageList());
            }

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
            _logger.LogInformation("Setting role for user {userId}. Request body: {@dto}", userId, dto);

            if (userId != dto.UserId)
            {
                _logger.LogWarning("User Id doesn't match.");
                return BadRequest("User Id doesn't match.");
            }

            try
            {
                await _userService.SetUserRole(userId.ToString(), dto.RoleName);
            }
            catch (InvalidRoleException irEx)
            {
                _logger.LogWarning(irEx, "Invalid role");
                return BadRequest(irEx.Message);
            }

            return Ok(dto);
        }
    }
}
