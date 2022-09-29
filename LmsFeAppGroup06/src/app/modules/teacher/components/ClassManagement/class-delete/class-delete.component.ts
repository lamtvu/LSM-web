import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import { ClassReadDto } from '../../../../../Dto/classDto';
import { TeacherService } from '../../../services/teacher.service';

@Component({
  selector: 'app-class-delete',
  templateUrl: './class-delete.component.html',
  styleUrls: ['./class-delete.component.scss']
})
export class ClassDeleteComponent implements OnInit {

  public $unsubcriber = new Subject<void>();

  isLoading: boolean = false;
  error = { isError: false, message: '' };


  constructor(
    public dialogRef: MatDialogRef<ClassDeleteComponent>,
    @Inject(MAT_DIALOG_DATA) public deleteClass: ClassReadDto,
    private _teacherServce: TeacherService
  ) { }

  ngOnInit(): void { }

  deleteHandling() {
    this.isLoading = true;
    this.error.isError = false;
    this._teacherServce.deleteClass(this.deleteClass.id)
      .pipe(takeUntil(this.$unsubcriber)).subscribe(
        res => {
          this.isLoading = false;
          this.dialogRef.close(true);
        },
        res => {
          console.log(res.error);
          this.isLoading = false;
          this.error = { isError: false, message:res.error.messager };
        }
      )
}

}
