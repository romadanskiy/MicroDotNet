import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { Errors } from '../../../app/models/errors';

@Component({
  selector: 'app-devpoint-upload-image',
  templateUrl: './devpoint-upload-image.component.html',
  styleUrls: ['./devpoint-upload-image.component.css'],
})
export class DevpointUploadImageComponent implements OnInit {
  dragover: boolean = false;
  image?: File;
  coverUrl?: string;
  bgImageUrl?: string;
  @Input() validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
  @Input() maxMbSize: number = 4;
  @ViewChild('fileDropRef') fileDropRef?: HTMLInputElement;
  @ViewChild('fileDropRef2') fileDropRef2?: HTMLInputElement;

  @Input() background: boolean = true;
  @Input() label?: string;
  @Input() description?: string;
  @Input() buttonLabel?: string;
  @Input() currentImagePath?: string;

  error?: string;

  _noMargin = false;

  @Input()
  set noMargin(value: boolean | '') {
    this._noMargin = value === '' || value;
  }
  get noMargin(): boolean {
    return this._noMargin;
  }

  private _fullWidth: boolean = false;

  @Input()
  set fullWidth(value: boolean | '') {
    this._fullWidth = value === '' || value;
  }
  get fullWidth(): boolean {
    return this._fullWidth;
  }

  @Output() fileChange = new EventEmitter<File>();

  constructor() {}

  ngOnInit(): void {}

  onFileDropped(event: FileList) {
    this.error = undefined;
    this.dragover = false;
    this.image = event[0];
    const fileType = this.image['type'];
    if (!this.validImageTypes.includes(fileType)) {
      this.error = 'File type not supported';
      return;
    }

    const mbSize = this.image.size / 1024 / 1024;
    if (mbSize > this.maxMbSize) {
      this.error = `The file size is ${Number(
        mbSize.toFixed(2),
      )} MB exceeding the maximum file size of ${this.maxMbSize} MB`;
      return;
    }

    const reader = new FileReader();

    reader.onload = (event: any) => {
      this.coverUrl = event.target.result;
      this.bgImageUrl = `url(${this.coverUrl})`;
      this.fileChange.emit(this.image);
    };

    reader.onerror = (event: any) => {
      this.error = 'File could not be read: ' + event.target.error.code;
    };

    reader.readAsDataURL(this.image);
  }

  onFileChanged(event?: HTMLInputElement) {
    if (event?.files) this.onFileDropped(event.files);
  }
}
