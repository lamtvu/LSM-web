
import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { ClassRequest } from 'src/app/Dto/class-request';
import { ClassReadDto } from '../../../../Dto/classDto';
import { PageDataDto } from '../../../../Dto/pageDataDto';
import { RequestService } from '../../services/request.service';
import { StudentService } from '../../services/student.service';
import { StudentRequestDialogComponent } from '../student-request-dialog/student-request-dialog.component';

@Component({
  selector: 'app-student-request',
  templateUrl: './student-request.component.html',
  styleUrls: ['./student-request.component.scss']
})
export class StudentRequestComponent implements OnInit {

  private $unsubscribe = new Subject();
  isLoading: boolean = false;
  title: string = '';

  $classes?: Observable<PageDataDto<ClassRequest[]>>;
  searchValue: string = '';

  @ViewChild('searchInput') searchInput!: ElementRef;


  constructor(
    private _matDialogService: MatDialog,
    private _requestService: RequestService,
    private _studentService: StudentService,
    ) { }

  ngOnInit(): void {
    this.loadClass(this.searchValue);
  }

  loadClass(searchValue: string) {
    this.isLoading = true;
    this.$classes = this._requestService
        .getClassToRequest(searchValue).pipe(map(res => res.data), tap(
          res => {           
            this.isLoading = false;
        }));
  }


  searchHandling() {
    this.searchValue = this.searchInput.nativeElement.value;
    this.loadClass(this.searchValue);
  }
 
  createRequest(id:number)
  {
    this._requestService.createRequest(id).pipe(takeUntil(this.$unsubscribe)).subscribe(
      res => {
        this.isLoading = true;        
        this.loadClass(this.searchValue);
      })
  }

  createDialog(model:ClassRequest) {
    const editDialog = this._matDialogService.open(StudentRequestDialogComponent, {
      width: '20%',
      data: {title: 'join'}
    })
    editDialog.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(
      result => {
        if (result)
          this.createRequest(model.classReadDto.id);
          console.log(model.classReadDto.id);
      }
    )
  }

  deleteRequest(id:number)
  {
    this._requestService.deleteRequest(id).pipe(takeUntil(this.$unsubscribe)).subscribe(
      res => {
        this.isLoading = true;        
        this.loadClass(this.searchValue);
      })
  }

  deleteDialog(model:ClassRequest) {
    const editDialog = this._matDialogService.open(StudentRequestDialogComponent, {
      width: '20%',
      data: {title: 'join'}
    })
    editDialog.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(
      result => {
        if (result)
          this.deleteRequest(model.classReadDto.id);
      }
    )
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }
}
