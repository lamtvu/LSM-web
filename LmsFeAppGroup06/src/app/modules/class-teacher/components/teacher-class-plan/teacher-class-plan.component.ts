import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import { filter, map, take, takeUntil, tap } from 'rxjs/operators';
import { ClassService } from '../../services/class.service';
import { PlanService } from '../../services/plan.service';
import { CreatePlanDialogComponent } from '../create-plan-dialog/create-plan-dialog.component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { AnnouncenmentReadDto } from '../../../../Dto/AnnouncenmentDto';

@Component({
  selector: 'app-teacher-class-plan',
  templateUrl: './teacher-class-plan.component.html',
  styleUrls: ['./teacher-class-plan.component.scss']
})
export class TeacherClassPlanComponent implements OnInit {
  private $unsubscribe = new Subject();
  private classId?: number;
  isLoading: boolean = false;

  $plans?: Observable<AnnouncenmentReadDto[]>;

  constructor(
    private _planService: PlanService,
    private _classService: ClassService,
    private _matDialogService: MatDialog
  ) { }

  ngOnInit(): void {
    this.classId = this._classService.classId;
    if (this.classId)
      this.loadPlan();
    this._classService.classIdEmit.pipe(takeUntil(this.$unsubscribe)).subscribe(id => {
      this.classId = id;
      this.loadPlan();
    })
  }

  loadPlan() {
    if (this.classId){
      this.isLoading = true;
      this.$plans = this._planService.getPlans(this.classId)
        .pipe(takeUntil(this.$unsubscribe), map(res => res.data),
        tap(res => this.isLoading = false, res => this.isLoading = false));
    }
  }

  openCreateDialog() {
    console.log(this.classId)
    const createPlanDialog = this._matDialogService.open(CreatePlanDialogComponent,
      {
        minWidth: '30%',
        minHeight: '50%',
        disableClose: true,
      });
    createPlanDialog.beforeClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
      if (result)
        this.loadPlan();
    });
  }

  openEditDialog(plan: AnnouncenmentReadDto) {
    const editPlanDialog = this._matDialogService.open(CreatePlanDialogComponent,
      {
        minWidth: '30%',
        disableClose: true,
        data: { ...plan }
      })
    editPlanDialog.beforeClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
      if (result)
        this.loadPlan();
    });
  }

  openDeleteDialog(plan: AnnouncenmentReadDto) {
    const deleteDialog = this._matDialogService.open(DeleteDialogComponent,
      {
        disableClose: false,
        data: { title: 'Delete Plan', deleteName: plan.title }
      })
      deleteDialog.beforeClosed().pipe(takeUntil(this.$unsubscribe)).subscribe(result => {
        if (result) {
          this.isLoading = true
          this._planService.deletePlan(plan.id!).pipe(takeUntil(this.$unsubscribe)).subscribe(
            res => {
              this.loadPlan();
            },res=>{
              this.isLoading = false;
            }
          )
        }
      })
  }

  ngOnDestroy(): void {
    console.log('destroy');
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }

}
