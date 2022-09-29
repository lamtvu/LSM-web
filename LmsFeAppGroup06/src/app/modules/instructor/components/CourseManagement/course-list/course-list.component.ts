import { Component, ElementRef, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { EventEmitter } from '@angular/core';
import { CourseService } from '../../../services/course.service';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { CourseCreateComponent } from '../course-create/course-create.component';
import { CourseReadDto } from '../../../../../Dto/CourseDto';
import { PageDataDto } from '../../../../../Dto/pageDataDto';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.scss']
})
export class CourseListComponent implements OnInit, OnDestroy {

  private $unsubscribe = new Subject();
  isLoading: boolean = false;

  $courses?: Observable<PageDataDto<CourseReadDto[]>>
  pageEvent: PageEvent = {pageIndex : 0, pageSize: 10, length: 10 };
  searchValue: string = '';

  @ViewChild('searchInput') searchInput!: ElementRef;
  @ViewChild('paginator') paginator!: MatPaginator;

  @Output('onClickRow') clickEmit = new EventEmitter<CourseReadDto>();
  constructor(
    public _dialogService: MatDialog,
    private _formBuilder: FormBuilder,
    private _courseService: CourseService,
    ) { }

  ngOnInit(): void {
    this.loadCourse(this.searchValue, 0, this.pageEvent.length);
    this._courseService.editEmit.pipe(takeUntil(this.$unsubscribe)).subscribe(res =>{
      this.loadCourse(this.searchValue, this.pageEvent.pageIndex, this.pageEvent.pageSize);
    })
  }

  openDeleteCourseDialog(_course: CourseReadDto) {
    const dialogRef = this._dialogService.open(DeleteDialogComponent, {data: {title: 'Course Delete', deleteName: _course.name}});

    dialogRef.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
      if (result) {
        this._courseService.deleteCourse(_course.id).pipe(takeUntil(this.$unsubscribe)).subscribe(res =>{
          this.loadCourse(this.searchValue, this.pageEvent.pageIndex, this.pageEvent.pageSize);
        },
        res => {

        })
      }
    });
  }

  loadCourse(searchValue: string, page: number, limit: number) {
    this.isLoading = true
    this.$courses = this._courseService
        .getCourses(searchValue,page,limit).pipe(map(res => res.data), tap(res => {this.isLoading = false;
        this.pageEvent.length = res.count;
        }));
  }

  pageHandling(event: PageEvent) {
    this.pageEvent = event;
    this.loadCourse(this.searchValue, event.pageIndex, event.pageSize);
  }

  searchHandling() {
    this.searchValue = this.searchInput.nativeElement.value;
    this.loadCourse(this.searchValue,0,this.pageEvent.pageSize);
    this.paginator.pageIndex = 0;
  }

  rowClickHandling(_course: CourseReadDto) {
    this.clickEmit.emit(_course);
  }

  openCreateCourseDialog() {
    const dialogRef = this._dialogService.open(CourseCreateComponent,
      { disableClose: true, autoFocus: false, minWidth: '50%', height: '50%' });
    dialogRef.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
      console.log(result)
      if (result) {
        this.loadCourse(this.searchValue,0,this.pageEvent.pageSize);
      }
    });
  }
  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }
}
