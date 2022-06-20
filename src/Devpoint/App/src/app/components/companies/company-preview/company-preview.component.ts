import { Component, Input, OnInit } from '@angular/core';
import { Company } from 'src/app/models/company';
import * as moment from 'moment';

@Component({
  selector: 'app-company-preview',
  templateUrl: './company-preview.component.html',
  styleUrls: ['./company-preview.component.css'],
})
export class CompanyPreviewComponent implements OnInit {
  @Input() company: Company = new Company();
  @Input() isOwned: boolean = false;

  private _isLarge: boolean = false;

  @Input()
  set large(value: boolean | '') {
    this._isLarge = value === '' || value;
  }
  get large(): boolean {
    return this._isLarge;
  }

  constructor() {}

  ngOnInit(): void {}
}
