import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ReportReadDTO } from '../../../../Dto/report-read-dto';
import { ClassService } from '../../services/class.service';
import { ReportService } from '../../services/report.service';
import { TeacherReportDetailComponent } from '../teacher-report-detail/teacher-report-detail.component';

@Component({
  selector: 'app-teacher-report-list',
  templateUrl: './teacher-report-list.component.html',
  styleUrls: ['./teacher-report-list.component.scss']
})
export class TeacherReportListComponent implements OnInit {
  
  private $unsubscriber = new Subject();
  reports?: ReportReadDTO[];
  isLoad: boolean = false;

  constructor(
    public dialog: MatDialog,
    private _classService: ClassService,
    private _reportService: ReportService) { }

  ngOnInit(): void {
    this.loadReport();
  }

  
  openDialog(content: string){
    const dialogRef = this.dialog.open(TeacherReportDetailComponent,{
      height: '',
      width: '100%',
      data:{
        message : content
      }
    });

    dialogRef.afterClosed().subscribe(result => {
       console.log(`Dialog result: ${result}`);
    });
  }

  loadReport() { 
    if(!this._classService.classId) return;
    this.isLoad = true
    this._reportService.getReports(this._classService.classId)
    .pipe(takeUntil(this.$unsubscriber)).subscribe(
      res => {
        this.isLoad=false;
        this.reports= res.data;
      })
                 
  }
 

  ngOnDestroy(): void {
    this.$unsubscriber.next();
    this.$unsubscriber.complete();
  }


}
