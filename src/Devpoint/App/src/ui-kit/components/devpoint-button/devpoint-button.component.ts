import {
  Attribute,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';

export enum DevpointButtonStyle {
  Main = 'main',
  White = 'white',
  Black = 'black',
  TransparentWhite = 'transparent-white',
  TransparentBlack = 'transparent-black',
  TransparentOutline = 'transparent-outline',
  TransparentOutlineRed = 'transparent-outline-red',
}

export enum DevpointBorderStyle {
  Default = 'default',
  More = 'more',
  Rount = 'round',
}

export enum DevpointSize {
  Default = 'default',
  Small = 'small',
  Large = 'large',
}

@Component({
  selector: 'app-devpoint-button',
  templateUrl: './devpoint-button.component.html',
  styleUrls: ['./devpoint-button.component.css'],
})
export class DevpointButtonComponent implements OnInit {
  @Input() bold?: boolean = true;

  @Input() maxWidth?: number;

  @Input() formTarget?: string;

  private _autoWidth: boolean = false;

  @Input() disabled?: boolean;

  @Input() padding?: string;

  @Input() buttonFontColor?: string;

  _devpointStyle?: `${DevpointButtonStyle}`;
  @Input('devpoint-style')
  public set devpointStyle(
    value:
      | 'main'
      | 'white'
      | 'black'
      | 'transparent-white'
      | 'transparent-black'
      | 'transparent-outline'
      | 'transparent-outline-red'
      | undefined,
  ) {
    if (this._devpointStyle)
      this.setClassMap(`style-${this._devpointStyle}`, false);
    if (value) this.setClassMap(`style-${value}`, true);
    this._devpointStyle = value;
  }
  get devpointStyle(): `${DevpointButtonStyle}` | undefined {
    return this._devpointStyle;
  }

  _devpointBorderStyle?: `${DevpointBorderStyle}`;
  @Input('devpoint-border-style')
  public set devpointBorderStyle(
    value: 'default' | 'more' | 'round' | undefined,
  ) {
    if (this._devpointBorderStyle)
      this.setClassMap(`border-${this._devpointBorderStyle}`, false);
    if (value) this.setClassMap(`border-${value}`, true);
    this._devpointBorderStyle = value;
  }
  get devpointBorderStyle(): `${DevpointBorderStyle}` | undefined {
    return this._devpointBorderStyle;
  }

  _devpointSize?: `${DevpointSize}`;
  @Input('size')
  public set devpointSize(value: 'default' | 'large' | 'small' | undefined) {
    if (this._devpointSize)
      this.setClassMap(`size-${this._devpointSize}`, false);
    if (value) this.setClassMap(`size-${value}`, true);
    this._devpointSize = value;
  }
  get devpointSize(): `${DevpointSize}` | undefined {
    return this._devpointSize;
  }

  @Input()
  set autoWidth(value: boolean | '') {
    this._autoWidth = value === '' || value;
  }
  get autoWidth(): boolean {
    return this._autoWidth;
  }

  private _fullWidth: boolean = false;

  @Input()
  set fullWidth(value: boolean | '') {
    this._fullWidth = value === '' || value;
  }
  get fullWidth(): boolean {
    return this._fullWidth;
  }

  private _noBorderLeft: boolean = false;

  @Input()
  set noBorderLeft(value: boolean | '') {
    this._noBorderLeft = value === '' || value;
  }
  get noBorderLeft(): boolean {
    return this._noBorderLeft;
  }

  private _noBorderRight: boolean = false;

  @Input()
  set noBorderRight(value: boolean | '') {
    this._noBorderRight = value === '' || value;
  }
  get noBorderRight(): boolean {
    return this._noBorderRight;
  }

  @Output() buttonClick = new EventEmitter<MouseEvent>();

  classMap: Record<string, boolean> = {};

  constructor(@Attribute('type') public type: string = 'submit') {}

  ngOnInit(): void {
    if (!this.devpointStyle) this.devpointStyle = DevpointButtonStyle.Main;
    if (!this.devpointBorderStyle)
      this.devpointBorderStyle = DevpointBorderStyle.Default;
    if (!this.devpointSize) this.devpointSize = DevpointSize.Default;
  }

  setClassMap(key: string, value: boolean) {
    this.classMap = {
      ...this.classMap,
      [key]: value,
    };
  }

  onClick(event: MouseEvent) {
    this.buttonClick.emit(event);
  }
}
