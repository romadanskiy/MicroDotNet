import { Tariff } from './tariff';
import { Entity } from './entity';

export class Subscription {
  id: number = 0;
  endTime?: Date;
  isAutoRenewal: boolean = false;
  tariff?: Tariff;
  entity?: Entity;
  entityType?: EndingType;
}
