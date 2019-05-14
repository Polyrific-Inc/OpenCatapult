export interface ExternalServiceDto {
    id: number;
    name: string;
    description: string;
    externalServiceTypeId: number;
    externalServiceTypeName: string;
    config: { [key: string]: string };
    isGlobal: boolean;
}
