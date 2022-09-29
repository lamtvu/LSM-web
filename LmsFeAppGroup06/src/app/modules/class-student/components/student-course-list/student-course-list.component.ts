import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { CourseReadDto } from '../../../../Dto/CourseDto';
import { PageDataDto } from '../../../../Dto/pageDataDto';
import { ClassCourseService } from '../../services/class-course.service';
import { ClassService } from '../../services/class.service';

@Component({
  selector: 'app-student-course-list',
  templateUrl: './student-course-list.component.html',
  styleUrls: ['./student-course-list.component.scss']
})
export class StudentCourseListComponent implements OnInit {
  private $unsubscriber = new Subject();
  $courses?: Observable<PageDataDto<CourseReadDto[]>>;

  pageEvent: PageEvent = { pageIndex: 0, pageSize: 10, length: 10 };
  @ViewChild('paginator') paginator!: MatPaginator;

  isLoading: boolean = false;

  constructor(
    private _classCourseService: ClassCourseService,
    private _classSercvice: ClassService
  ) { }

  ngOnInit(): void {
    this.loadCourse();
  }

  loadCourse() {
    if (!this._classSercvice.classId) return;
    this.isLoading = true;
    this.$courses = this._classCourseService.getCourses(this._classSercvice.classId,
      this.pageEvent.pageIndex, this.pageEvent.pageSize, '').pipe(
        takeUntil(this.$unsubscriber),
        map(res => res.data), tap(data => {
          this.isLoading = false;
          this.paginator.length = data.count;
        }, res => {
          this.isLoading = false;
        })
      )
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

  pageChange(pageEvent: PageEvent) {
    this.pageEvent = pageEvent;
    this.loadCourse();
  }

}
