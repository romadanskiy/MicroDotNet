import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Post } from 'src/app/models/post';
import { AppService } from '../../../services/app.service';
import { MarkdownService } from 'ngx-markdown';

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
  options?: any;

  constructor(
    private app: AppService,
    private markdownService: MarkdownService,
  ) {}

  ngOnInit(): void {
    this.options = {
      lang: 'en_US',
      mode: 'sv',
      transform: this.parse.bind(this),
    };

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

  highlight() {
    setTimeout(() => {
      this.markdownService.highlight();
    });
  }

  parse(inputValue: string) {
    const markedOutput = this.markdownService.compile(inputValue.trim());
    this.highlight();

    return markedOutput;
  }
}
