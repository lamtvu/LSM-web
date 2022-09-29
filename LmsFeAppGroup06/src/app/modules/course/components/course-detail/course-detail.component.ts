import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ContentReadDto } from '../../../../Dto/ContentDto';
import { CourseReadDto } from '../../../../Dto/CourseDto';
import { SectionReadDto } from '../../../../Dto/SectionDto';
import { CourseService } from '../../services/course.service';
import { SectionService } from '../../services/section.service';
import { CourseContentDialogComponent } from '../course-content-dialog/course-content-dialog.component';
import { CourseContentComponent } from '../course-content/course-content.component';

@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html',
  styleUrls: ['./course-detail.component.scss']
})
export class CourseDetailComponent implements OnInit {

  $unsubscriber = new Subject();
  public sections: SectionReadDto[] = [];
  course?: CourseReadDto;

  panelOpenState = false;

  private $unsubcriber = new Subject();
  isLoading: boolean = false;
  constructor(public dialog: MatDialog,
    private _sectionService: SectionService,
    private _courseService: CourseService,
    public activatedRouteService: ActivatedRoute) { }

  ngOnInit(): void {
    this.course = this._courseService.course;
    this.onloadSection();
    console.log(this._courseService.course);
    this._courseService.courseEmit.pipe(takeUntil(this.$unsubcriber)).subscribe(
      course => {
        this.course = course;
        this.onloadSection();
      }
    )
  }

  openDialog(content: ContentReadDto) {
    const contentDialog = this.dialog.open(CourseContentDialogComponent, { data: content, disableClose: true });
  }

  onloadSection() {
    this.isLoading = true;
    if (!this._courseService.courseId) return;
    console.log(this._courseService.courseId)
    this._sectionService
      .getSections(this._courseService.courseId).pipe(takeUntil(this.$unsubcriber)).subscribe(
        res => {
          this.isLoading = false;
          this.sections = res.data;
          this._sectionService.section = res.data;
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
