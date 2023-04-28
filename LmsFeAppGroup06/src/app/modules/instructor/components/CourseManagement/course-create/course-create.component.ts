import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CourseService } from '../../../services/course.service';
import { CourseReadDto } from '../../../../../Dto/CourseDto';

@Component({
  selector: 'app-course-create',
  templateUrl: './course-create.component.html',
  styleUrls: ['./course-create.component.scss']
})
export class CourseCreateComponent implements OnInit {

  //Upload file
  public isUpload: boolean = false;
  public url: string = 'https://thang.odoo.com/web/static/src/img/placeholder.png';
  public file?: File;
  private $unsubcriber = new Subject();
  isLoading: boolean = false;
  courseForm = this._formBuilder.group({
    name: ['', [
      Validators.required,
      Validators.minLength(5)
    ]],
    level: [0],
    isPublic: [true],
    description: ['']
  })

  errorHandling(controlName: string, errorName: string) {
    return this.courseForm.controls[controlName].hasError(errorName);
  }

  constructor(
    private _formBuilder: FormBuilder,
    private _courseService: CourseService,
    public dialogRef: MatDialogRef<CourseCreateComponent>,
    @Inject(MAT_DIALOG_DATA) public course: CourseReadDto
  ) {

  }

  ngOnInit(): void {
    if (!this.course) {
      return;
    }
    this.courseForm.patchValue(this.course);
    this.url = 'https://localhost:5001/api/course/image/' + this.course.id;
  }

  // Get image String
  onSelectFile(event: any) { // called each time file input changes
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event: any) => { // called once readAsDataURL is completed
        this.url = event.target.result;
      }
      this.file = event.target.files[0];
    }
  }


  //Create Handling
  createHandling() {
    if (!this.courseForm.valid || !this.file) {
      return;
    }
    this.isLoading = true;
    const formData = new FormData();
    formData.append('file', this.file);
    formData.append('name', this.courseForm.controls['name'].value);
    formData.append('level', this.courseForm.controls['level'].value);
    formData.append('isPublic', this.courseForm.controls['isPublic'].value);
    formData.append('description', this.courseForm.controls['description'].value);
    this._courseService.createCourse(formData).pipe(takeUntil(this.$unsubcriber)).subscribe(
      res => {
        this.isLoading = false;
        this.dialogRef.close(true);
      },
      res => {
        this.isLoading = false;
        console.log(res.error);
      }
    )
  }

  editHandling() {
    if (!this.courseForm.valid) {
      return;
    }
    this.isLoading = true;
    const formData = new FormData();
    if (this.file)
      formData.append('file', this.file);
    formData.append('name', this.courseForm.controls['name'].value);
    formData.append('level', this.courseForm.controls['level'].value);
    formData.append('isPublic', this.courseForm.controls['isPublic'].value);
    formData.append('description', this.courseForm.controls['description'].value);
    this._courseService.editCourse(this.course.id, formData).pipe(takeUntil(this.$unsubcriber)).subscribe(
      res => {
        this.isLoading = false;
        this._courseService.refreshList();
        this.dialogRef.close(this.courseForm.value);
      },
      res => {
        this.isLoading = false;
        console.log(res.error);
      }
    )
  }

  //
  onClickHandle() {
    if (!this.course) {
      this.createHandling();
      return;
    }
    this.editHandling();
  }

  ngOnDestroy(): void {
    this.$unsubcriber.next();
    this.$unsubcriber.complete();
  }
}
