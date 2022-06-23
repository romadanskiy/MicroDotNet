export type PaymentEntry = {
  id: number;
  amount: number;
  dateTime: Date;
  status?: PaymentStatus;
  subscriptionType?: string;
};

export enum PaymentStatus {
  Success,
  Failed,
}
