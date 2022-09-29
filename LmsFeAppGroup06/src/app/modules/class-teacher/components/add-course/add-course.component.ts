import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { CourseReadDto } from '../../../../Dto/CourseDto';
import { PageDataDto } from '../../../../Dto/pageDataDto';
import { ClassCourseService } from '../../services/class-course.service';
import { ClassService } from '../../services/class.service';

@Component({
  selector: 'app-add-course',
  templateUrl: './add-course.component.html',
  styleUrls: ['./add-course.component.scss']
})
export class AddCourseComponent implements OnInit {
  private $unsubsriber = new Subject();
  $courses?: Observable<PageDataDto<CourseReadDto[]>>;
  pageEvent: PageEvent = { pageIndex: 0, length: 10, pageSize: 10 };
  searchValue: string = '';
  isLoading: boolean = false;
  messager: string = ''
  isAdded: boolean = false;

  @ViewChild('paginator') paginator!: MatPaginator;

  constructor(
    private _classCourseService: ClassCourseService,
    private _classService: ClassService
  ) { }

  ngOnInit(): void {
    this.loadCourse();
  }

  pageChange(pageEvent: PageEvent) {
    this.pageEvent = pageEvent;
    this.loadCourse();
  }

  loadCourse() {
    this.isLoading = true;
    this.$courses = undefined;
    this.messager = '';
    console.log(this.messager)
    this.$courses = this._classCourseService.getAllCourse(this.pageEvent.pageIndex,
      this.pageEvent.pageSize, this.searchValue).pipe(takeUntil(this.$unsubsriber), map(res => res.data),
        tap(data => {
          this.isLoading = false;
          this.paginator.length = data.count;
          if (data.count == 0) {
            this.messager = 'Not Found';
          }
        }, res => this.isLoading = false))
  }

  onChange(event: any) {
    this.searchValue = event.target.value;
    this.loadCourse();
  }

  addCourse(course: CourseReadDto) {
    this.isLoading = true;
    this.messager = '';
    this._classService.addCourseToclass(course.id).pipe(takeUntil(this.$unsubsriber))
      .subscribe(res => {
        this.isLoading = false;
        this.isAdded = true;
        this.messager = "Success"
      }, res => {
        this.isLoading = false;
      })
  }

}
