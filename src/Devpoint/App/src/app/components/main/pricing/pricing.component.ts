import { Component, OnInit } from '@angular/core';
import { Entity, EntityType } from '../../../models/entity';
import { plainToTyped } from 'type-transformer';
import { Developer } from '../../../models/developer';
import { Subscription } from '../../../models/subscription';
import { Tariff } from '../../../models/tariff';
import { MatDialog } from '@angular/material/dialog';
import { UnsubscribeConfirmModalComponent } from '../../account/subscriptions/subscription-partial/unsubscribe-confirm-modal/unsubscribe-confirm-modal.component';
import { SubscribeConfirmModalComponent } from './subscribe-confirm-modal/subscribe-confirm-modal.component';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { Errors } from '../../../models/errors';
import { AppService } from '../../../services/app.service';
import { catchError, firstValueFrom, from, switchMap, tap } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-pricing',
  templateUrl: './pricing.component.html',
  styleUrls: ['./pricing.component.css'],
})
export class PricingComponent implements OnInit {
  currentSubscription?: Subscription;
  entity?: Entity;
  entityId?: string;
  type?: EntityType;
  link?: string;
  isAutoRenewable: boolean = true;

  successMessage?: string;
  errors: Errors = { errors: {} };
  constructor(
    public dialog: MatDialog,
    private app: AppService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.route.queryParams
      .pipe(
        tap((params) => {
          this.entityId = params['id'];
          this.type = Number(params['type']);

          if (!this.entityId || this.type === undefined)
            this.router.navigateByUrl('/subscriptions');

          this.link = '/';
          switch (this.type) {
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
          this.link += `/${this.entityId}`;
        }),
        switchMap((_) => from(this.setSubscription())),
      )
      .subscribe();
  }

  async setSubscription() {
    if (!this.entityId || this.type === undefined) return;

    try {
      this.currentSubscription = await firstValueFrom(
        this.app.getSubscription(this.entityId, this.type),
      );
    } catch (e) {
      // ignore
    }

    if (this.currentSubscription)
      this.isAutoRenewable = this.currentSubscription.isAutoRenewal;

    if (
      this.currentSubscription &&
      this.currentSubscription.tariff!.subscriptionLevelId >= 5
    )
      this.router.navigateByUrl('/subscriptions');

    if (this.currentSubscription?.entity)
      this.entity = this.currentSubscription.entity;
    else {
      switch (this.type) {
        case EntityType.Project:
          this.entity = await firstValueFrom(
            this.app.getProject(this.entityId),
          );
          break;
        case EntityType.Company:
          this.entity = await firstValueFrom(
            this.app.getCompany(this.entityId),
          );
          break;
        case EntityType.Developer:
          this.entity = await firstValueFrom(
            this.app.getDeveloper(this.entityId),
          );
          break;
      }
    }

    if (!this.entity) this.router.navigateByUrl('/404');
  }

  onSubscribe(id: number) {
    const dialogRef = this.dialog.open(SubscribeConfirmModalComponent, {
      data: {
        price: this.getPrice(id),
        type:
          (this.currentSubscription?.tariff?.subscriptionLevelId ?? id) > id
            ? 'Downgrade'
            : (this.currentSubscription?.tariff?.subscriptionLevelId ?? id) < id
            ? 'Upgrade'
            : 'Subscribe',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.app
          .createSubscription(
            this.entityId!,
            id,
            this.type!,
            this.isAutoRenewable,
          )
          .subscribe({
            next: () => {
              window.location.reload();
            },
            error: (err) => {
              this.setError(err);
            },
          });
      }
    });
  }

  getPrice(id: number) {
    if ((this.currentSubscription?.tariff?.subscriptionLevelId ?? -1) < id)
      switch (id) {
        case 2:
          return 9.99;
        case 3:
          return 19.99;
        case 4:
          return 39.99;
      }
    return 0;
  }

  onAutoRenewableChange(event: MatSlideToggleChange) {
    this.isAutoRenewable = event.checked;
    if (this.currentSubscription)
      this.app
        .updateSubscription(this.currentSubscription.id, this.isAutoRenewable)
        .subscribe({
          error: (err) => this.setError(err),
        });
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
