import { Component, Input, OnInit } from '@angular/core';
import { Post } from '../../../models/post';
import { Observable } from 'rxjs';
import { AppService } from '../../../services/app.service';

@Component({
  selector: 'app-posts-container',
  templateUrl: './posts-container.component.html',
  styleUrls: ['./posts-container.component.css'],
})
export class PostsContainerComponent implements OnInit {
  @Input() getRequest?: (
    take: number,
    skip: number,
    search?: string,
  ) => Observable<{ totalCount: number; posts: Post[] }>;
  totalCount: number = 0;
  posts: Post[] = [];
  search?: string;
  @Input() take: number = 3;

  constructor() {}

  ngOnInit(): void {
    if (this.getRequest)
      this.getRequest(this.take, 0, this.search).subscribe((value) => {
        this.totalCount = value.totalCount;
        this.appendPosts(value.posts);
      });
  }

  appendPosts(newPosts: Post[]) {
    this.posts = [...(this.posts ?? []), ...newPosts];
  }

  onScrollDown() {
    if (this.posts.length >= this.totalCount) return;
    if (this.getRequest)
      this.getRequest(
        this.take,
        this.posts?.length ?? 0,
        this.search,
      ).subscribe((value) => {
        this.totalCount = value.totalCount;
        this.appendPosts(value.posts);
      });
  }

  onSearchChange() {
    this.posts = [];
    if (this.getRequest)
      this.getRequest(this.take, 0, this.search).subscribe((value) => {
        this.totalCount = value.totalCount;
        this.appendPosts(value.posts);
      });
  }
}
