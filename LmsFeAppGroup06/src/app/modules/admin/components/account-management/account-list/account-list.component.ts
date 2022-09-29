import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, pipe, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { PageDataDto } from '../../../../../Dto/pageDataDto';
import { UserReadDto } from '../../../../../Dto/userDto';
import { AdminService } from '../../../services/admin.service';
import { LockDialogComponent } from '../../lock-dialog/lock-dialog.component';

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.scss']
})
export class AccountListComponent implements OnInit {
  private $unsubscribe = new Subject();
  isLoading: boolean = false;
  isLock?:boolean;
  title: string = '';

  $accounts?: Observable<PageDataDto<UserReadDto[]>>;
  searchValue: string = '';

  @ViewChild('searchInput') searchInput!: ElementRef;
  @Output('onClickRow') clickEmit = new EventEmitter<UserReadDto>();


  constructor(
    private _matDialogService: MatDialog,
    private _adminService: AdminService,
    ) { }

  ngOnInit(): void {
    this.loadAccount(this.searchValue);
  }

  loadAccount(searchValue: string) {
    this.isLoading = true;
    this.$accounts = this._adminService
        .getAccountList(searchValue).pipe(map(res => res.data), tap(
          res => {
            this.isLoading = false;
            console.error();
        }));
  }

  searchHandling() {
    this.searchValue = this.searchInput.nativeElement.value;
    this.loadAccount(this.searchValue);
  }
  rowClickHandling(_account: UserReadDto) {
    this.clickEmit.emit(_account);
  }

  changeLock(id:number)
  {
    this._adminService.changeLock(id).pipe(takeUntil(this.$unsubscribe)).subscribe(
      res => {
        this.isLoading = true;
        this.loadAccount(this.searchValue);
      })
  }

  changeTitle(type: string){
    if(type == 'lock'){
      this.title = 'unlock';
      return;
    }
    this.title = 'lock';
  }

  openEditDialog(user: UserReadDto) {
    const editDialog = this._matDialogService.open(LockDialogComponent, {
      width: '20%',
      data: {title: this.title}
    })
    editDialog.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(
      result => {
        if (result)
          this.changeLock(user.id);
      }
    )
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }
}
