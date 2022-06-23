import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-developer-modal',
  templateUrl: './add-developer-modal.component.html',
  styleUrls: ['./add-developer-modal.component.css'],
})
export class AddDeveloperModalComponent implements OnInit {
  form: FormGroup;
  isSubmitting = false;
  submittedOnce = false;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<AddDeveloperModalComponent>,
  ) {
    this.form = this.fb.group({
      email: [
        '',
        Validators.compose([
          Validators.required,
          Validators.maxLength(64),
          Validators.email,
        ]),
      ],
    });
  }

  ngOnInit(): void {}

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

  onSubmit() {}

  onCancel() {
    this.dialogRef.close();
  }
}
