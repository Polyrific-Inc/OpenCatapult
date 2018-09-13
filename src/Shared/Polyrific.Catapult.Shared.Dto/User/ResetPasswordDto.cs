// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.User
{
    public class ResetPasswordDto
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Reset password token
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// New password of the user
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirm new password for the user
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The new password and confirmation new password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}