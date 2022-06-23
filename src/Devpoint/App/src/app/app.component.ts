import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { UserService } from './services/user.service';
import { AppService } from './services/app.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class AppComponent implements OnInit {
  constructor(private userService: UserService, private app: AppService) {}

  ngOnInit(): void {
    this.userService.populate();
    this.app.getSubscriptionLevelNames().subscribe({
      next: (data) => {
        this.app._subscriptionLevels = {
          0: 'Subscribe',
        };
        for (const { id, name } of data)
          this.app._subscriptionLevels[id] = name;

        this.app.subscriptionLevels.next(this.app._subscriptionLevels);
      },
    });
  }
}
