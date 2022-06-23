import { Component, Input, OnInit } from '@angular/core';
import { Company } from 'src/app/models/company';
import { Developer } from '../../../models/developer';
import { plainToTyped } from 'type-transformer';
import { DevpointMiniPreviewProps } from '@ui-kit/components/devpoint-mini-preview/devpoint-mini-preview.props';
import * as moment from 'moment';
import { catchError, EMPTY, firstValueFrom, from, map, Observable, of, switchMap, tap } from 'rxjs';
import { Post } from '../../../models/post';
import { Project } from '../../../models/project';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AppService } from '../../../services/app.service';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styleUrls: ['./company.component.css'],
})
export class CompanyComponent implements OnInit {
  isOwned: boolean = true;

  currentUser?: Developer;
  company?: Company;
  companyId?: string;

  hideProjects: boolean = true;
  hideDevelopers: boolean = true;

  projects: DevpointMiniPreviewProps[] = [];
  developers: DevpointMiniPreviewProps[] = [];

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
          this.companyId = params['id'];
        }),
        switchMap((_) =>
          this.userService.currentUser.pipe(
            tap((userData: Developer) => {
              this.currentUser = userData;
            }),
            switchMap((_) => from(this.setCompany())),
          ),
        ),
        catchError((err) => this.router.navigateByUrl('/404')),
      )
      .subscribe();
  }

  async setCompany() {
    this.company = await firstValueFrom(this.app.getCompany(this.companyId!));

    if (!this.company) throw new Error();

    this.company.tags = await firstValueFrom(
      this.app.getCompanyTags(this.companyId!),
    );

    this.developers = await firstValueFrom(
      this.app.getCompanyDevelopers(this.companyId!).pipe(
        map((developer: Developer[]) =>
          developer.map((d) => ({
            link: `/developer/${d.id}`,
            name: d.name,
            imgSrc: d.imagePath
              ? this.app.getImagePath(d.imagePath)
              : undefined,
          })),
        ),
      ),
    );

    this.projects = await firstValueFrom(
      this.app.getCompanyProjects(this.companyId!).pipe(
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

    this.isOwned = this.currentUser?.id === this.company?.ownerId;
  }

  getPostsRequest(take: number, skip: number, search?: string) {
    if (!this.companyId) return EMPTY;
    return this.app.getCompanyPosts(this.companyId, search, take, skip);
  }
}
