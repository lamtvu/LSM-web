import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { ClassReadDto } from '../../../../../Dto/classDto';
import { PageDataDto } from '../../../../../Dto/pageDataDto';
import { UserReadDto } from '../../../../../Dto/userDto';
import { ClassService } from '../../../services/class.service';

@Component({
  selector: 'app-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.scss']
})
export class StudentListComponent implements OnInit {
  private $unsubscriber = new Subject();

  $students?: Observable<PageDataDto<UserReadDto[]>>;
  isLoading: boolean = false;

  private pageEvent: PageEvent = { pageIndex: 0, pageSize: 10, length: 10 };
  currentClass?: ClassReadDto;


  @ViewChild('paginator') paginator!: MatPaginator;

  constructor(
    private _classService: ClassService,
    private _matDialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadStudents();
    this.currentClass = this._classService.class;
    this._classService.classEmit.pipe(takeUntil(this.$unsubscriber)).subscribe(_class => {
      this.currentClass = _class;
    })
  }

  loadStudents() {
    this.isLoading = true
    this.$students = this._classService.getStudents(this.pageEvent.pageIndex, this.pageEvent.pageSize)
      .pipe(map(res => res.data), tap(data => {
        this.isLoading = false;
        this.paginator.length = data.count;
        console.log(data.count);
      }, data => this.isLoading = false));
  }

  changePage(pageEvent: PageEvent){
    this.pageEvent = pageEvent;
    this.loadStudents();
  }
}
