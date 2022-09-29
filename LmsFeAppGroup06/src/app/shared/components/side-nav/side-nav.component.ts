import { Component, Input, OnInit } from '@angular/core';
import {
  trigger,
  state,
  style,
  animate,
  transition,
} from '@angular/animations';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss'],
  animations: [
    trigger('showHidden', [
      state('show', style({
        width: 150
      })),
      state('hidden', style({
        width: 0
      })),
      transition('show => hidden', animate('0.25s')),
      transition('hidden => show', animate('0.25s'))
    ])
  ]
})
export class SideNavComponent implements OnInit {

  @Input() dataLinks: Datalink[] = [];

  isShow: boolean = true;

  constructor() { }

  ngOnInit(): void {
  }

  get stateName() {
    return this.isShow ? 'show' : 'hidden';
  }

  onToggle() {
    this.isShow = !this.isShow;
  }
}

export interface Datalink {
  url: string,
  label: string,
  icon: string
}