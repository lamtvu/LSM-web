import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CourseReadDto } from '../../../../../Dto/CourseDto';
import { CourseCreateComponent } from '../course-create/course-create.component';

@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html',
  styleUrls: ['./course-detail.component.scss']
})
export class CourseDetailComponent implements OnInit {

  public url: string = 'https://thang.odoo.com/web/static/src/img/placeholder.png';
  $unsubscriber = new Subject();
  @Input() courseData!: CourseReadDto;
  constructor(
    private _matDialogService: MatDialog
  ) { }

  ngOnInit(): void {
  }


  openDialog() {
    const editDialog = this._matDialogService.open(CourseCreateComponent, { data: this.courseData, width: '30%' });
    editDialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(res => {
      if (res)
        this.courseData = res;
    })
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}
