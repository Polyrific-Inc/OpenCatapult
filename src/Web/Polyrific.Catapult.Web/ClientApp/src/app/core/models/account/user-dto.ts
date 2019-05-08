export interface UserDto {
  id: number;
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean;
  emailConfirmed: boolean;
  role: string;
  avatarFileId: number;
  externalAccountIds: { [key: string]: string };
}
