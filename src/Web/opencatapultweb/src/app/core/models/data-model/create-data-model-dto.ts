import { CreateDataModelPropertyDto } from './create-data-model-property-dto';

export interface CreateDataModelDto {
    name: string;
    description: string;
    label: string;
    isManaged: boolean;
    selectKey: string;
    properties: CreateDataModelPropertyDto[];
}
