import { Entity, EntityType } from './entity';

export class Company extends Entity {
  override get type(): EntityType {
    return EntityType.Company;
  }

  ownerId: string = '';
}
