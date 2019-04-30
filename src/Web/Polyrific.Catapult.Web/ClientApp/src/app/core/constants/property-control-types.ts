import { PropertyControlType } from '../enums/property-control-type';

export const propertyControlTypes: [string, string][] = [
  [PropertyControlType.InputText, 'Input Text'],
  [PropertyControlType.InputNumber, 'Input Number'],
  [PropertyControlType.Select, 'Dropdown'],
  [PropertyControlType.Checkbox, 'Checkbox'],
  [PropertyControlType.CheckboxList, 'Checkbox List'],
  [PropertyControlType.Radio, 'Radio Button'],
  [PropertyControlType.Textarea, 'Textarea'],
  [PropertyControlType.Calendar, 'Calendar'],
  [PropertyControlType.Image, 'Image'],
  [PropertyControlType.InputFile, 'File'],
];
