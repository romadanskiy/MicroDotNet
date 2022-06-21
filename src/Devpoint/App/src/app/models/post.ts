import { Tag } from './tag';

export class Post {
  id?: string = '';
  title?: string = '';
  content?: string = '';
  date?: string = '';
  imagePath?: string = '';
  hasUserAccess: boolean = true;
  requiredSubscriptionLevel?: number = 0;
  tags?: Tag[] = [];
}
