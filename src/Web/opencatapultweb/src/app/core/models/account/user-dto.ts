import { ManagedFileDto } from '../managed-file/managed-file-dto';

export interface UserDto {
  id: number;
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean;
  role: string;
  avatarFile: ManagedFileDto;
}
