import { Component, OnInit } from '@angular/core';
import { Developer } from 'src/app/models/developer';
import { Project } from 'src/app/models/project';
import { PageEvent } from '@angular/material/paginator';
import {
  EMPTY,
  firstValueFrom,
  forkJoin,
  map,
  Observable,
  of,
  startWith,
  take,
} from 'rxjs';
import { FormControl } from '@angular/forms';
import { plainToTyped } from 'type-transformer';
import { Company } from '../../models/company';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AppService } from '../../services/app.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent implements OnInit {
  search?: string;

  control = new FormControl();
  developerNames: string[] = [];
  projectNames: string[] = [];
  comapanyNames: string[] = [];
  names: string[] = [];
  filteredNames?: Observable<string[]>;

  developers: Developer[] = [];
  projects: Project[] = [];
  companies: Company[] = [];

  searchingDevelopers: boolean = true;
  searchingProjects: boolean = true;
  searchingCompanies: boolean = true;

  totalDevCount: number = 0;
  totalProjectCount: number = 0;
  totalCompanyCount: number = 0;
  totalCount: number = 0;
  take: number = 4;
  page: number = 0;

  follow: boolean = false;

  pageSize: number = 4 * 3;
  pageOptions = [4 * 3, 8 * 3, 16 * 3];

  currentUser?: Developer;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private app: AppService,
  ) {}

  ngOnInit(): void {
    this.userService.currentUser.subscribe((userData) => {
      this.currentUser = userData;
    });
    this.fetchNames().then(() => {
      this.changeNames();
    });
    this.filteredNames = this.control.valueChanges.pipe(
      startWith(''),
      map((value) => this._filter(value)),
    );

    this.route.queryParams.subscribe((params) => {
      this.follow = params['follow'] ?? false;
      this.onSearchChange();
    });
  }

  private async fetchNames() {
    this.developerNames = await firstValueFrom(this.app.getDeveloperNames());
    this.projectNames = await firstValueFrom(this.app.getProjectNames());
    this.comapanyNames = await firstValueFrom(this.app.getCompanyNames());
  }

  private changeNames() {
    this.names = [
      ...(this.searchingDevelopers ? this.developerNames : []),
      ...(this.searchingProjects ? this.projectNames : []),
      ...(this.searchingCompanies ? this.comapanyNames : []),
    ];
  }

  private _filter(value: string): string[] {
    const filterValue = this._normalizeValue(value);
    return this.names.filter((name) =>
      this._normalizeValue(name).includes(filterValue),
    );
  }

  private _normalizeValue(value: string): string {
    return value.toLowerCase().replace(/\s/g, '');
  }

  onSearchChange() {
    const typeCount = this.getTypesCount();
    forkJoin([
      this.searchingDevelopers
        ? this.app.getDevelopers(
            this.search,
            this.take,
            this.page * this.take,
            this.follow,
          )
        : of(null),
      this.searchingProjects
        ? this.app.getProjects(
            this.search,
            this.take,
            this.page * this.take,
            this.follow,
          )
        : of(null),
      this.searchingCompanies
        ? this.app.getCompanies(
            this.search,
            this.take,
            this.page * this.take,
            this.follow,
          )
        : of(null),
    ]).subscribe(([developers, projects, companies]) => {
      this.totalDevCount = developers?.totalCount ?? 0;
      this.developers = developers?.developers ?? [];

      this.totalProjectCount = projects?.totalCount ?? 0;
      this.projects = projects?.projects ?? [];

      this.totalCompanyCount = companies?.totalCount ?? 0;
      this.companies = companies?.companies ?? [];

      this.totalCount =
        this.totalCompanyCount + this.totalDevCount + this.totalProjectCount;
    });
  }

  changePageSize() {
    this.pageSize = this.take * this.getTypesCount();
    this.pageOptions = [
      4 * this.getTypesCount(),
      8 * this.getTypesCount(),
      16 * this.getTypesCount(),
    ];
  }

  toggleDevelopersSearch() {
    this.searchingDevelopers = !this.searchingDevelopers;
    this.changeNames();
    this.onSearchChange();
    this.changePageSize();
  }

  toggleProjectsSearch() {
    this.searchingProjects = !this.searchingProjects;
    this.changeNames();
    this.onSearchChange();
    this.changePageSize();
  }

  toggleCompaniesSearch() {
    this.searchingCompanies = !this.searchingCompanies;
    this.changeNames();
    this.onSearchChange();
    this.changePageSize();
  }

  onPageChange(event: PageEvent) {
    this.take = event.pageSize / this.getTypesCount();
    this.page = event.pageIndex;
    this.onSearchChange();
  }

  getTypesCount() {
    return (
      +this.searchingCompanies +
      +this.searchingProjects +
      +this.searchingDevelopers
    );
  }
}
