import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Post } from 'src/app/models/post';
import { AppService } from '../../../services/app.service';

@Component({
  selector: 'app-post-preview',
  templateUrl: './post-preview.component.html',
  styleUrls: ['./post-preview.component.css'],
})
export class PostPreviewComponent implements OnInit {
  @Input() post: Post = new Post();
  loading: boolean = true;
  requiredLevelName?: string;
  imagePath?: string;
  bgImagePath?: string;

  constructor(private app: AppService) {}

  ngOnInit(): void {
    this.requiredLevelName = '';

    this.app
      .getSubscriptionLevelName()
      .subscribe(
        (table) =>
          (this.requiredLevelName =
            table[this.post.requiredSubscriptionLevel!]),
      );

    if (this.post?.imagePath)
      this.imagePath = this.app.getImagePath(this.post.imagePath);

    this.bgImagePath = `url(${this.imagePath ?? 'assets/img/empty.png'})`;
  }

  onLoad() {
    this.loading = false;
  }
}
