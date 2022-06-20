import { Component, OnInit, ViewChild } from '@angular/core';
import * as moment from 'moment';
import { Moment } from 'moment';
import { MarkdownService } from 'ngx-markdown';
import { LocalStorageService } from '../../../services/local-storage.service';
import { Developer } from '../../../models/developer';
import { Errors } from '../../../models/errors';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AppService } from '../../../services/app.service';
import { Entity, EntityType } from '../../../models/entity';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-add-post',
  templateUrl: './add-post.component.html',
  styleUrls: ['./add-post.component.css'],
})
export class AddPostComponent implements OnInit {
  date?: Moment;
  options?: any;
  saved?: Moment;
  page?: string = 'editor';
  cover?: File;

  contentTouched: boolean = false;
  form: FormGroup;
  isSubmitting = false;
  submittedOnce = false;

  currentUser?: Developer;
  errors: Errors = { errors: {} };

  ownerId?: string;
  type?: EntityType;

  @ViewChild('input') input?: any;
  entity?: Entity;

  constructor(
    private markdownService: MarkdownService,
    private localStorageService: LocalStorageService,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private fb: FormBuilder,
    private app: AppService,
  ) {
    this.form = this.fb.group({
      title: [
        '',
        Validators.compose([Validators.required, Validators.maxLength(32)]),
      ],
      content: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(3000),
        ]),
      ],
      subType: [
        0,
        Validators.compose([
          Validators.required,
          Validators.min(1),
          Validators.max(4),
        ]),
      ],
    });
  }

  ngOnInit(): void {
    this.date = moment();
    this.options = {
      lang: 'en_US',
      mode: 'sv',
      tab: '  ',
      counter: {
        enable: true,
        max: 3000,
      },
      upload: {
        url: this.app.getUploadMdUrl(),
        multiple: false,
        format: (files: any, response: string) => {
          const res = JSON.parse(response);
          const key = Object.keys(res.data.succMap)[0];
          res.data.succMap[key] = this.app.getImagePath(res.data.succMap[key]);
          return JSON.stringify(res);
        },
      },
      typewriterMode: true,
      toolbar: [
        'emoji',
        'headings',
        'bold',
        'italic',
        'strike',
        'link',
        '|',
        'list',
        'ordered-list',
        'outdent',
        'indent',
        '|',
        'quote',
        'line',
        'code',
        'inline-code',
        'insert-before',
        'insert-after',
        '|',
        'upload',
        'table',
        '|',
        'undo',
        'redo',
        '|',
        'both',
        'preview',
      ],
      preview: {
        transform: this.parse.bind(this),
        actions: [],
      },
    };

    this.form.setValue({
      title: this.localStorageService.getData('post-title') ?? '',
      content: this.localStorageService.getData('post') ?? '',
      subType: 0,
    });

    this.userService.currentUser.subscribe((userData) => {
      this.currentUser = userData;
    });

    this.route.queryParams.subscribe((params) => {
      this.ownerId = params['id'];
      if (!this.ownerId) {
        this.router.navigateByUrl(`/`);
        return;
      }

      let query: Observable<any> | null = null;
      if (params['type'] === 'company') {
        this.type = EntityType.Company;
        query = this.app.getCompany(this.ownerId);
      }
      if (params['type'] === 'developer') {
        this.type = EntityType.Developer;
        query = this.app.getDeveloper(this.ownerId);
      }
      if (params['type'] === 'project') {
        this.type = EntityType.Project;
        query = this.app.getProject(this.ownerId);
      }

      if (!query) {
        this.router.navigateByUrl(`/`);
        return;
      }

      query.subscribe((entity: Entity) => {
        this.entity = entity;
      });
    });
  }

  get formControl() {
    return this.form.controls;
  }

  get subTypeControl(): FormControl {
    return this.form.controls['subType'] as FormControl;
  }

  get contentTypeControl(): FormControl {
    return this.form.controls['content'] as FormControl;
  }

  hasError(control: string, error: string): boolean {
    return (
      (this.formControl[control].touched || this.submittedOnce) &&
      this.formControl[control].errors?.[error]
    );
  }

  hasErrors(control: string): boolean {
    return (
      (this.formControl[control].touched || this.submittedOnce) &&
      !!this.formControl[control].errors
    );
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

  onCached() {
    this.saved = moment();
  }

  cacheTitle() {
    this.localStorageService.setData('post-title', this.form.value.title);
    this.onCached();
  }

  onFileChanged(image?: File) {
    this.cover = image;
  }

  cacheContent() {
    this.localStorageService.setData('post', this.form.value.content);
    this.onCached();
  }

  onSubmit() {
    this.isSubmitting = true;
    this.submittedOnce = true;
    this.errors = { errors: {} };

    if (!this.form.valid || !this.ownerId || !this.currentUser || !this.cover)
      return;

    const projectDto = {
      title: this.form.value.title,
      text: this.form.value.content,
      ownerId: this.ownerId,
      developerId: this.currentUser.id,
      subscriptionLevelId: this.form.value.subType,
      type: this.type,
    };

    this.app.createPost(projectDto).subscribe(
      (postId: number) => {
        this.localStorageService.removeData('post');
        this.localStorageService.removeData('post-title');
        if (this.cover)
          this.app.uploadFile(this.cover).subscribe((data: string) => {
            this.app.updatePost(postId, { imagePath: data }).subscribe({
              complete: () => {
                this.localStorageService.removeData('post');
                this.localStorageService.removeData('post-title');
                this.router.navigateByUrl(`/post/${postId}`);
              },
            });
          });
      },
      (err) => {
        this.errors = { errors: { error: err } };
        this.isSubmitting = false;
      },
    );
  }
}
