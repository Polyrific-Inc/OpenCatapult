import { ExternalServicePropertyDto } from './external-service-property-dto';

export interface ExternalServiceTypeDto {
  id: number;
  name: string;
  externalServiceProperties: ExternalServicePropertyDto[];
}
