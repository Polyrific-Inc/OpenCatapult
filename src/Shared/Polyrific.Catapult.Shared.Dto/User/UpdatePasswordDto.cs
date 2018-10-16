// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Polyrific.Catapult.Shared.Dto.User
{
    public class UpdatePasswordDto
    {
        /// <summary>
        /// Old password of the user
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

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
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation new password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
