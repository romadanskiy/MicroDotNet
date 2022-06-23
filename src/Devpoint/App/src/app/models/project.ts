import { Tag } from './tag';
import { Entity, EntityType } from './entity';

export class Project extends Entity {
  override get type(): EntityType {
    return EntityType.Project;
  }

  ownerId: string = '';
  companyId?: string;
}
