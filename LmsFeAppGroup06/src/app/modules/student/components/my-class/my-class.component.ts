import { Component, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { ClassReadDto } from '../../../../Dto/classDto';
import { PageDataDto } from '../../../../Dto/pageDataDto';
import { StudentService } from '../../services/student.service';

@Component({
  selector: 'app-my-class',
  templateUrl: './my-class.component.html',
  styleUrls: ['./my-class.component.scss']
})
export class MyClassComponent implements OnInit, OnDestroy {

  private $unsubscribe = new Subject<void>();
  isLoading: boolean = false;

  $classes?: Observable<PageDataDto<ClassReadDto[]>>
  pageEvent: PageEvent = {pageIndex : 0, pageSize: 10, length: 10 };

  @ViewChild('paginator') paginator!: MatPaginator;

  @Output('onClickRow') clickEmit = new EventEmitter<ClassReadDto>();
  constructor(
    public _dialogService: MatDialog, 
    private _studentService: StudentService) { }

  ngOnInit(): void {
    this.loadList(0, this.pageEvent.length);
    this._studentService.editEmit.pipe(takeUntil(this.$unsubscribe)).subscribe(
      res =>{
      this.loadList(this.pageEvent.pageIndex, this.pageEvent.pageSize);
    })
  }

  loadList(page: number, limit: number) {
    this.isLoading = true
    this.$classes = this._studentService
        .getStudingClass(page,limit).pipe(map(res => res.data), tap(res => 
          {
            this.isLoading = false;
            this.pageEvent.length = res.count;
          }));
  }

  pageHandling(event: PageEvent) {
    this.pageEvent = event;
    this.loadList(event.pageIndex, event.pageSize);
  }

  rowClickHandling(_class: ClassReadDto) {
    this.clickEmit.emit(_class);
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }

}
