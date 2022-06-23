import { Injectable } from '@angular/core';
import { AbstractControl, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class CustomValidationService {
  patternValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) {
        return null;
      }
      const regex = new RegExp('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$');
      const valid = regex.test(control.value);
      return valid ? null : { invalidPassword: true };
    };
  }

  MatchPassword(password: string, confirmPassword: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const passwordValue = control.value[password];
      const confirmPasswordValue = control.value[confirmPassword];

      if (!passwordValue || !confirmPasswordValue) {
        return null;
      }

      if (passwordValue !== confirmPasswordValue) {
        return { passwordMismatch: true };
      } else {
        return null;
      }
    };
  }
}
