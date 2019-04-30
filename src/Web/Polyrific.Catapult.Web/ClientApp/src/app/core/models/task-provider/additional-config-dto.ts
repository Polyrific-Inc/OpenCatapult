export interface AdditionalConfigDto {
    name: string;
    label: string;
    type: string;
    isRequired: boolean;
    isSecret: boolean;
    isInputMasked: boolean;
    allowedValues: string[];
    hint: string;
}
