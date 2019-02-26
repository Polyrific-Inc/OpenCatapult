export interface ExternalServiceDto {
    id: number;
    name: string;
    description: string;
    externalServiceTypeId: number;
    externalServcieTypeName: string;
    Config: { [key: string]: string };
}
