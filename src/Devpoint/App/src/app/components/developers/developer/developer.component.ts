import { Component, Input, OnInit } from '@angular/core';
import { Developer } from 'src/app/models/developer';
import { DevpointMiniPreviewProps } from '@ui-kit/components/devpoint-mini-preview/devpoint-mini-preview.props';
import * as moment from 'moment';
import { plainToTyped } from 'type-transformer';
import {
  catchError,
  concatMap,
  EMPTY,
  firstValueFrom,
  map,
  Observable,
  of,
  pipe,
  switchMap,
  tap,
  timeout,
  timer,
} from 'rxjs';
import { Post } from '../../../models/post';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../../../services/app.service';
import { UserService } from '../../../services/user.service';
import { Project } from '../../../models/project';
import { from } from 'rxjs';
import { Company } from '../../../models/company';

@Component({
  selector: 'app-developer',
  templateUrl: './developer.component.html',
  styleUrls: ['./developer.component.css'],
})
export class DeveloperComponent implements OnInit {
  @Input() developerId?: string;
  isProfile: boolean = false;

  developer?: Developer;
  currentUser?: Developer;

  hideProjects: boolean = true;
  hideCompanies: boolean = true;

  projects: DevpointMiniPreviewProps[] = [];
  companies: DevpointMiniPreviewProps[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private app: AppService,
  ) {
    this.getPostsRequest = this.getPostsRequest.bind(this);
  }

  ngOnInit(): void {
    this.route.params
      .pipe(
        tap((params) => {
          this.developerId = this.developerId ?? params['id'];
        }),
        switchMap((_) =>
          this.userService.currentUser.pipe(
            tap((userData: Developer) => {
              this.currentUser = userData;
            }),
            switchMap((_) => from(this.setDeveloper())),
          ),
        ),
        catchError((err) => this.router.navigateByUrl('/404')),
      )
      .subscribe();
  }

  async setDeveloper() {
    this.developer = await firstValueFrom(
      this.app.getDeveloper(this.developerId!),
    );

    if (!this.developer) throw new Error();

    this.developer.tags = await firstValueFrom(
      this.app.getDeveloperTags(this.developerId!),
    );

    this.projects = await firstValueFrom(
      this.app.getDeveloperProjects(this.developerId!).pipe(
        map((projects: Project[]) =>
          projects.map((p) => ({
            link: `/project/${p.id}`,
            name: p.name,
            imgSrc: p.imagePath
              ? this.app.getImagePath(p.imagePath)
              : undefined,
          })),
        ),
      ),
    );

    this.companies = await firstValueFrom(
      this.app.getDeveloperCompanies(this.developerId!).pipe(
        map((companies: Company[]) =>
          companies.map((c) => ({
            link: `/company/${c.id}`,
            name: c.name,
            imgSrc: c.imagePath
              ? this.app.getImagePath(c.imagePath)
              : undefined,
          })),
        ),
      ),
    );

    this.isProfile = this.currentUser?.id === this.developer?.id;
  }

  getPostsRequest(take: number, skip: number, search?: string) {
    if (!this.developerId) return EMPTY;
    return this.app.getDeveloperPosts(this.developerId, search, take, skip);
  }
}
