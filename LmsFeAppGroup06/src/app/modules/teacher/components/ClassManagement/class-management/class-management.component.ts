import { Component, OnInit } from '@angular/core';
import { ClassReadDto } from '../../../../../Dto/classDto';

@Component({
  selector: 'app-class-management',
  templateUrl: './class-management.component.html',
  styleUrls: ['./class-management.component.scss']
})
export class ClassManagementComponent implements OnInit {

  focusClass?: ClassReadDto;

  constructor() { }

  ngOnInit(): void {
  }

  clickRowHandling(event: ClassReadDto) {
    this.focusClass = event;
  }
}
