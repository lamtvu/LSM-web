import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ContentService } from '../../../services/content.service';

@Component({
  selector: 'app-edit-content',
  templateUrl: './edit-content.component.html',
  styleUrls: ['./edit-content.component.scss']
})
export class EditContentComponent implements OnInit {


  //AddnewSection
  public addContent: boolean = false;
  public newContent?: string;

  public url: string = 'https://thang.odoo.com/web/static/src/img/placeholder.png';
  public file?: File;

  private $unsubcriber = new Subject();
  isLoading: boolean = false;
  checkcontentData: boolean = false //false: number => Create Button, true: ContentReadDto(object)=> Edit Button
  contentForm = this._formBuilder.group({
    name: ['', [
      Validators.required,
      Validators.minLength(5)
    ]],
  })

  errorHandling(controlName: string, errorName: string) {
    return this.contentForm.controls[controlName].hasError(errorName);
  }

  constructor(
    private _formBuilder: FormBuilder,
    private _contentService: ContentService,
    public dialogRef: MatDialogRef<EditContentComponent>,
    // @Inject(MAT_DIALOG_DATA) public contentData: {sectionId: number, content: ContentReadDto} //Create: sectionID, Edit: ContentReadDto
    //@Inject(MAT_DIALOG_DATA) public contentData: number | ContentReadDto //Create: sectionID, Edit: ContentReadDto
    @Inject(MAT_DIALOG_DATA) public contentData: any //Create: sectionID, Edit: ContentReadDto
  ) { }

  ngOnInit(): void {
    if (!isNaN(this.contentData)) {
      return;
    }
    this.contentForm.patchValue(this.contentData);
    this.checkcontentData = true;
  }

  onClickHandle() {
    if (!isNaN(this.contentData)) {
      this.createHandling();
      return;
    }
    this.editHandling();
  }

  createHandling() {
    this.onCreateSubmit();
  }

  editHandling() {
    this.onEditSubmit();
  }

  // File Upload
  onFileSelect(event: any) {
    if (event.target.files.length > 0) {
      this.file = event.target.files[0];
    }
  }

  // Create Submit Api
  onCreateSubmit() {
    if (!this.file) {
      return;
    }
    if (!this.contentForm.valid) {
      return;
    }
    const formData = new FormData();
    formData.append('file', this.file);
    formData.append('name', this.contentForm.controls['name'].value);
    this.isLoading = true;
    this._contentService.createContent(this.contentData, formData).pipe(takeUntil(this.$unsubcriber)).subscribe(
      res => {
        this.isLoading = false;
        this._contentService.refreshList();
        this.dialogRef.close(true);
      },
      res => {
        this.isLoading = false;
        console.log(res.error);
      }
    );
  }

  // Edit Submit Api
  onEditSubmit() {
    if (!this.contentForm.valid) {
      return;
    }
    if (this.file) {
      const formData = new FormData();
      formData.append('file', this.file);
      this._contentService.editFileContent(this.contentData.id, formData).pipe(takeUntil(this.$unsubcriber)).subscribe(
        res => {
        },
        res => {
          this.isLoading = false;
          console.log(res.error);
        }
      );
    }
    this.isLoading = true;
    this._contentService.editContent(this.contentData.id, this.contentForm.value).pipe(takeUntil(this.$unsubcriber)).subscribe(
      res => {
        this.isLoading = false;
        this._contentService.refreshList();
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
