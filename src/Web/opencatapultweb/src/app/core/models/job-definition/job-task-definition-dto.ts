export interface JobTaskDefinitionDto {
  id: number;
  name: string;
  jobDefinitionId: number;
  type: string;
  provider: string;
  configs: Map<string, string>;
  additionalConfigs: Map<string, string>;
  sequence: number;
}
