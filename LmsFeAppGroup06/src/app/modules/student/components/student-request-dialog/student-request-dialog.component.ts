
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { StudentService } from '../../services/student.service';

@Component({
  selector: 'app-student-request-dialog',
  templateUrl: './student-request-dialog.component.html',
  styleUrls: ['./student-request-dialog.component.scss']
})
export class StudentRequestDialogComponent implements OnInit {
  public title!: string;
  constructor(
    private _studentServices: StudentService,
    private _dialogRef: MatDialogRef<StudentRequestDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {title: string}) { }

  ngOnInit(){
    this.title =this.data.title;
  }

  clickHandling(): void {
    this._dialogRef.close(true);
  }

}
