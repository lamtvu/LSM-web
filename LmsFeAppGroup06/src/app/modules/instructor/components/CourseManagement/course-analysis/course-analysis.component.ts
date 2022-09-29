import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CourseReadDto } from '../../../../../Dto/CourseDto';
import { CourseService } from '../../../services/course.service';

@Component({
  selector: 'app-course-analysis',
  templateUrl: './course-analysis.component.html',
  styleUrls: ['./course-analysis.component.scss']
})
export class CourseAnalysisComponent implements OnInit {

  private $unsubcriber = new Subject();

  public courses!: CourseReadDto[];

  isLoading: boolean = false;
  constructor(
    private _courseService: CourseService
  ) { }

  ngOnInit(): void {
    this.loadCourse();
  }

  public courseId: number = 1;

  loadCourse() {
    this.isLoading = true
    this._courseService
      .getCourses('', 0, 5).pipe(takeUntil(this.$unsubcriber)).subscribe(
        res => {
          this.courses = res.data.data;
          this.courseId = this.courses[0].id;
        }
      )
  }

  ngOnDestroy(): void {
    this.$unsubcriber.next();
    this.$unsubcriber.complete();
  }
}
