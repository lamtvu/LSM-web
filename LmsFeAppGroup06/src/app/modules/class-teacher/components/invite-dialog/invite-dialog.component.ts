import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ClassService } from '../../services/class.service';

@Component({
  selector: 'app-invite-dialog',
  templateUrl: './invite-dialog.component.html',
  styleUrls: ['./invite-dialog.component.scss']
})
export class InviteDialogComponent implements OnInit {
  private $unsubscriber = new Subject();
  stateName: 'IDE' | 'LOADING' | 'ERROR' = 'IDE';
  dialogState = { state: this.stateName, message: "" };

  @ViewChild('usernameInput') usernameInput!: ElementRef

  constructor(
    private _classService: ClassService,
    private _dialogRef: MatDialogRef<InviteDialogComponent>
  ) { }

  ngOnInit(): void {

  }

  inviteHandling() {
    const username = this.usernameInput.nativeElement.value;
    if (!username) {
      this.dialogState.state = 'ERROR';
      this.dialogState.message = 'username is required';
      return
    }
    this.dialogState.state = 'LOADING';
    this._classService.inviteStudent({username: username}).pipe(takeUntil(this.$unsubscriber))
      .subscribe(res => {
        this.dialogState.state = 'IDE'
        this._dialogRef.close(true);
      }, res => {
        this.dialogState.state = 'ERROR';
        this.dialogState.message = res.error.messager;
      })
  }


}
