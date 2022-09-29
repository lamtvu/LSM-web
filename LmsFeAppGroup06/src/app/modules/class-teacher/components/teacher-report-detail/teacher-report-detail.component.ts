import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-teacher-report-detail',
  templateUrl: './teacher-report-detail.component.html',
  styleUrls: ['./teacher-report-detail.component.scss']
})
export class TeacherReportDetailComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: {message: string}) { }

  ngOnInit(): void {
  }

}
