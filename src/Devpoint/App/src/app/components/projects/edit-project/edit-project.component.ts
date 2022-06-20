import { Component, OnInit } from '@angular/core';
import { Developer } from '../../../models/developer';
import { Tag } from '../../../models/tag';
import { ActivatedRoute, Router } from '@angular/router';
import { Project } from '../../../models/project';
import { Errors } from '../../../models/errors';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { AppService } from '../../../services/app.service';
import { MatDialog } from '@angular/material/dialog';
import { catchError, firstValueFrom, from, map, switchMap, tap } from 'rxjs';
import { AddDeveloperModalComponent } from '../../companies/edit-company/add-developer-modal/add-developer-modal.component';

@Component({
  selector: 'app-edit-project',
  templateUrl: './edit-project.component.html',
  styleUrls: ['./edit-project.component.css'],
})
export class EditProjectComponent implements OnInit {
  project?: Project;
  projectId?: string;

  page: number = 0;

  developers: { developer: Developer; deleted: boolean }[] = [];

  errors: Errors = { errors: {} };
  form: FormGroup;
  isSubmitting = false;
  submittedOnce = false;

  image?: File;
  currentImagePath?: string;
  tags: Tag[] = [];

  successMessage?: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private fb: FormBuilder,
    private app: AppService,
    public dialog: MatDialog,
  ) {
    this.form = this.fb.group({
      name: [
        '',
        Validators.compose([Validators.required, Validators.maxLength(32)]),
      ],
      description: ['', Validators.maxLength(256)],
    });
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.page = Number(params['page'] ?? 0);
    });

    this.route.params
      .pipe(
        tap((params) => {
          this.projectId = params['id'];
        }),
        switchMap((_) => from(this.setCompany())),
        catchError((err) => this.router.navigateByUrl('/404')),
      )
      .subscribe();
  }

  async setCompany() {
    this.project = await firstValueFrom(this.app.getProject(this.projectId!));

    if (!this.project) throw new Error();

    this.project.tags = await firstValueFrom(
      this.app.getProjectTags(this.projectId!),
    );

    this.developers = await firstValueFrom(
      this.app.getProjectDevelopers(this.projectId!).pipe(
        map((developers: Developer[]) =>
          developers.map((developer) => ({
            developer,
            deleted: false,
          })),
        ),
      ),
    );

    if (this.project.imagePath)
      this.currentImagePath = this.app.getImagePath(this.project.imagePath);

    this.tags = this.project.tags ?? [];
    this.form.setValue({
      name: this.project.name,
      description: this.project.description,
    });
  }

  onFileChanged(image?: File) {
    this.image = image;
  }

  get formControl() {
    return this.form.controls;
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

  onDeveloperRemove(entry: { developer: Developer; deleted: boolean }) {
    entry.deleted = true;
  }

  onDeveloperUndoRemove(entry: { developer: Developer; deleted: boolean }) {
    entry.deleted = false;
  }

  onDeveloperAdd() {
    const dialogRef = this.dialog.open(AddDeveloperModalComponent, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result)
        this.app.getDeveloperByEmail(result).subscribe((developer) =>
          this.developers.push({
            developer,
            deleted: false,
          }),
        );
    });
  }

  submitForm() {
    this.isSubmitting = true;
    this.submittedOnce = true;
    this.errors = { errors: {} };

    if (!this.form.valid || !this.projectId) return;

    const updateDto = {
      name: this.form.value.name,
      description: this.form.value.description,
      tags: this.tags,
    };

    this.app.updateProject(this.projectId, updateDto).subscribe(
      (data) => {
        this.successMessage = 'Project saved successfully!';
      },
      (err) => {
        this.setError(err);
        this.isSubmitting = false;
      },
    );

    if (this.image)
      this.app.uploadFile(this.image).subscribe((data: string) => {
        this.app.updateProject(this.projectId!, { imagePath: data }).subscribe({
          error: (err) => {
            this.setError(err);
          },
        });
      });

    if (this.developers?.length) {
      const filteredEntries = this.developers.filter((entry) => !entry.deleted);
      this.app
        .updateProjectDevelopers(
          this.projectId,
          filteredEntries.map((entry) => entry.developer.id),
        )
        .subscribe({
          next: () => {
            this.developers = filteredEntries;
          },
          error: (err) => {
            this.setError(err);
          },
        });
    }
  }

  setError(err: any) {
    this.errors = {
      errors: {
        ...this.errors.errors,
        [err]: err,
      },
    };
  }
}
