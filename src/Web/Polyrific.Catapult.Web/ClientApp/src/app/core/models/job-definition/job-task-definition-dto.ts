export interface JobTaskDefinitionDto {
  id: number;
  name: string;
  jobDefinitionId: number;
  type: string;
  provider: string;
  configs: { [key: string]: string };
  additionalConfigs: { [key: string]: string };
  sequence: number;
  valid: boolean;
  validationError: string;
}
