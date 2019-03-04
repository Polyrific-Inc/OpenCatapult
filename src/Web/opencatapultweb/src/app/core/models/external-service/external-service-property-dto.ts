import { AdditionalLogicDto } from './additional-logic-dto';

export interface ExternalServicePropertyDto {
  name: string;
  description: string;
  allowedValues: string[];
  isRequired: boolean;
  isSecret: boolean;
  sequence: number;
  additionalLogic: AdditionalLogicDto;
}
