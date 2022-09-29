import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { CourseService } from '../services/course.service';

@Component({
  selector: 'app-course-container',
  templateUrl: './course-container.component.html',
  styleUrls: ['./course-container.component.scss']
})
export class CourseContainerComponent implements OnInit {
  private $unsubscribe = new Subject<void>();

  constructor(
    private _courseService: CourseService,
    private _activcedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this._activcedRoute.params.pipe(takeUntil(this.$unsubscribe))
      .subscribe(params => {
        console.log(params.id)
        this._courseService.courseId = params.id;
        if (this._courseService.courseId)
          this._courseService.getCourseById(this._courseService.courseId).pipe(
            map(res => res.data), takeUntil(this.$unsubscribe)).subscribe(course => this._courseService.setCourse(course));
      })
  }

  loadCourse() {

  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }

}
