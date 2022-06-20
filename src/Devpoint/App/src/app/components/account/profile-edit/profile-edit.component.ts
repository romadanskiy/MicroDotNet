import { Component, OnInit } from '@angular/core';
import { Developer } from '../../../models/developer';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { catchError, firstValueFrom, from, switchMap, tap } from 'rxjs';
import { Tag } from '../../../models/tag';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AppService } from '../../../services/app.service';
import { Errors } from '../../../models/errors';
import { CustomValidationService } from '../../../services/custom-validation.service';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.css'],
})
export class ProfileEditComponent implements OnInit {
  developerId?: string;

  developer?: Developer;
  currentUser?: Developer;

  page: number = 0;

  errors: Errors = { errors: {} };
  form: FormGroup;
  isSubmitting = false;
  submittedOnce = false;

  passwordForm: FormGroup;
  isPasswordSubmitting = false;
  passwordSubmittedOnce = false;

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
    private customValidator: CustomValidationService,
  ) {
    this.form = this.fb.group({
      name: [
        '',
        Validators.compose([Validators.required, Validators.maxLength(32)]),
      ],
      description: ['', Validators.maxLength(256)],
    });

    this.passwordForm = this.fb.group(
      {
        oldPassword: [
          '',
          Validators.compose([Validators.required, Validators.maxLength(32)]),
        ],
        newPassword: [
          '',
          Validators.compose([
            Validators.required,
            this.customValidator.patternValidator(),
            Validators.maxLength(32),
          ]),
        ],
        confirmNewPassword: [
          '',
          Validators.compose([Validators.required, Validators.maxLength(32)]),
        ],
      },
      {
        validators: this.customValidator.MatchPassword(
          'newPassword',
          'confirmNewPassword',
        ),
      },
    );
  }

  ngOnInit(): void {
    this.userService.currentUser
      .pipe(
        tap((userData: Developer) => {
          this.currentUser = userData;
          this.developerId = this.currentUser.id;
        }),
        switchMap((_) => from(this.setDeveloper())),
        catchError((err) => this.router.navigateByUrl('/404')),
      )
      .subscribe();
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

  get passwordFormControl() {
    return this.passwordForm.controls;
  }

  passwordHasError(control: string, error: string): boolean {
    return (
      (this.passwordFormControl[control].touched ||
        this.passwordSubmittedOnce) &&
      this.passwordFormControl[control].errors?.[error]
    );
  }

  passwordFormHasError(error: string): boolean {
    return this.passwordSubmittedOnce && this.passwordForm.errors?.[error];
  }

  passwordHasErrors(control: string): boolean {
    return (
      (this.passwordFormControl[control].touched ||
        this.passwordSubmittedOnce) &&
      !!this.passwordFormControl[control].errors
    );
  }

  async setDeveloper() {
    this.developer = await firstValueFrom(
      this.app.getDeveloper(this.developerId!),
    );

    if (!this.developer) throw new Error();

    this.developer.tags = await firstValueFrom(
      this.app.getDeveloperTags(this.developerId!),
    );

    if (this.developer.imagePath)
      this.currentImagePath = this.app.getImagePath(this.developer.imagePath);

    this.tags = this.developer.tags ?? [];
    this.form.setValue({
      name: this.developer.name,
      description: this.developer.description,
    });
  }

  onFileChanged(image?: File) {
    this.image = image;
  }

  submitForm() {
    this.isSubmitting = true;
    this.submittedOnce = true;
    this.errors = { errors: {} };

    if (!this.form.valid || !this.developerId) return;

    const updateDto = {
      name: this.form.value.name,
      description: this.form.value.description,
      tags: this.tags,
    };

    this.app.updateDeveloper(this.developerId, updateDto).subscribe(
      (data) => {
        this.successMessage = 'Profile saved successfully!';
      },
      (err) => {
        this.setError(err);
        this.isSubmitting = false;
      },
    );

    if (this.image)
      this.app.uploadFile(this.image).subscribe((data: string) => {
        this.app
          .updateDeveloper(this.developerId!, { imagePath: data })
          .subscribe({
            error: (err) => {
              this.setError(err);
            },
          });
      });
  }

  setError(err: any) {
    this.errors = {
      errors: {
        ...this.errors.errors,
        [err]: err,
      },
    };
  }

  submitPasswordForm() {
    this.isPasswordSubmitting = true;
    this.passwordSubmittedOnce = true;
    this.errors = { errors: {} };

    if (!this.passwordForm.valid || !this.developerId) return;

    const credentials = {
      oldPassword: this.passwordForm.value.oldPassword,
      newPassword: this.passwordForm.value.newPassword,
    };

    //TODO: add change password query
  }
}
