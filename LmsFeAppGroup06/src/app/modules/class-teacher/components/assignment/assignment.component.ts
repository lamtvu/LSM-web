import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ClassService } from '../../services/class.service';

@Component({
  selector: 'app-assignment',
  templateUrl: './assignment.component.html',
  styleUrls: ['./assignment.component.scss']
})
export class AssignmentComponent implements OnInit {
  private $unsubscriber = new Subject();

  stateName: 'IDE' | 'LOADING' | 'ERROR' = 'IDE';
  dialogState = { state: this.stateName, message: '' };

  @ViewChild('usernameInput') usernameInput!: ElementRef;

  constructor(
    private _classService: ClassService,
    private _dialogRef: MatDialogRef<AssignmentComponent>,
    @Inject(MAT_DIALOG_DATA) public isClassAdmin: boolean
  ) { }

  ngOnInit(): void {
  }

  onChoose() {
    if (!this.isClassAdmin) {
      this.chooseAssistantHandling();
      return;
    }
    this.chooseClassAdminHandling();
  }

  chooseAssistantHandling() {
    this.dialogState.state = 'LOADING';
    const usernameValue = this.usernameInput.nativeElement.value;
    this._classService.chooseAssistant({username: usernameValue}).pipe(
      takeUntil(this.$unsubscriber)
    ).subscribe(res => {
      this._dialogRef.close(true);
      this.dialogState.state = 'IDE';
      this._classService.setClass(res.data);
      console.log(res.data)
    }, res => {
      this.dialogState.state = 'ERROR';
      this.dialogState.message = res.error.messager;
    })
  }

  chooseClassAdminHandling() {
    this.dialogState.state = 'LOADING';
    const usernameValue = this.usernameInput.nativeElement.value;
    this._classService.chooseClassAdmin({username: usernameValue}).pipe(
      takeUntil(this.$unsubscriber)
    ).subscribe(res => {
      this._dialogRef.close(true);
      this.dialogState.state = 'IDE';
      this._classService.setClass(res.data);
    }, res => {
      this.dialogState.state = 'ERROR';
      this.dialogState.message = res.error.messager;
    })
  }
}
