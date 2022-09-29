import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { AnnouncenmentReadDto } from '../../../Dto/AnnouncenmentDto';
import { UserReadDto } from '../../../Dto/userDto';
import { UserService } from '../../services/user.service';
import { NotifyDetailComponent } from '../notify-detail/notify-detail.component';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  private $unsubscriber = new Subject();
  isShow: boolean = true; // control sivenav

  notifications?: AnnouncenmentReadDto[]
  isLoadAnnouncement: boolean = false;

  @Input() currentUser?: UserReadDto;

  @Output('isShowSideNav') onHandleSideNav = new EventEmitter<boolean>();

  constructor(
    private _routerService: Router,
    private _userService: UserService,
    private dialog: MatDialog
    ) { }

  ngOnInit(): void {
    console.log(this.notifications);
    this._userService.userEmmit.pipe(takeUntil(this.$unsubscriber)).subscribe(
      user=>{
        // console.log(this.currentUser)
        this.currentUser = user;
      }
    )
  }

  onSideNavClick() {
    this.isShow = !this.isShow;
    this.onHandleSideNav.emit(this.isShow);
  }

  logoutHandling() {
    localStorage.clear();
    let currentUrl = this._routerService.url;
    if(currentUrl == '/home'){
      window.location.reload();
    }
    else{
      this._routerService.navigate(['/home']);
    };
  }

  loadNotification() {
    this.isLoadAnnouncement = true
    this._userService.getAnnouncement()
    .pipe(takeUntil(this.$unsubscriber)).subscribe(
      res => {
        this.isLoadAnnouncement=false;
        this.notifications= res.data;
        console.log(this.notifications);
      })

  }

 clickHandling() {
    this.loadNotification();
  }

  openDialog(content: string){
    const dialogRef = this.dialog.open(NotifyDetailComponent,{
      height: '',
      width: '100%',
      data:{
        message : content
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      //  console.log(`Dialog result: ${result}`);
    });
  }


  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }

}
