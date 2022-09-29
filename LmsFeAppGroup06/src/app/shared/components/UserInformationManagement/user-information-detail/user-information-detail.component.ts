import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from 'src/app/shared/services/user.service';
import { UserReadDto } from '../../../../Dto/userDto';
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

  messageSendMail: string = '';
  messageChangeRole: string = '';
  urlAvatar?: string;
  isLoading: boolean = false;
  isLoadingAvatar: boolean = false;
  isSendingMail: boolean = false;
  isChangeRole: boolean = false;

  file?: File;

  constructor(
    private _userService: UserService,
    private _matDialogService: MatDialog,
  ) { }

  ngOnInit(): void {
    this.isLoading = true;
    this._userService.getDetail().pipe(takeUntil(this.$unsubscribe))
      .subscribe(res => {
        this.isLoading = false;
        this.currentUser = res.data;
        this.checkRole(this.currentUser.roleId);
        this.urlAvatar = 'https://lmstechbe.azurewebsites.net/api/user/avatar/' + this.currentUser?.id;
      });
    this._userService.editEmit.pipe(takeUntil(this.$unsubscribe)).subscribe(res => {
      this.isLoading = true;
      this._userService.getDetail().pipe(takeUntil(this.$unsubscribe))
        .subscribe(res => {
          this.isLoading = false;
          this.currentUser = res.data;
          this.checkRole(this.currentUser.roleId);
        });
    })
  }

  checkRole(roleId: number) {
    if (roleId == 1) {
      this.userRole = 'admin';
    }
    else if (roleId == 2) {
      this.userRole = 'teacher';
    }
    else if (roleId == 3) {
      this.userRole = 'instructor';
    }
    else this.userRole = 'student';
  }

  getVerify() {
    if (!this.currentUser) return;
    this.isSendingMail = true;
    this._userService.getVerify(this.currentUser?.email).pipe(takeUntil(this.$unsubscribe)).subscribe(
      res => {
        this.messageSendMail = 'verification has been sent to the mail';
        this.isSendingMail = false;
      }, res => {
        this.isSendingMail = false;
        if (res.error)
          this.messageSendMail = res.error.message;
      }
    )
  }


  onchangeInformationDialog() {
    const editDialog = this._matDialogService.open(ChangeInformationComponent, { data: this.currentUser, width: '18%' });
    editDialog.afterClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(res => {
    },
      res => {
      });
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }

  changeFile(event: any) {
    if (event.target.files.length === 0) return;
    this.file = event.target.files[0];
    var reander = new FileReader();
    reander.readAsDataURL(event.target.files[0]);
    reander.onload = (event: any) => {
      this.urlAvatar = event.target.result;
    }
  }

  changeAvatar() {
    if (!this.file) return;
    const formData = new FormData();
    formData.append('file', this.file);
    this.isLoadingAvatar = true;
    this._userService.changeAvatar(formData).pipe(
      takeUntil(this.$unsubscribe)).subscribe(
        res => {
          this.file = undefined;
          this.isLoadingAvatar = false;
        }, res => {
          this.isLoadingAvatar = false;
        }
      )
  }

  changeRole(roleId: number) {
    if (!this.currentUser) return;
    this.isLoading = true;
    this._userService.changeRole(this.currentUser.id, roleId).pipe(takeUntil(this.$unsubscribe))
      .subscribe(
        res => {
          this.isLoading = false;
          this.isChangeRole = false;
          localStorage.clear();
          this.messageChangeRole = 'Success, Please log in again';
        }, res => {
          this.isLoading = false;
          if (res.error) {
            this.messageChangeRole = res.error.messager
          }
        }
      )
  }

}
