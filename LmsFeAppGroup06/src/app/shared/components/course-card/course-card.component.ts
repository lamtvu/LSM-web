import { Component, Input, OnInit } from '@angular/core';
import { CourseReadDto } from '../../../Dto/CourseDto';

@Component({
  selector: 'app-course-card',
  templateUrl: './course-card.component.html',
  styleUrls: ['./course-card.component.scss']
})
export class CourseCardComponent implements OnInit {
  @Input() course!: CourseReadDto;

  constructor() { }

  ngOnInit(): void {
  }

}
