import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { CommentReadDto } from '../../../../../Dto/CommentDto';
import { CourseService } from '../../../services/course.service';

@Component({
  selector: 'app-course-review',
  templateUrl: './course-review.component.html',
  styleUrls: ['./course-review.component.scss']
})
export class CourseReviewComponent implements OnInit {

  @Input('getCourseId') courseId?: number;

  private $unsubcriber = new Subject();
  public comments!: CommentReadDto[];

  pageEvent: PageEvent = { pageIndex: 0, pageSize: 10, length: 10 };
  isLoading: boolean = false;

  @ViewChild('paginator') paginator!: MatPaginator;

  constructor(
    private _courseService: CourseService
  ) { }

  ngOnInit(): void {
    this.loadReviews();
  }

  ngOnChanges(): void {
    this.pageEvent = { pageIndex: 0, pageSize: 10, length: 10 };
    this.loadReviews();
  }

  loadReviews() {
    this.isLoading = true
    if (!this.courseId) {
      return;
    }
    this._courseService
      .getCommentReviewAll(this.courseId, this.pageEvent.pageIndex, this.pageEvent.pageSize).pipe(takeUntil(this.$unsubcriber),
        map(res => res.data)).subscribe(
          res => {
            this.comments = res.data;
            if (this.paginator)
              this.paginator.length = res.count;
          },
          res => {
            console.log(res.error);
          })
  }

  ngOnDestroy(): void {
    this.$unsubcriber.next();
    this.$unsubcriber.complete();
  }

  pageChange(event: PageEvent) {
    this.pageEvent = event;
    this.loadReviews();

  }

}
