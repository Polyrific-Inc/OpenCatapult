import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { UserDto } from '../models/account/user-dto';
import { SetUserRoleDto } from '../models/account/set-user-role-dto';
import { RegisterUserDto } from '../models/account/register-user-dto';
import { UpdateUserDto } from '../models/account/update-user-dto';
import { UpdatePasswordDto } from '../models/account/update-password-dto';
import { ResetPasswordDto } from '../models/account/reset-password-dto';
import { ExternalAccountTypeDto } from '../models/account/external-account-type-dto';
import { TwoFactorKeyDto } from '../models/account/two-factor-key-dto';
import { User2faInfoDto } from '../models/account/user-2fa-info-dto';
import { Generate2faRecoveryCodesDto } from '../models/account/generate-2fa-recovery-codes-dto';
import { VerifyTwoFactorCodeDto } from '../models/account/verify-two-factor-code-dto';

@Injectable()
export class AccountService {

  constructor(private api: ApiService) { }

  getUserByUserName(userName: string) {
    return this.api.get<UserDto>(`account/name/${userName}`);
  }

  getExternalAccountTypes() {
    return this.api.get<ExternalAccountTypeDto[]>('account/external-type');
  }

  getUsers(status: string, role: string) {
    return this.api.get<UserDto[]>(`account?status=${status}&role=${role}`);
  }

  activate(userId: number) {
    return this.api.postString(`account/${userId}/activate`);
  }

  suspend(userId: number) {
    return this.api.postString(`account/${userId}/suspend`);
  }

  remove(userId: number) {
    return this.api.delete(`account/${userId}`);
  }

  setUserRole(userId: number, dto: SetUserRoleDto) {
    return this.api.post(`account/${userId}/role`, dto);
  }

  register(dto: RegisterUserDto) {
    return this.api.post('account/register', dto);
  }

  updateUser(userId: number, dto: UpdateUserDto) {
    return this.api.put(`account/${userId}`, dto);
  }

  updateAvatar(userId: number, managedFileId: number) {
    return this.api.put(`account/${userId}/avatar?managedFileId=${managedFileId}`, null);
  }

  updatePassword(dto: UpdatePasswordDto) {
    return this.api.putString('account/password', dto);
  }

  requestResetPassword(username: string) {
    return this.api.getString(`account/name/${username}/resetpassword`);
  }

  resetPassword(username: string, dto: ResetPasswordDto) {
    return this.api.postString(`account/name/${username}/resetpassword`, dto);
  }

  confirmEmail(userId: number, token: string) {
    return this.api.getString(`account/${userId}/confirm?token=${token}`);
  }

  getTwoFactorAuthKey() {
    return this.api.get<TwoFactorKeyDto>('account/two-factor-key');
  }

  verifyTwoFactorAuthenticatorCode(dto: VerifyTwoFactorCodeDto) {
    return this.api.post('account/verify-two-factor-code', dto);
  }

  getUser2faInfo() {
    return this.api.get<User2faInfoDto>('account/2fa-info');
  }

  generate2faRecoveryCodes() {
    return this.api.post<Generate2faRecoveryCodesDto>('account/2fa-recovery');
  }

  resetAuthenticatorKey() {
    return this.api.put('account/reset-authenticator');
  }

  disableTwoFactor() {
    return this.api.put('account/disable-2fa');
  }
}
