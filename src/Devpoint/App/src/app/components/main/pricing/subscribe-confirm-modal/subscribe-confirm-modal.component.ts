import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface SubscribeConfirmModalData {
  price: number;
  type: string;
}

@Component({
  selector: 'app-subscribe-confirm-modal',
  templateUrl: './subscribe-confirm-modal.component.html',
  styleUrls: ['./subscribe-confirm-modal.component.css'],
})
export class SubscribeConfirmModalComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<SubscribeConfirmModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SubscribeConfirmModalData,
  ) {}

  ngOnInit(): void {}
}
