import { DataModelPropertyDto } from './data-model-property-dto';

export interface DataModelDto {
  id: number;
  projectId: number;
  name: string;
  description: string;
  label: string;
  isManaged: boolean;
  selectKey: string;
  properties: DataModelPropertyDto[];
}
