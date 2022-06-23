import { Component, Input, OnInit } from '@angular/core';
import { Project } from 'src/app/models/project';
import { Developer } from '../../../models/developer';
import { plainToTyped } from 'type-transformer';
import { DevpointMiniPreviewProps } from '@ui-kit/components/devpoint-mini-preview/devpoint-mini-preview.props';
import * as moment from 'moment';
import {
  BehaviorSubject,
  catchError,
  EMPTY,
  firstValueFrom,
  from,
  map,
  Observable,
  of,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { Post } from '../../../models/post';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AppService } from '../../../services/app.service';
import { Entity } from '../../../models/entity';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css'],
})
export class ProjectComponent implements OnInit {
  isOwned: boolean = true;

  currentUser?: Developer;
  project?: Project;
  projectId?: string;

  owner: BehaviorSubject<Entity | null> = new BehaviorSubject<Entity | null>(
    null,
  );

  hideDevelopers: boolean = true;

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
          this.projectId = params['id'];
        }),
        switchMap((_) =>
          this.userService.currentUser.pipe(
            tap((userData: Developer) => {
              this.currentUser = userData;
            }),
            switchMap((_) => from(this.setProject())),
          ),
        ),
        catchError((err) => this.router.navigateByUrl('/404')),
      )
      .subscribe();
  }

  async setProject() {
    this.project = await firstValueFrom(this.app.getProject(this.projectId!));

    if (!this.project) throw new Error();

    this.project.tags = await firstValueFrom(
      this.app.getProjectTags(this.projectId!),
    );

    this.developers = await firstValueFrom(
      this.app.getProjectDevelopers(this.projectId!).pipe(
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

    if (this.project.companyId)
      this.owner.next(
        await firstValueFrom(this.app.getCompany(this.project.companyId)),
      );
    else
      this.owner.next(
        await firstValueFrom(this.app.getDeveloper(this.project.ownerId)),
      );

    this.isOwned = this.currentUser?.id === this.project?.ownerId;
  }

  getPostsRequest(take: number, skip: number, search?: string) {
    if (!this.projectId) return EMPTY;
    return this.app.getProjectPosts(this.projectId, search, take, skip);
  }
}
