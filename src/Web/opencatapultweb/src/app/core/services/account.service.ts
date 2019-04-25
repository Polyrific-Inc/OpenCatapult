import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { UserDto } from '../models/account/user-dto';
import { SetUserRoleDto } from '../models/account/set-user-role-dto';
import { RegisterUserDto } from '../models/account/register-user-dto';
import { UpdateUserDto } from '../models/account/update-user-dto';
import { UpdatePasswordDto } from '../models/account/update-password-dto';
import { ResetPasswordDto } from '../models/account/reset-password-dto';
import { ExternalAccountTypeDto } from '../models/account/external-account-type-dto';

@Injectable()
export class AccountService {

  constructor(private api: ApiService) { }

  getUserByEmail(email: string) {
    return this.api.get<UserDto>(`account/email/${email}`);
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

  requestResetPassword(email: string) {
    return this.api.getString(`account/email/${email}/resetpassword`);
  }

  resetPassword(email: string, dto: ResetPasswordDto) {
    return this.api.postString(`account/email/${email}/resetpassword`, dto);
  }

}
