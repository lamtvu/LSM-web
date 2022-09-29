import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { SectionReadDto } from '../../../../../Dto/SectionDto';
import { SectionService } from '../../../services/section.service';

@Component({
  selector: 'app-change-section-name',
  templateUrl: './change-section-name.component.html',
  styleUrls: ['./change-section-name.component.scss']
})
export class ChangeSectionNameComponent implements OnInit {

  private $unsubcriber = new Subject();
  isLoading: boolean = false;

  constructor(
    private _formBuilder: FormBuilder,
    private _sectionService: SectionService,
    public dialogRef: MatDialogRef<ChangeSectionNameComponent>,
    @Inject(MAT_DIALOG_DATA) public sectionData: SectionReadDto
  ) { }

  ngOnInit(): void {
    this.sectionForm.patchValue(this.sectionData);
  }

  sectionForm = this._formBuilder.group({
    name: ['',[
      Validators.required,
      Validators.minLength(5)
    ]],
  })

  errorHandling(controlName: string, errorName: string){
    return this.sectionForm.controls[controlName].hasError(errorName);
  }

  onEditSection(){
    if(!this.sectionForm.valid){
      return;
    }
    this.isLoading = true;
    this._sectionService.editSection(this.sectionData.id, this.sectionForm.value).pipe(takeUntil(this.$unsubcriber)).subscribe(
      res =>{
        this.isLoading = false;
        this._sectionService.refreshList();
        this.dialogRef.close(true);
      },
      res => {
        this.isLoading = false;
        console.log(res.error);
      }
    );
  }

  ngOnDestroy(): void {
    this.$unsubcriber.next();
    this.$unsubcriber.complete();
  }
}
