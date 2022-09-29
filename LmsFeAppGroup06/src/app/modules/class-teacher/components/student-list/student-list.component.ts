import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { ClassReadDto } from '../../../../Dto/classDto';
import { PageDataDto } from '../../../../Dto/pageDataDto';
import { UserReadDto } from '../../../../Dto/userDto';
import { ClassService } from '../../services/class.service';
import { AssignmentComponent } from '../assignment/assignment.component';

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.scss']
})
export class StudentListComponent implements OnInit {
  private $unsubscriber = new Subject();

  $students?: Observable<PageDataDto<UserReadDto[]>>;
  isLoading: boolean = false;

  private pageEvent: PageEvent = { pageIndex: 0, pageSize: 10, length: 10 };
  currentClass?: ClassReadDto;


  @ViewChild('paginator') paginator!: MatPaginator;

  constructor(
    private _classService: ClassService,
    private _matDialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadStudents();
    this.currentClass = this._classService.class;
    this._classService.classEmit.pipe(takeUntil(this.$unsubscriber)).subscribe(_class => {
      this.currentClass = _class;
    })
  }

  loadStudents() {
    this.isLoading = true
    this.$students = this._classService.getStudents(this.pageEvent.pageIndex, this.pageEvent.pageSize)
      .pipe(map(res => res.data), tap(data => {
        this.isLoading = false;
        this.paginator.length = data.count;
        console.log(data.count);
      }, data => this.isLoading = false));
  }

  changePage(pageEvent: PageEvent){
    this.pageEvent = pageEvent;
    this.loadStudents();
  }
  
  deleteStudentHandling(student: UserReadDto) {
    const dialog = this._matDialog.open(DeleteDialogComponent, { data: { title: 'Delete Student', deleteName: student.username } })
    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(data => {
      if (data) {
        this.isLoading = true
        this._classService.deleteStudent({ ...student }).pipe(takeUntil(this.$unsubscriber))
          .subscribe(res => {
            this.isLoading = false
            this.loadStudents();
          }, res => {
            this.isLoading = false;
          })
      }
    })
  }

  onChooseAssistant() {
    const dialog = this._matDialog.open(AssignmentComponent, {data: false});
    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(
      data => {

      }
    )
  }

  onChooseClassAdmin() {
    const dialog = this._matDialog.open(AssignmentComponent, {data: true});
    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(
      data => {

      }
    )
  }


  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}
