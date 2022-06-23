import { Component, OnInit } from '@angular/core';
import { Subscription } from '../../../models/subscription';
import { AppService } from '../../../services/app.service';
import { forkJoin, of } from 'rxjs';
import { Errors } from '../../../models/errors';

@Component({
  selector: 'app-subscriptions',
  templateUrl: './subscriptions.component.html',
  styleUrls: ['./subscriptions.component.css'],
})
export class SubscriptionsComponent implements OnInit {
  developerSubscriptions: Subscription[] = [];
  projectSubscriptions: Subscription[] = [];
  companiesSubscriptions: Subscription[] = [];

  searchingDevelopers: boolean = true;
  searchingProjects: boolean = true;
  searchingCompanies: boolean = true;
  successMessage?: string;
  errors: Errors = { errors: {} };

  constructor(private app: AppService) {}

  ngOnInit(): void {
    this.onSearchChange();
  }

  onSearchChange() {
    forkJoin([
      this.searchingDevelopers
        ? this.app.getDeveloperSubscriptions()
        : of(null),
      this.searchingProjects ? this.app.getProjectSubscriptions() : of(null),
      this.searchingCompanies ? this.app.getCompanySubscriptions() : of(null),
    ]).subscribe(([developers, projects, companies]) => {
      this.developerSubscriptions = developers ?? [];
      this.projectSubscriptions = projects ?? [];
      this.companiesSubscriptions = companies ?? [];
    });
  }

  toggleDevelopersSearch() {
    this.searchingDevelopers = !this.searchingDevelopers;
    this.onSearchChange();
  }

  toggleProjectsSearch() {
    this.searchingProjects = !this.searchingProjects;
    this.onSearchChange();
  }

  toggleCompaniesSearch() {
    this.searchingCompanies = !this.searchingCompanies;
    this.onSearchChange();
  }

  onCancelError(err: string) {
    this.errors = { errors: { [err]: err } };
  }

  onCancelSuccess() {
    this.errors = { errors: {} };
    this.successMessage = 'Subscription canceled successfully';
    this.onSearchChange();
  }
}
