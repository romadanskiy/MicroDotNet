import { Developer } from './developer';

export type PaymentEntry = {
  id: number;
  amount: number;
  dateTime: Date;
  status?: PaymentStatus;
  subscriptionType?: string;
  from?: Developer;
};

export enum PaymentStatus {
  Success,
  Failed,
}
