import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.scss']
})
export class PieChartComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  chartOptions = {
    responsive: true,
  };
  chartLabels = [['Finish 50% Course'], ['Finish Course']];
  chartData = [300, 500];
  chartColors = [{
    backgroundColor: ['rgb(255,161,181)', 'rgb(134,199,243)'],
    borderColor: ['transparent', 'transparent']
  }];
  chartLegend = true;
  chartPlugins = [];

}
