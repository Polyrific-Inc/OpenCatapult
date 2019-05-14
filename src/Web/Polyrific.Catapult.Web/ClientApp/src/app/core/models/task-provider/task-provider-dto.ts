import { AdditionalConfigDto } from '@app/core';

export interface TaskProviderDto {
    id: number;
    name: string;
    type: string;
    author: string;
    version: string;
    requiredServices: string[];
    additionalConfigs: AdditionalConfigDto[];
    tags: string[];
    created: Date;
    updated: Date;
    displayName: string;
    description: string;
    thumbnailUrl: string;
}
