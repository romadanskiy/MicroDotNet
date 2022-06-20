import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Tag } from '../../../models/tag';
import { Errors } from '../../../models/errors';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AppService } from '../../../services/app.service';

@Component({
  selector: 'app-create-company',
  templateUrl: './create-company.component.html',
  styleUrls: ['./create-company.component.css'],
})
export class CreateCompanyComponent implements OnInit {
  image?: File;
  tags: Tag[] = [];

  errors: Errors = { errors: {} };
  form: FormGroup;
  isSubmitting = false;
  submittedOnce = false;

  userId?: string;

  constructor(
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
      description: ['', Validators.maxLength(256)],
    });
  }

  ngOnInit() {
    this.userService.currentUser.subscribe((userData) => {
      this.userId = userData.id;
    });
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

  onFileChanged(image?: File) {
    this.image = image;
  }

  onSubmit() {
    this.isSubmitting = true;
    this.submittedOnce = true;
    this.errors = { errors: {} };

    if (!this.form.valid || !this.userId) return;

    const companyDto = {
      name: this.form.value.title,
      description: this.form.value.description,
      ownerId: this.userId,
      tags: this.tags,
    };

    this.app.createCompany(companyDto).subscribe(
      (companyId: string) => {
        if (this.image)
          this.app.uploadFile(this.image).subscribe((data: string) => {
            this.app.updateCompany(companyId, { imagePath: data }).subscribe({
              complete: () => {
                this.router.navigateByUrl(`/company/${companyId}`);
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
