import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, takeWhile, tap } from 'rxjs/operators';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { PageDataDto } from '../../../../Dto/pageDataDto';
import { RequestStudentReadDto } from '../../../../Dto/requestDto';
import { ClassService } from '../../services/class.service';
import { RequestStudentService } from '../../services/request-student.service';
import { InviteDialogComponent } from '../invite-dialog/invite-dialog.component';

@Component({
  selector: 'app-request-list',
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.scss']
})
export class RequestListComponent implements OnInit {

  private $unsubscriber = new Subject();
  private pageEvent: PageEvent = { pageIndex: 0, pageSize: 10, length: 10 };
  $requestStudents?: Observable<PageDataDto<RequestStudentReadDto[]>>;
  isLoading: boolean = false;

  @ViewChild('paginator') paginator!: MatPaginator;

  constructor(
    private _classService: ClassService,
    private _requestStudentService: RequestStudentService,
    private _matDialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadRequest();
  }

  onDeleteRequest(request: RequestStudentReadDto) {
    const dialog = this._matDialog.open(DeleteDialogComponent, {
      data: {
        title: 'Delete Request',
        deleteName: 'request of ' + request.sender.username
      }
    })
    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(data => {
      if (data) {
        this.isLoading = true;
        this._requestStudentService.deleteRequest(request.id).pipe(takeUntil(this.$unsubscriber))
          .subscribe(res => {
            this.loadRequest();
            this.isLoading = false;
          }, res => {
            this.isLoading = false;
          })
      }
    })
  }

  onDeleteAllRequest() {
    const dialog = this._matDialog.open(DeleteDialogComponent, {
      data: {
        title: 'Delete Request',
        deleteName: 'all request'
      }
    })
    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(data => {
      if (!data) return;
      if (!this._classService.classId) return;
      this.isLoading = true;
      this._requestStudentService.deleteAllRequest(this._classService.classId).pipe(takeUntil(this.$unsubscriber))
        .subscribe(
          res => {
            this.loadRequest();
            this.isLoading = false;
          },
          res => {
            this.isLoading = false;
          }
        )
    })
  }

  loadRequest() {
    if (!this._classService.classId) return;
    this.isLoading = true;
    this.$requestStudents = this._requestStudentService.getRequests(this._classService.classId, this.pageEvent.pageIndex, this.pageEvent.pageSize)
      .pipe(map(res => res.data), tap(data => {
        this.paginator.length = data.count;
        this.isLoading = false
      }, data => this.isLoading = false))
  }

  acceptRequestHandling(request: RequestStudentReadDto) {
    if (!this._classService.classId) return;
    this.isLoading = true;
    this._requestStudentService.deleteRequest(request.id).pipe(takeUntil(this.$unsubscriber))
      .subscribe(res => {
        this.loadRequest();
        this.isLoading = false;
      }, res => {
        this.isLoading = false;
      })
    this._classService.addStudent(this._classService.classId, { username: request.sender.username }).pipe(takeUntil(this.$unsubscriber))
      .subscribe();
  }

  inviteHandling() {
    const dialog = this._matDialog.open(InviteDialogComponent, {disableClose: true});
    dialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(data => {

    })
  }

  changePage(pageEvent: PageEvent) {
    this.pageEvent = pageEvent;
    this.loadRequest();
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }
}
