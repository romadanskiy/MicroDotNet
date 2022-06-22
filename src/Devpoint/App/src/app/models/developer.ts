import { Entity, EntityType } from './entity';
import { Tag } from './tag';

export class Developer extends Entity {
  override get type(): EntityType {
    return EntityType.Developer;
  }

  email: string = '';
}
