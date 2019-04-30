import { PropertyRelationalType } from '../enums/property-relational-type';

export const propertyRelationalTypes: [string, string][] = [
  [PropertyRelationalType.OneToOne, 'One to one'],
  [PropertyRelationalType.OneToMany, 'One to many'],
  [PropertyRelationalType.ManyToMany, 'Many to many']
];
