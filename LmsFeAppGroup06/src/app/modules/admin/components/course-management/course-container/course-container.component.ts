import { Component, OnInit } from '@angular/core';
import { CourseReadDto } from '../../../../../Dto/CourseDto';

@Component({
  selector: 'app-course-container',
  templateUrl: './course-container.component.html',
  styleUrls: ['./course-container.component.scss']
})
export class CourseContainerComponent implements OnInit {

  focusClass?: CourseReadDto;

  constructor() { }

  ngOnInit(): void {
  }

  clickRowHandling(event: CourseReadDto) {
    this.focusClass = event;
  }

}
