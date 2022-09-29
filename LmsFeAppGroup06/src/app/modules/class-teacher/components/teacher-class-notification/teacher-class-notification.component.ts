import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, pipe, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { AnnouncenmentReadDto } from '../../../../Dto/AnnouncenmentDto';
import { ClassService } from '../../services/class.service';
import { NotificationService } from '../../services/notification.service';
import { CreateNotifyComponent } from '../create-notify/create-notify.component';

@Component({
  selector: 'app-teacher-class-notification',
  templateUrl: './teacher-class-notification.component.html',
  styleUrls: ['./teacher-class-notification.component.scss']
})
export class TeacherClassNotificationComponent implements OnInit {
  private $unsubscribe = new Subject();
  private classId?: number;
  isLoading: boolean = false;

  $notifications?: Observable<AnnouncenmentReadDto[]>

  constructor(
    private _classService: ClassService,
    private _notificationService: NotificationService,
    private _matDialogService: MatDialog
  ) { }

  ngOnInit(): void {
    this.classId = this._classService.classId;
    if (this.classId)
      this.loadNotification();
    this._classService.classIdEmit.pipe(takeUntil(this.$unsubscribe)).subscribe(id => {
      this.classId = id;
      this.loadNotification();
    })
  }

  loadNotification() {
    this.isLoading = true;
    if (this.classId)
      this.$notifications = this._notificationService.getNotifications(this.classId)
        .pipe(takeUntil(this.$unsubscribe), map(res => res.data), tap(res => this.isLoading = false));
  }


  openCreateDialog() {
    const createPlanDialog = this._matDialogService.open(CreateNotifyComponent,
      {
        minWidth: '30%',
        minHeight: '50%',
        disableClose: true,
      });
    createPlanDialog.beforeClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
      if (result)
        this.loadNotification();
    });
  }

  openEditDialog(notification: AnnouncenmentReadDto) {
    const createPlanDialog = this._matDialogService.open(CreateNotifyComponent,
      {
        minWidth: '30%',
        minHeight: '50%',
        disableClose: true,
        data: {...notification}
      });
    createPlanDialog.beforeClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
      if (result)
        this.loadNotification();
    });
  }

  openDeleteDialog(notification: AnnouncenmentReadDto) {
    const deleteDialog = this._matDialogService.open(DeleteDialogComponent, {
      data: { title: 'Delete notification', deleteName: notification.title }
    })
    deleteDialog.beforeClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
      if (result) {
        this._notificationService.deleteNofity(notification.id).pipe(takeUntil(this.$unsubscribe))
          .subscribe(res => this.loadNotification());
      }
    });
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }
}
