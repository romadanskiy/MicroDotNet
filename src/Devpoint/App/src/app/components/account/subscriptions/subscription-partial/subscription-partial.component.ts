import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Subscription } from '../../../../models/subscription';
import { EntityType } from '../../../../models/entity';
import * as moment from 'moment';
import { MatDialog } from '@angular/material/dialog';
import { UnsubscribeConfirmModalComponent } from './unsubscribe-confirm-modal/unsubscribe-confirm-modal.component';
import { AppService } from '../../../../services/app.service';

@Component({
  selector: 'app-subscription-partial',
  templateUrl: './subscription-partial.component.html',
  styleUrls: ['./subscription-partial.component.css'],
})
export class SubscriptionPartialComponent implements OnInit {
  @Input() subscription?: Subscription;
  link?: string;
  entityTypes = EntityType;
  endTime?: string;
  loading: boolean = true;
  imagePath?: string;
  subscriptionLevelName?: string;
  @Output() cancelError = new EventEmitter<string>();
  @Output() cancelSuccess = new EventEmitter<Subscription>();
  constructor(public dialog: MatDialog, private app: AppService) {}

  ngOnInit(): void {
    this.link = '/';
    switch (this.subscription?.entity?.type) {
      case EntityType.Project:
        this.link += 'project';
        break;
      case EntityType.Company:
        this.link += 'company';
        break;
      case EntityType.Developer:
        this.link += 'developer';
        break;
    }
    this.link += `/${this.subscription?.entity?.id ?? 0}`;

    this.endTime = moment(this.subscription?.endTime).format('DD.MM.YYYY');
    if (this.subscription?.entity?.imagePath)
      this.imagePath = this.app.getImagePath(
        this.subscription.entity.imagePath,
      );

    if (this.subscription?.tariff)
      this.app
        .getSubscriptionLevelName()
        .subscribe(
          (table) =>
            (this.subscriptionLevelName =
              table[this.subscription?.tariff?.subscriptionLevelId!]),
        );
  }

  openDialog() {
    const dialogRef = this.dialog.open(UnsubscribeConfirmModalComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result && this.subscription)
        this.app.cancelSubscription(this.subscription.id).subscribe({
          next: () => this.cancelSuccess.emit(this.subscription),
          error: (err) => this.cancelError.emit(err),
        });
    });
  }

  onLoad() {
    this.loading = false;
  }
}
