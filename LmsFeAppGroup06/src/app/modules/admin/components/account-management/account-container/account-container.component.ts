import { Component, OnInit } from '@angular/core';
import { UserReadDto } from '../../../../../Dto/userDto';

@Component({
  selector: 'app-account-container',
  templateUrl: './account-container.component.html',
  styleUrls: ['./account-container.component.scss']
})
export class AccountContainerComponent implements OnInit {

  focusClass?: UserReadDto;

  constructor() { }

  ngOnInit(): void {
  }

  clickRowHandling(event: UserReadDto) {
    this.focusClass = event;
  }

}
