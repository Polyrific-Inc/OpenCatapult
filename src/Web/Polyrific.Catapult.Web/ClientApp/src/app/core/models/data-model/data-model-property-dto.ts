export interface DataModelPropertyDto {
  id: number;
  projectDataModelId: number;
  name: string;
  label: string;
  dataType: string;
  isRequired: boolean;
  controlType: string;
  relatedProjectDataModelId: number;
  relatedProjectDataModelName: string;
  relationalType: string;
  isManaged: string;
}
