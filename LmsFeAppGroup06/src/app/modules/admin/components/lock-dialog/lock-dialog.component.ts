import {Component, Inject} from '@angular/core';
import {MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'lock-dialog',
  templateUrl: './lock-dialog.component.html',
  styleUrls: ['./lock-dialog.component.scss']
})
export class LockDialogComponent {
  public title!: string;
  constructor(
    private _adminService: AdminService,
    private _dialogRef: MatDialogRef<LockDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {title: string}) { }

  ngOnInit(){
    this.title =this.data.title;
  }

  clickHandling(): void {
    this._dialogRef.close(true);
  }
}
