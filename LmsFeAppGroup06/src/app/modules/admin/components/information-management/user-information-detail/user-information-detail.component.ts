import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserReadDto } from '../../../../../Dto/userDto';
import { UserService } from 'src/app/shared/services/user.service';
import { AdminService } from '../../../services/admin.service';
import { ChangeInformationComponent } from '../change-information/change-information.component';

@Component({
  selector: 'app-user-information-detail',
  templateUrl: './user-information-detail.component.html',
  styleUrls: ['./user-information-detail.component.scss']
})
export class UserInformationDetailComponent implements OnInit {

  private $unsubscribe = new Subject<void>();
  public currentUser?: UserReadDto;
  public userRole: string = '';
  isLoading: boolean = false;
  constructor(
    private _userService: UserService,
    private _adminService: AdminService,
    private _matDialogService: MatDialog
  ) { }

  ngOnInit(): void {
    this.isLoading = true;
    this._userService.getDetail().pipe(takeUntil(this.$unsubscribe))
    .subscribe(res => {
      this.isLoading = false;
      this.currentUser = res.data;
      this.checkRole(this.currentUser.roleId);
    });
    this._adminService.editEmit.pipe(takeUntil(this.$unsubscribe)).subscribe(res =>{
      this.isLoading = true;
      this._userService.getDetail().pipe(takeUntil(this.$unsubscribe))
      .subscribe(res => {
        this.isLoading = false;
        this.currentUser = res.data;
        this.checkRole(this.currentUser.roleId);
      });
    })
  }

  checkRole(roleId: number){
    if(roleId == 1){
      this.userRole = 'Admin';
    }
    else if (roleId == 2){
      this.userRole = 'Teacher';
    }
    else if (roleId == 3){
      this.userRole = 'Instructor';
    }
    else this.userRole = 'Student';
  }

  onchangeInformationDialog(){
      const editDialog = this._matDialogService.open(ChangeInformationComponent, {data: this.currentUser, width: '15%'});
      editDialog.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(res => {
      },
      res => {

      });
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }

}
