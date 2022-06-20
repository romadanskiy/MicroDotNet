import { Component, Input, OnInit } from '@angular/core';
import { Project } from 'src/app/models/project';
import { DevpointMiniPreviewProps } from '@ui-kit/components/devpoint-mini-preview/devpoint-mini-preview.props';

@Component({
  selector: 'app-devpoint-mini-preview',
  templateUrl: './devpoint-mini-preview.component.html',
  styleUrls: ['./devpoint-mini-preview.component.css'],
})
export class DevpointMiniPreviewComponent implements OnInit {
  @Input() item: DevpointMiniPreviewProps | null = null;
  loading: boolean = true;

  constructor() {}

  ngOnInit(): void {}

  onLoad() {
    this.loading = false;
  }
}
