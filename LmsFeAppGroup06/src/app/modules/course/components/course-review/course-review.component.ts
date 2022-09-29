import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from 'src/app/shared/services/user.service';
import { CommentReadDto } from '../../../../Dto/CommentDto';
import { UserReadDto } from '../../../../Dto/userDto';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-course-review',
  templateUrl: './course-review.component.html',
  styleUrls: ['./course-review.component.scss']
})
export class CourseReviewComponent implements OnInit {

  currentUser?: UserReadDto;
  isComment: boolean = false;
  rating = 0;
  starCount = 5;
  ratingArr: boolean[] = [];
  commentlist?: CommentReadDto[];

  private $unsubscriber = new Subject();
  isLoading: boolean = false;
  public courseId!: number;
  commentForm = this._formBuilder.group({
    comment: ['', [Validators.required]]
  })

  errorHandling(controlName: string, errorName: string) {
    return this.commentForm.controls[controlName].hasError(errorName);
  }

  constructor(
    private _formBuilder: FormBuilder,
    private _courseService: CourseService,
    private _userService: UserService,
    public activatedRouteService: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    if (this._courseService.courseId) {
      this.courseId = this._courseService.courseId;
    }
    this.currentUser = this._userService.user;
    console.log(this.currentUser)
    this._userService.userEmmit.pipe(takeUntil(this.$unsubscriber)).subscribe(
      user => {
        this.currentUser = user;
      });
    this.ratingArr = Array(this.starCount).fill(false);
    this.loadComment();

  }

  // Cancel Button
  onCancelComment() {
    this.isComment = !this.isComment;
    (<HTMLInputElement>document.getElementById('myInput')).value = '';
    this.rating = 0;
  }

  // Handle Star
  returnStar(i: number) {
    if (this.rating >= i + 1) {
      return 'star';
    } else {
      return 'star_border';
    }
  }

  onClick(i: number) {
    this.rating = i + 1;
  }


  onCreateComment() {
    if (!this.commentForm.valid) {
      return;
    }
    this.isLoading = true;
    this._courseService.createcourseReview(this.courseId, { ...this.commentForm.value, star: this.rating })
      .pipe(takeUntil(this.$unsubscriber)).subscribe(
        res => {
          this.isLoading = false;
          this.isComment = false;
          this.commentForm.reset();
          this.loadComment();
        },
        res => {
          this.isLoading = false;
          console.log(res.error);
        }
      )
  }

  loadComment() {
    this.isLoading = true;
    this._courseService.getcoursesReviewAll(this.courseId).pipe(takeUntil(this.$unsubscriber)).subscribe(
      res => {
        this.isLoading = false;
        this.commentlist = res.data;
      },
      res => {
        this.isLoading = false;
        console.log(res.error);
      }
    )
  }


  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

}
