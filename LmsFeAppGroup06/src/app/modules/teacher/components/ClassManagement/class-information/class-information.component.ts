import { Component, Input, OnInit } from '@angular/core';
import { ClassReadDto } from '../../../../../Dto/classDto';

@Component({
  selector: 'app-class-information',
  templateUrl: './class-information.component.html',
  styleUrls: ['./class-information.component.scss']
})
export class ClassInformationComponent implements OnInit {

  @Input() classData!: ClassReadDto;

  constructor() { }

  ngOnInit(): void {
  }

}
