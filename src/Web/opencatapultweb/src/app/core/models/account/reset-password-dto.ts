export interface ResetPasswordDto {
  token: string;
  newPassword: string;
  confirmNewPassword: string;
}
