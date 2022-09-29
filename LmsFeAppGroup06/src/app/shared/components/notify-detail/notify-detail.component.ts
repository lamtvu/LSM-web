import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-notify-detail',
  templateUrl: './notify-detail.component.html',
  styleUrls: ['./notify-detail.component.scss']
})
export class NotifyDetailComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {message: string}) { }

  ngOnInit(): void {
  }

}
