import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from 'src/app/shared/services/user.service';
import { UserReadDto } from '../../Dto/userDto';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  private $unsubscribe = new Subject<void>();
  currentUser?: UserReadDto;

  constructor(
    private _userService: UserService
  ) { }

  ngOnInit(): void {
    this._userService.getDetail().pipe(takeUntil(this.$unsubscribe))
    .subscribe(res => this.currentUser = res.data);
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }

}
