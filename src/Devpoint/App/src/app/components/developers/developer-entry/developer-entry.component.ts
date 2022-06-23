import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Developer } from '../../../models/developer';
import { AppService } from '../../../services/app.service';

@Component({
  selector: 'app-developer-entry',
  templateUrl: './developer-entry.component.html',
  styleUrls: ['./developer-entry.component.css'],
})
export class DeveloperEntryComponent implements OnInit {
  @Input() developer?: Developer;
  @Input() isRemoved?: boolean;
  @Input() isOwner?: boolean;
  @Output() remove = new EventEmitter<Developer>();
  @Output() undoRemove = new EventEmitter<Developer>();
  imagePath?: string;
  loading: boolean = true;
  constructor(private app: AppService) {}

  ngOnInit(): void {
    if (this.developer?.imagePath)
      this.imagePath = this.app.getImagePath(this.developer.imagePath);
  }

  onLoad() {
    this.loading = false;
  }
}
