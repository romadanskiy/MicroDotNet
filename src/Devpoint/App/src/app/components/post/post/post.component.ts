import { Component, OnInit } from '@angular/core';
import { Post } from '../../../models/post';
import * as moment from 'moment';
import { AppService } from '../../../services/app.service';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, firstValueFrom, from, switchMap, tap } from 'rxjs';
import { Developer } from '../../../models/developer';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css'],
})
export class PostComponent implements OnInit {
  postId?: string;
  post?: Post;
  loading: boolean = true;
  imagePath?: string;
  bgImagePath?: string;

  constructor(
    private app: AppService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.route.params
      .pipe(
        tap((params) => {
          this.postId = params['id'];
        }),
        switchMap((_) => from(this.setPost())),
        catchError((err) => this.router.navigateByUrl('/404')),
      )
      .subscribe();
  }

  async setPost() {
    if (!this.postId) throw new Error();
    this.post = await firstValueFrom(this.app.getPost(this.postId));

    if (!this.post?.hasUserAccess) {
      this.router.navigateByUrl('/403');
    }

    if (this.post?.imagePath)
      this.imagePath = this.app.getImagePath(this.post.imagePath);

    this.bgImagePath = `url(${this.imagePath ?? 'assets/img/empty.png'})`;
  }

  onLoad() {
    this.loading = false;
  }
}
