import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, take, takeUntil, tap } from 'rxjs/operators';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { CourseReadDto } from '../../../../Dto/CourseDto';
import { PageDataDto } from '../../../../Dto/pageDataDto';
import { ClassCourseService } from '../../services/class-course.service';
import { ClassService } from '../../services/class.service';
import { AddCourseComponent } from '../add-course/add-course.component';

@Component({
  selector: 'app-teacher-course-management',
  templateUrl: './teacher-course-management.component.html',
  styleUrls: ['./teacher-course-management.component.scss']
})
export class TeacherCourseManagementComponent implements OnInit {
  private $unsubscriber = new Subject<void>();
  $course?: Observable<PageDataDto<CourseReadDto[]>>;
  pageEvent: PageEvent = { pageIndex: 0, pageSize: 10, length: 10 };
  searchValue: string = '';
  isLoading: boolean = false;

  @ViewChild('paginator') paginator!: MatPaginator;

  constructor(
    private _classService: ClassService,
    private _ClassCourseService: ClassCourseService,
    private _matDialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadCourse();
  }

  loadCourse(): void {
    if (!this._classService.classId) return;
    this.isLoading = true;
    this.$course = this._ClassCourseService.getCourses(this._classService.classId,
      this.pageEvent?.pageIndex, this.pageEvent?.pageSize, this.searchValue)
      .pipe(map(res => res.data), takeUntil(this.$unsubscriber), tap(data => {
        this.isLoading = false;
        this.paginator.length = data.count;
      }, res => this.isLoading = false));
  }

  pageChange(pageEvent: PageEvent) {
    console.log('loading');
    this.pageEvent = pageEvent;
    this.loadCourse();
  }

  searchHandling(event: any) {
    this.searchValue = event.target.value;
    this.loadCourse()
  }

  deleteCourse(course: CourseReadDto) {
    const dialog = this._matDialog.open(DeleteDialogComponent, { data: { title: 'Delete Course', deleteName: course.name } });
    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(
      data => {
        console.log(data);
        if (!data) return;
        this.isLoading = true;
        this._classService.removeCourseInClass(course.id).pipe(takeUntil(this.$unsubscriber))
          .subscribe(res => {
            this.loadCourse();
          }, res => {
            this.isLoading = false;
          })
      }
    )
  }

  addCourse() {
    const dialog = this._matDialog.open(AddCourseComponent, {
      width: '100%',
      height: '90%',
      disableClose: true
    });

    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(
      data => {
        if (data) {
          this.loadCourse();
        }
      }
    )
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}
