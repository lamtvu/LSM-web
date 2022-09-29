import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Datalink } from 'src/app/shared/components/side-nav/side-nav.component';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-student-container',
  templateUrl: './student-container.component.html',
  styleUrls: ['./student-container.component.scss']
})
export class StudentContainerComponent implements OnInit {
  private $unsubscribe = new Subject()
  dataLinks: Datalink[] = [
    { label: 'My Classes', url: '/student/my-class', icon: 'class' },
    { label: 'Information', url: '/student/information/profile', icon: 'person' },
  ]

  constructor(private _userService: UserService) { }

  ngOnInit(): void {
    this._userService.getDetail().pipe(takeUntil(this.$unsubscribe))
      .subscribe(res => {
        this._userService.setUser(res.data);
      });
  }

  isShow: boolean = true;
  onShowSideNav() {
    this.isShow = !this.isShow;
  }

}
