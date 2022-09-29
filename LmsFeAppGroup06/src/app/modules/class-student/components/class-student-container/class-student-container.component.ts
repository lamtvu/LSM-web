import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, take, takeUntil, tap } from 'rxjs/operators';
import { ClassReadDto } from '../../../../Dto/classDto';
import { ClassService } from '../../services/class.service';
import { SendReportDialogComponent } from '../send-report-dialog/send-report-dialog.component';

@Component({
  selector: 'app-class-student-container',
  templateUrl: './class-student-container.component.html',
  styleUrls: ['./class-student-container.component.scss']
})
export class ClassStudentContainerComponent implements OnInit {
  private $unsubscriber = new Subject();
  $class!: Observable<ClassReadDto>;
  links: { url: string, label: string }[] = [
    { url: `class-detail`, label: 'Class Detail' },
    { url: `course-list`, label: 'Course List' },
    { url: `student-management`, label: 'Student List' },
    { url: `my-score`, label: 'Score' },
  ];;
  activeLink: string = '';

  constructor(
    private _router: Router,
    private _activedRoute: ActivatedRoute,
    private _classService: ClassService,
    private _matDialogService: MatDialog
  ) { }

  ngOnInit(): void {
    this.activeLink = this._router.url;
    this._activedRoute.params.pipe(takeUntil(this.$unsubscriber)).subscribe(data => {
      this._classService.setClassId(data.id);
      this.loadClass();
    });

  }

  loadClass() {
    this.$class = this._classService.getClass().pipe(map(res => res.data),
      tap(data => {
        this._classService.setClass(data);
      }));
  }

  onSendReport(){
    const editDialog = this._matDialogService.open(SendReportDialogComponent, {data: 'abc', width: '17%'});
    editDialog.afterClosed().pipe(takeUntil(this.$unsubscriber)).subscribe(res => {
    },
    res => {

    });
  }

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

}
