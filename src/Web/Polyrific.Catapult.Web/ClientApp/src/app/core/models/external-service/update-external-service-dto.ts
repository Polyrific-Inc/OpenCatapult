export interface UpdateExternalServiceDto {
  description: string;
  config: { [key: string]: string };
  isGlobal: boolean;
}
