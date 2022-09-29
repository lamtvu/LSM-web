import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Datalink } from 'src/app/shared/components/side-nav/side-nav.component';
import { UserService } from 'src/app/shared/services/user.service';
import { UserReadDto } from '../../../../Dto/userDto';

@Component({
  selector: 'app-intrustor-container',
  templateUrl: './intrustor-container.component.html',
  styleUrls: ['./intrustor-container.component.scss']
})
export class IntrustorContainerComponent implements OnInit {

  private $unsubscribe = new Subject<void>();
  currentUser?: UserReadDto;
  dataLinks: Datalink[] = [
    { label: 'Manage Course', url: '/instructor/course-management', icon: 'book' },
    { label: 'Analysis Course', url: '/instructor/course-analysis', icon: 'dashboard' },
    { label: 'Personal Information', url: '/instructor/information/profile', icon: 'person' }
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
