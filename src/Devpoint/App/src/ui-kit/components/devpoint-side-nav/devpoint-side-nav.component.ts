import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DevpointBorderStyle, DevpointButtonStyle } from '@ui-kit/components';

@Component({
  selector: 'app-devpoint-side-nav',
  templateUrl: './devpoint-side-nav.component.html',
  styleUrls: ['./devpoint-side-nav.component.css'],
})
export class DevpointSideNavComponent implements OnInit {
  @Input() pages: string[] = [];
  @Output() pageChange = new EventEmitter<number>();
  @Input() page = 0;

  @Input() borderStyle: 'default' | 'more' | 'round' = 'more';

  _vertical: boolean = false;
  @Input()
  set vertical(value: boolean | '') {
    this._vertical = value === '' || value;
  }
  get vertical(): boolean {
    return this._vertical;
  }

  _noStick: boolean = false;
  @Input('no-stick')
  set noStick(value: boolean | '') {
    this._noStick = value === '' || value;
  }
  get noStick(): boolean {
    return this._noStick;
  }

  constructor() {}

  ngOnInit(): void {}

  onClick(index: number) {
    this.page = index;
    this.pageChange.emit(index);
  }
}
