import { Component, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { AnnouncenmentReadDto } from '../../../../Dto/AnnouncenmentDto';
import { ClassService } from '../../services/class.service';
import { PlanService } from '../../services/plan.service';

@Component({
  selector: 'app-student-class-notification',
  templateUrl: './student-class-notification.component.html',
  styleUrls: ['./student-class-notification.component.scss']
})
export class StudentClassNotificationComponent {
  private $unsubscriber = new Subject();
  isLoading: boolean = false;
  $notification?: Observable<AnnouncenmentReadDto[]>

  constructor(
    private _classService: ClassService,
    private _planService: PlanService
  ) { }

  ngOnInit(): void {
    this.loadNotification();
  }
  loadNotification() {
    if (!this._classService.classId) return;
    this.isLoading = true
    this.$notification = this._planService.getNotifications(this._classService.classId)
      .pipe(takeUntil(this.$unsubscriber)
        , map(res => res.data),
        tap(
          data => this.isLoading = false,
          data => this.isLoading = false)
      )
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

}
