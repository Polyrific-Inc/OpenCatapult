export interface User2faInfoDto {
  is2faEnabled: boolean;
  recoveryCodesLeft: number;
  hasAuthenticator: boolean;
}
