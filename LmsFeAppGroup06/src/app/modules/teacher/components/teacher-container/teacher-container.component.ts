import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Datalink } from 'src/app/shared/components/side-nav/side-nav.component';
import { UserService } from 'src/app/shared/services/user.service';
import { UserReadDto } from '../../../../Dto/userDto';

@Component({
  selector: 'app-teacher-container',
  templateUrl: './teacher-container.component.html',
  styleUrls: ['./teacher-container.component.scss']
})
export class TeacherContainerComponent implements OnInit {
  private $unsubscribe = new Subject<void>();
  currentUser?: UserReadDto;
  dataLinks: Datalink[] = [
    { label: 'Class Management', url: '/teacher/class-management', icon: 'class' },
    { label: 'Information', url: '/teacher/information/profile', icon: 'person' }
  ]
  constructor(private _userService: UserService) { }

  ngOnInit(): void {
    this._userService.getDetail().pipe(takeUntil(this.$unsubscribe))
      .subscribe(res => this.currentUser = res.data);
  }

  isShow: boolean = true;
  onShowSideNav() {
    this.isShow = !this.isShow;
  }

}
