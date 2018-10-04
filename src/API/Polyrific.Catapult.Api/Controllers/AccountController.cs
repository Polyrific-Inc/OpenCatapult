// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly NotificationProvider _notificationProvider;

        public AccountController(IUserService service, IMapper mapper, NotificationProvider notificationProvider)
        {
            _userService = service;
            _mapper = mapper;
            _notificationProvider = notificationProvider;
        }

        /// <summary>
        /// Register the user
        /// </summary>
        /// <param name="dto">The register request body</param>
        /// <returns>The user id and confirmation token</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto dto)
        {
            int userId = 0;

            try
            {
                var createdUser = await _userService.CreateUser(dto.Email, dto.FirstName, dto.LastName, dto.Password);
                if (createdUser != null)
                {
                    userId = createdUser.Id;

                    var token = await _userService.GenerateConfirmationToken(createdUser.Id);
                    string confirmToken = HttpUtility.UrlEncode(token);

                    // TODO: We might need to change the confirm url into the web UI url, when it's ready
                    var confirmUrl = $"{this.Request.Scheme}://{Request.Host}/account/{userId}/confirm?token={confirmToken}";
                    _notificationProvider.SendNotification(new SendNotificationRequest
                    {
                        MessageType = NotificationConfig.RegistrationCompleted,
                        Emails = new List<string>
                        {
                            dto.Email
                        }
                    }, new Dictionary<string, string>
                    {
                        {MessageParameter.ConfirmUrl, confirmUrl}
                    });
                }
            }
            catch (UserCreationFailedException uex)
            {
                return BadRequest(uex.GetExceptionMessageList());
            }

            return Ok(new RegisterUserResultDto
            {
                UserId = userId
            });
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
            await _userService.ConfirmEmail(userId, token);

            return Ok("Email confirmed.");
        }

        /// <summary>
        /// Get all users, optionally filterred it by status
        /// </summary>
        /// <param name="status">Status filter [active | suspended]</param>
        /// <returns>The list of user</returns>
        [HttpGet]
        [Authorize(Policy = AuthorizePolicy.UserRoleAdminAccess)]
        public async Task<IActionResult> GetUsers(string status)
        {
            var users = await _userService.GetUsers(status);

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
            var currentUserId = await _userService.GetUserId(User);
            if (currentUserId != userId && !User.IsInRole(UserRole.Administrator))
            {
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
            var currentUserId = await _userService.GetUserId(User);

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
            if (User.Identity.Name.ToLower() != userName.ToLower() && !User.IsInRole(UserRole.Administrator))
            {
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
            var currentUserEmail = await _userService.GetUserEmail(User);
            if (currentUserEmail.ToLower() != email.ToLower() && !User.IsInRole(UserRole.Administrator))
            {
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
            var currentUserId = await _userService.GetUserId(User);
            if (currentUserId != userId && !User.IsInRole(UserRole.Administrator))
            {
                return Unauthorized();
            }

            if (userId != updatedUser.Id)
                return BadRequest("User Id doesn't match.");

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
            await _userService.Reactivate(userId);

            return Ok("User activated");
        }

        /// <summary>
        /// Update user's password
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="dto">The request body for update password</param>
        /// <returns></returns>
        [HttpPost("{userId}/password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(int userId, UpdatePasswordDto dto)
        {
            try
            {
                var currentUserId = await _userService.GetUserId(User);
                if (currentUserId != userId && !User.IsInRole(UserRole.Administrator))
                {
                    return Unauthorized();
                }

                await _userService.UpdatePassword(userId, dto.OldPassword, dto.NewPassword);

                return Ok("Password updated");
            }
            catch (UpdatePasswordFailedException uex)
            {
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
            var token = await _userService.GetResetPasswordToken(userId);

            return Ok(new ResetPasswordTokenDto
            {
                Token = token
            });
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
            try
            {
                await _userService.ResetPassword(userId, dto.Token, dto.NewPassword);

                return Ok("Reset password success");
            }
            catch (ResetPasswordFailedException rex)
            {
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
            try
            {
                await _userService.DeleteUser(userId);
            }
            catch (UserDeletionFailedException uex)
            {
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
            if (userId != dto.UserId)
                return BadRequest("User Id doesn't match.");

            try
            {
                await _userService.SetUserRole(userId.ToString(), dto.RoleName);
            }
            catch (InvalidRoleException irEx)
            {
                return BadRequest(irEx.Message);
            }

            return Ok(dto);
        }
    }
}
