import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserReadDto } from '../../../Dto/userDto';
import { Datalink } from 'src/app/shared/components/side-nav/side-nav.component';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-admin-container',
  templateUrl: './admin-container.component.html',
  styleUrls: ['./admin-container.component.scss']
})
export class AdminContainerComponent implements OnInit {

  private $unsubscribe = new Subject<void>();
  currentUser?: UserReadDto;

  dataLinks: Datalink[] = [
    { label: 'Manage Account', icon: 'book', url: '/admin/account-management/account-list' },
    { label: 'Manage Course', icon: 'dashboard', url: '/admin/course-management/course-list' },
    { label: 'Personal Information', icon: 'person', url: '/admin/information' }
  ]

  constructor(
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    this._userService.getDetail().pipe(takeUntil(this.$unsubscribe))
    .subscribe(res => this.currentUser = res.data);
  }

  isShow: boolean = true;
  onShowSideNav(){
    this.isShow = !this.isShow;
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }

}
