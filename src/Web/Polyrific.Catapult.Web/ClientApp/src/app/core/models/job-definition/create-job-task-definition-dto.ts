export interface CreateJobTaskDefinitionDto {
    name: string;
    type: string;
    provider: string;
    configs: { [key: string]: string };
    additionalConfigs: { [key: string]: string };
    sequence: number;
}
