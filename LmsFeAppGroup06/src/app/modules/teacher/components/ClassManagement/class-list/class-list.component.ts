import { Component, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { data } from 'autoprefixer';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil } from 'rxjs/operators';
import { ClassReadDto } from '../../../../../Dto/classDto';
import { PageDataDto } from '../../../../../Dto/pageDataDto';
import { TeacherService } from '../../../services/teacher.service';
import { ClassCreateComponent } from '../class-create/class-create.component';
import { ClassDeleteComponent } from '../class-delete/class-delete.component';

@Component({
  selector: 'app-class-list',
  templateUrl: './class-list.component.html',
  styleUrls: ['./class-list.component.scss']
})
export class ClassListComponent implements OnInit, OnDestroy {
  private $unsubscribe = new Subject<void>();
  classList?: PageDataDto<ClassReadDto[]>;
  isLoading: boolean = false;
  pageEvent: PageEvent = { pageIndex: 0, pageSize: 10, length: 10 };
  searchValue: string = '';

  @ViewChild('searchInput') searchInput!: ElementRef;
  @ViewChild('paginator') paginator!: MatPaginator;

  @Output('onClickRow') clickEmit = new EventEmitter<ClassReadDto>();

  constructor(public _dialogService: MatDialog, private _teacherService: TeacherService) { }

  ngOnInit(): void {
    this.loadList(0, this.pageEvent.length, this.searchValue);
  }

  openDeleteClassDialog(_class: ClassReadDto) {
    const dialogRef = this._dialogService.open(ClassDeleteComponent, { data: _class });

    dialogRef.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
      if (result) {
        this.loadList(0, this.pageEvent.pageSize, this.searchValue);
      }
    });
  }

  openCreateClassDialog() {
    const dialogRef = this._dialogService.open(ClassCreateComponent,
      { disableClose: true, autoFocus: false, minWidth: '25%', height: '50%' });
    dialogRef.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
      console.log(result)
      if (result) {
        this.loadList(0, this.pageEvent.pageSize, this.searchValue);
      }
    });
  }

  loadList(page: number, limit: number, searchValue: string) {
    this.isLoading = true;
    this._teacherService.getOwnedClass(page, limit, searchValue).pipe(map(res => res.data), takeUntil(this.$unsubscribe))
      .subscribe(data => {
        this.classList = data;
        this.isLoading = false;
      });
  }

  pageHandling(event: PageEvent) {
    this.pageEvent = event;
    this.loadList(event.pageIndex, event.pageSize, this.searchValue);
  }

  searchHandling() {
    this.searchValue = this.searchInput.nativeElement.value;
    this.loadList(0, this.pageEvent.pageSize, this.searchValue);
    this.paginator.pageIndex = 0;
  }

  rowClickHandling(_class: ClassReadDto) {
    this.clickEmit.emit(_class);
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }

}
