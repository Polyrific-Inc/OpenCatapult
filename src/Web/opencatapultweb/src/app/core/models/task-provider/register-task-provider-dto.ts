import { AdditionalConfigDto } from '@app/core';

export interface RegisterTaskProviderDto {
  name: string;
  type: string;
  author: string;
  version: string;
  registrationDate: Date;
  requiredServices: string[];
  additionalConfigs: AdditionalConfigDto[];
}
