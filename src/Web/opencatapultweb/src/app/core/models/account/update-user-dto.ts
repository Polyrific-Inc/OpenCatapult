import { ManagedFileDto } from '../managed-file/managed-file-dto';

export interface UpdateUserDto {
  id: number;
  firstName: string;
  lastName: string;
  avatarFile: ManagedFileDto;
}
