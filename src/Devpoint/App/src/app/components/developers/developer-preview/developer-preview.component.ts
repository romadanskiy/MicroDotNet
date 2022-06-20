import { Component, Input, OnInit } from '@angular/core';
import { Developer } from 'src/app/models/developer';

@Component({
  selector: 'app-developer-preview',
  templateUrl: './developer-preview.component.html',
  styleUrls: ['./developer-preview.component.css'],
})
export class DeveloperPreviewComponent implements OnInit {
  private _isLarge: boolean = false;

  @Input()
  set large(value: boolean | '') {
    this._isLarge = value === '' || value;
  }
  get large(): boolean {
    return this._isLarge;
  }

  @Input() developer: Developer = new Developer();
  @Input() isCurrent: boolean = false;

  constructor() {}

  ngOnInit(): void {}
}
