import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params } from '@angular/router';
import { ClassReadDto } from '../../../../Dto/classDto';
import { StudentService } from '../../services/student.service';

@Component({
  selector: 'app-class-cart',
  templateUrl: './class-cart.component.html',
  styleUrls: ['./class-cart.component.scss']
})
export class ClassCartComponent implements OnInit {
  @Input() _class?: ClassReadDto

  constructor(public _dialogService: MatDialog, private _studentService: StudentService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
   
  }

}
