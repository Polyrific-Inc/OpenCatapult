export interface CreateExternalServiceDto {
  name: string;
  description: string;
  externalServiceTypeId: number;
  config: { [key: string]: string };
}
