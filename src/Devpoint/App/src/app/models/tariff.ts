import { EntityType } from './entity';
import { SubscriptionLevel } from './subscription-level';

export class Tariff {
  id: number = 0;
  pricePerMonth: number = 0;
  subscriptionType: EntityType = 0;
  subscriptionLevelId: number = 0;
}
