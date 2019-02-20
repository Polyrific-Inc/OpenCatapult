export interface CreateJobTaskDefinitionDto {
    name: string;
    type: string;
    provider: string;
    configs: Map<string, string>;
    additionalConfigs: Map<string, string>;
    sequence: number;
}
