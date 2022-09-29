import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { TeacherService } from 'src/app/modules/teacher/services/teacher.service';
import { ClassReadDto } from '../../../../Dto/classDto';
import { ClassService } from '../../services/class.service';

@Component({
  selector: 'app-class-teacher-container',
  templateUrl: './class-teacher-container.component.html',
  styleUrls: ['./class-teacher-container.component.scss']
})
export class ClassTeacherContainerComponent implements OnInit {
  private $unsubscribe = new Subject<void>();
  links: { url: string, label: string }[] = [
    { url: `class-detail`, label: 'Class Detail' },
    { url: `course-management`, label: 'Course Management' },
    { url: `student-management`, label: 'Student Management' },
    { url: `score-management`, label: 'Score Management' },
    { url: `report-list`, label: 'Reports' },
  ];
  ;
  activeLink: string = '';
  currentClass?: ClassReadDto;

  @Output('focusClassChangage') focusClassEmmit = new EventEmitter<boolean>()

  constructor(
    private _router: Router,
    private _teacherService: TeacherService,
    private _classService: ClassService,
    private _activedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this._activedRoute.params.subscribe(data => {
      const classId = data.id;
      this.activeLink = this._router.url;
      this.loadClass(classId);
      this._classService.setClassId(classId);
    });
  }

  loadClass(id: number): void {
    this._teacherService.getClass(id)
      .pipe(takeUntil(this.$unsubscribe)).subscribe(
        res => {
          this._classService.setClass(res.data);
          this.currentClass = res.data;
        },
        res => {
        }
      );
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }
}
