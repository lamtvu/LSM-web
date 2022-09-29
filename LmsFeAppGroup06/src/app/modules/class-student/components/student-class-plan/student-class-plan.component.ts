import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { AnnouncenmentReadDto } from '../../../../Dto/AnnouncenmentDto';
import { ClassService } from '../../services/class.service';
import { PlanService } from '../../services/plan.service';

@Component({
  selector: 'app-student-class-plan',
  templateUrl: './student-class-plan.component.html',
  styleUrls: ['./student-class-plan.component.scss']
})
export class StudentClassPlanComponent implements OnInit {
  isLoading: boolean = false;
  $plans?: Observable<AnnouncenmentReadDto[]>;
  constructor(
    private _planService: PlanService,
    private _classService: ClassService
  ) { }

  ngOnInit(): void {
    this.loadPlan();
  }

  loadPlan() {
    if(!this._classService.classId) return;
    this.isLoading = true
    this.$plans = this._planService.getPlans(this._classService.classId).pipe(map(res=>res.data),
    tap(data=> this.isLoading = false));
  }
}
