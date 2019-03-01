import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { UserDto } from '../models/account/user-dto';

@Injectable()
export class AccountService {

  constructor(private api: ApiService) { }

  getUserByEmail(email: string) {
    return this.api.get<UserDto>(`account/email/${email}`);
  }
}
