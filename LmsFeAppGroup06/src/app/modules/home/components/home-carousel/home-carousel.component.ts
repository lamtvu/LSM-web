import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-home-carousel',
  templateUrl: './home-carousel.component.html',
  styleUrls: ['./home-carousel.component.scss']
})
export class HomeCarouselComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  slides = [
    {'image': 'https://www.iofc.org/sites/default/files/media/image/ne/neonbrand-zfso6bnzjtw-unsplash.jpg'},
    {'image': 'https://archive.org/download/dylan-gillis-KdeqA3aTnBY-unsplash/dylan-gillis-KdeqA3aTnBY-unsplash.jpg'},
    {'image': 'https://www.evidenceinvestor.com/wp-content/uploads/2017/06/the-climate-reality-project-Hb6uWq0i4MI-unsplash.jpg'},
  ];
}
