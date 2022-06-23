import { Component, Input, OnInit } from '@angular/core';
import { Errors } from '../../../app/models/errors';

@Component({
  selector: 'app-devpoint-errors',
  templateUrl: './devpoint-errors.component.html',
  styleUrls: ['./devpoint-errors.component.css'],
})
export class DevpointErrorsComponent implements OnInit {
  formattedErrors: Array<string> = [];

  @Input()
  set errors(errorList: Errors) {
    this.formattedErrors = Object.keys(errorList.errors || {}).map(
      (key) => `${errorList.errors[key]}`,
    );
  }

  get errorList() {
    return this.formattedErrors;
  }

  constructor() {}

  ngOnInit() {}
}
