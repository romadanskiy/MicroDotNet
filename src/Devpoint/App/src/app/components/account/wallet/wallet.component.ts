import { Component, OnInit } from '@angular/core';
import { AppService } from '../../../services/app.service';
import { PaymentEntry } from '../../../models/payment-entry';
import { Errors } from '../../../models/errors';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css'],
})
export class WalletComponent implements OnInit {
  amount: number = 0;
  deposit: number = 0;
  withdraw: number = 0;
  successMessage?: string;
  page: number = 0;

  withdrawals?: PaymentEntry[];
  deposits?: PaymentEntry[];
  bills?: PaymentEntry[];
  earnings?: PaymentEntry[];
  formattedAmount: number = 0;
  errors: Errors = { errors: {} };
  constructor(private app: AppService) {}

  ngOnInit(): void {
    this.app.getWalletAmount().subscribe((amount) => {
      this.amount = amount;
      this.changeFormattedAmount();
    });

    this.app.getAllWithdrawals().subscribe((entries) => {
      this.withdrawals = entries;
    });

    this.app.getAllDeposits().subscribe((entries) => {
      this.deposits = entries;
    });

    this.app.getAllBills().subscribe((entries) => {
      this.bills = entries;
    });

    this.app.getAllEarnings().subscribe((entries) => {
      this.earnings = entries;
    });
  }

  onWithdrawal() {
    if (this.withdraw)
      this.app.withdraw(this.withdraw).subscribe(
        (withdrawal) => {
          this.amount -= this.withdraw!;
          this.changeFormattedAmount();
          this.withdraw = 0;
          this.successMessage = 'Withdrawal request successful!';
          if (!this.withdrawals) this.withdrawals = [];
          this.withdrawals = [withdrawal, ...this.withdrawals];
        },
        (err) => this.setError(err),
      );
  }

  onDeposit() {
    if (this.deposit)
      this.app.deposit(this.deposit).subscribe(
        (deposit) => {
          this.amount += this.deposit!;
          this.changeFormattedAmount();
          this.deposit = 0;
          this.successMessage = 'Deposit request successful!';
          if (!this.deposits) this.deposits = [];
          this.deposits = [deposit, ...this.deposits];
        },
        (err) => this.setError(err),
      );
  }

  changeFormattedAmount() {
    this.formattedAmount = Number(this.amount.toFixed(2));
  }

  setError(err: any) {
    this.errors = {
      errors: {
        ...this.errors.errors,
        [err]: err,
      },
    };
  }
}
