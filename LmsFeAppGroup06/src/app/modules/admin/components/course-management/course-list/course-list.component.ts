import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { CourseReadDto } from '../../../../../Dto/CourseDto';
import { PageDataDto } from '../../../../../Dto/pageDataDto';
import { AdminService } from '../../../services/admin.service';
import { LockDialogComponent } from '../../lock-dialog/lock-dialog.component';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.scss']
})
export class CourseListComponent implements OnInit {

  private $unsubscribe = new Subject();
  isLoading: boolean = false;
  isLock?:boolean;
  title: string = '';

  $courses?: Observable<PageDataDto<CourseReadDto[]>>;
  searchValue: string = '';

  @ViewChild('searchInput') searchInput!: ElementRef;
  @Output('onClickRow') clickEmit = new EventEmitter<CourseReadDto>();


  constructor(
    private _matDialogService: MatDialog,
    private _adminService: AdminService,
    ) { }

  ngOnInit(): void {
    this.loadCourse(this.searchValue);
  }

  loadCourse(searchValue: string) {
    this.isLoading = true;
    this.$courses = this._adminService
        .getCourseList(searchValue).pipe(map(res => res.data), tap(
          res => {
            this.isLoading = false;
        }));
  }

  searchHandling() {
    this.searchValue = this.searchInput.nativeElement.value;
    this.loadCourse(this.searchValue);
  }
  rowClickHandling(_course: CourseReadDto) {
    this.clickEmit.emit(_course);
  }

  changeLock(id:number)
  {
    this._adminService.changeLockCourse(id).pipe(takeUntil(this.$unsubscribe)).subscribe(
      res => {
        this.isLoading = true;
        this.loadCourse(this.searchValue);
      })
  }

  changeTitle(type: string){
    if(type == 'lock'){
      this.title = 'unlock';
      return;
    }
    this.title = 'lock';
  }

  openEditDialog(course: CourseReadDto) {
    const editDialog = this._matDialogService.open(LockDialogComponent, {
      width: '20%',
      data: {title: this.title}
    })
    editDialog.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(
      result => {
        if (result)
          this.changeLock(course.id);
      }
    )
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }
}
