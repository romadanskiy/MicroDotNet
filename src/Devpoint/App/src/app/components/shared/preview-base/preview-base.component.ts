import { Component, Input, OnInit } from '@angular/core';
import { Entity, EntityType } from 'src/app/models/entity';
import { Observable } from 'rxjs';
import { AppService } from '../../../services/app.service';
import { Router } from '@angular/router';
import { routes } from '../../../routes';

@Component({
  selector: 'app-preview-base',
  templateUrl: './preview-base.component.html',
  styleUrls: ['./preview-base.component.css'],
})
export class PreviewBaseComponent implements OnInit {
  @Input() link = '';
  imagePath?: string;
  loading: boolean = true;

  @Input() isOwned: boolean = true;
  @Input() owner?: Observable<Entity | null>;
  ownerEntity?: Entity;
  ownerLink?: string;

  private _isLarge: boolean = false;
  entityTypes = EntityType;

  subscribeButtonName?: string;

  @Input()
  set large(value: boolean | '') {
    this._isLarge = value === '' || value;
  }
  get large(): boolean {
    return this._isLarge;
  }

  @Input() entity?: Entity;

  constructor(private app: AppService, private router: Router) {}

  ngOnInit(): void {
    if (this.owner)
      this.owner.subscribe((data) => {
        if (!data) return;

        this.ownerEntity = data;
        switch (this.ownerEntity.type) {
          case EntityType.Developer:
            this.ownerLink = `/developer/${this.ownerEntity.id}`;
            break;
          case EntityType.Project:
            this.owner = undefined;
            break;
          case EntityType.Company:
            this.ownerLink = `/company/${this.ownerEntity.id}`;
            break;
        }
      });

    if (this.entity?.imagePath)
      this.imagePath = this.app.getImagePath(this.entity.imagePath);

    this.setSubscribeButtonTitle();
  }

  onLoad() {
    this.loading = false;
  }

  onFollow() {
    if (!this.entity) return;

    let request: Observable<any>;
    switch (this.entity.type) {
      case EntityType.Developer:
        request = this.app.switchDeveloperFollow(this.entity.id);
        break;
      case EntityType.Project:
        request = this.app.switchProjectFollow(this.entity.id);
        break;
      case EntityType.Company:
        request = this.app.switchCompanyFollow(this.entity.id);
        break;
    }

    request.subscribe({
      next: () => {
        if (this.entity?.subscriberCount === undefined) return;

        if (this.entity.isFollowing) this.entity.subscriberCount--;
        else this.entity.subscriberCount++;
        this.entity.isFollowing = !this.entity.isFollowing;
      },
      error: () => {
        this.router.navigateByUrl('/login');
      },
    });
  }

  setSubscribeButtonTitle() {
    if (
      !this.entity ||
      !this.entity.userSubscriptionLevel ||
      this.entity.userSubscriptionLevel <= 1
    )
      this.subscribeButtonName = 'Subscribe';
    else
      this.app
        .getSubscriptionLevelName()
        .subscribe(
          (table) =>
            (this.subscribeButtonName =
              table[this.entity!.userSubscriptionLevel!]),
        );
  }
}
