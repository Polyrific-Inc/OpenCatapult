export interface CreateDataModelPropertyDto {
    name: string;
    label: string;
    dataType: string;
    controlType: string;
    relatedProjectDataModelId: number;
    relatedProjectDataModelName: string;
    relationalType: string;
    isRequired: boolean;
    isManaged: boolean;
}
