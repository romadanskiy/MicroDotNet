import { Tag } from './tag';

export enum EntityType {
  Developer,
  Project,
  Company,
}

export class Entity {
  id: string = '';
  name: string = '';
  tags: Tag[] = [];
  subscriberCount?: number = 0;
  imagePath?: string = '/assets/img/avatar.png';
  description?: string = '';
  isFollowing?: boolean = false;
  userSubscriptionLevel?: number = 1;

  get type(): EntityType {
    return 0;
  }
}
