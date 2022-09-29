import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-star-review',
  templateUrl: './star-review.component.html',
  styleUrls: ['./star-review.component.scss']
})
export class StarReviewComponent implements OnInit {

  @Input() starNumber!: number;


  arrStar?: number[];

  constructor() { }

  ngOnInit(): void {
    let countStar: number = Math.trunc(this.starNumber);
    this.arrStar = Array.from({ length: countStar }, (_, index) => {
      return 1
    });
    if (countStar == 5) {
      return;
    }
    let lastStar: number = (this.starNumber - countStar);
    this.arrStar = [...this.arrStar, lastStar]
    while (this.arrStar.length < 5) {
      this.arrStar = [...this.arrStar, 0];
    }
  }



}
