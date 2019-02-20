import { AdditionalConfigDto } from '@app/core';

export interface TaskProviderDto {
    id: number;
    name: string;
    type: string;
    author: string;
    version: string;
    registrationDate: Date;
    requiredServices: string[];
    additionalConfigs: AdditionalConfigDto[];
}
