import { Component, OnInit } from '@angular/core';
import { Tag } from '../../../models/tag';
import { Company } from '../../../models/company';
import { plainToTyped } from 'type-transformer';
import { ActivatedRoute, Router } from '@angular/router';
import { Developer } from '../../../models/developer';
import { Errors } from '../../../models/errors';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../services/user.service';
import { AppService } from '../../../services/app.service';
import { CustomValidationService } from '../../../services/custom-validation.service';
import { catchError, firstValueFrom, from, map, switchMap, tap } from 'rxjs';
import { Project } from '../../../models/project';
import { MatDialog } from '@angular/material/dialog';
import { AddDeveloperModalComponent } from './add-developer-modal/add-developer-modal.component';

@Component({
  selector: 'app-edit-company',
  templateUrl: './edit-company.component.html',
  styleUrls: ['./edit-company.component.css'],
})
export class EditCompanyComponent implements OnInit {
  company?: Company;
  companyId?: string;

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
          this.companyId = params['id'];
        }),
        switchMap((_) => from(this.setCompany())),
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
        map((developers: Developer[]) =>
          developers.map((developer) => ({
            developer,
            deleted: false,
          })),
        ),
      ),
    );

    if (this.company.imagePath)
      this.currentImagePath = this.app.getImagePath(this.company.imagePath);

    this.tags = this.company.tags ?? [];
    this.form.setValue({
      name: this.company.name,
      description: this.company.description,
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

    if (!this.form.valid || !this.companyId) return;

    const updateDto = {
      name: this.form.value.name,
      description: this.form.value.description,
      tags: this.tags,
    };

    this.app.updateCompany(this.companyId, updateDto).subscribe(
      (data) => {
        this.successMessage = 'Company saved successfully!';
      },
      (err) => {
        this.setError(err);
        this.isSubmitting = false;
      },
    );

    if (this.image)
      this.app.uploadFile(this.image).subscribe((data: string) => {
        this.app.updateCompany(this.companyId!, { imagePath: data }).subscribe({
          error: (err) => {
            this.setError(err);
          },
        });
      });

    if (this.developers?.length) {
      const filteredEntries = this.developers.filter((entry) => !entry.deleted);
      this.app
        .updateCompanyDevelopers(
          this.companyId,
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
