import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.scss']
})
export class BarChartComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  chartOptions = {
    responsive: true,
  };
  chartLabels = ['January', 'February', 'March', 'April', 'May', 'June', 'July'];
  chartLegend = true;
  chartPlugins = [];

  chartData = [
    { data: [65, 59, 80, 81, 56, 55, 40], label: 'Enroll' },
    { data: [28, 48, 40, 19, 86, 27, 90], label: 'Cacel' }
  ];


}
