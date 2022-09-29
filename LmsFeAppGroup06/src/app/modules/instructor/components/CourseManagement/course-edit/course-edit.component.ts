import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params } from '@angular/router';
import { EditContentComponent } from '../edit-content/edit-content.component';
import { Observable, Subject, Subscription } from 'rxjs';
import { SectionService } from '../../../services/section.service';
import { FormBuilder, Validators } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { DeleteDialogComponent } from 'src/app/shared/components/delete-dialog/delete-dialog.component';
import { ContentService } from '../../../services/content.service';
import { ChangeSectionNameComponent } from '../change-section-name/change-section-name.component';
import { CourseService } from '../../../services/course.service';
import { SectionReadDto } from '../../../../../Dto/SectionDto';
import { ContentReadDto } from '../../../../../Dto/ContentDto';


@Component({
  selector: 'app-course-edit',
  templateUrl: './course-edit.component.html',
  styleUrls: ['./course-edit.component.scss']
})
export class CourseEditComponent implements OnInit {

  private $unsubcriber = new Subject();
  isLoading: boolean = false;
  //AddnewSection
  public addSection: boolean = false;

  panelOpenState = false;

  public sections: SectionReadDto[] = [];

  public courseId!: number;   // Store API

  sectionForm = this._formBuilder.group({
    name: ['', [Validators.minLength(5)]]
  })

  errorHandling(controlName: string, errorName: string){
    return this.sectionForm.controls[controlName].hasError(errorName);
  }

  public numberOfSection?: number;
  public numberOfContent: number = 0;

  constructor(
    public _dialogService: MatDialog,
    public activatedRouteService: ActivatedRoute,
    private _sectionService: SectionService,
    private _contentService: ContentService,
    private _courseService: CourseService,
    private _formBuilder: FormBuilder,
    ) { }

  // public courseDescription!: string;
  ngOnInit(): void {
    this.handleParams();
    this.getCourseName();
    this.onloadSection();
    this._sectionService.editEmit.pipe(takeUntil(this.$unsubcriber)).subscribe(res =>{
      this.onloadSection();
    });
  }

  handleParams(){
    this.activatedRouteService.params.pipe(takeUntil(this.$unsubcriber)).subscribe((params: Params)=>{
      // console.log(params['id']);
      this.courseId = parseInt(params['id']);
    });
  }

  get newSection(): any { return this.sectionForm.get('name'); }
  clearInput(){
    this.addSection = false;
    this.newSection.reset();
  }

  openEditContentDialog(content: ContentReadDto) {
    const dialogRef = this._dialogService.open(EditContentComponent, {data: content, width: '30%'});
    dialogRef.afterClosed().pipe(takeUntil(this.$unsubcriber)).subscribe(result => {
      if(result){
        this.onloadSection();
      }
    });
  }

  openCreateContentDialog(sectionId: number) {
    const dialogRef = this._dialogService.open(EditContentComponent, {data: sectionId, width: '30%' });

    dialogRef.afterClosed().pipe(takeUntil(this.$unsubcriber)).subscribe(result => {
      if(result){
        this.onloadSection();
      }
    });
  }


  onCreateSection(){
    if(!this.sectionForm.valid){
      return;
    }
    this.addSection = false;
    this.isLoading = true;
    this._sectionService.createSection(this.courseId,this.sectionForm.value).pipe(takeUntil(this.$unsubcriber)).subscribe(
      res => {
        this.isLoading = false;
        this.onloadSection();
        this.newSection.reset();
      },
      res => {
        this.isLoading = false;
        console.log(res.error);
      }
    )
  }

  onEditSection(section: SectionReadDto){
    if(!this.sectionForm.valid){
      return;
    }
    const dialogRef = this._dialogService.open(ChangeSectionNameComponent, {data: section, width: '30%' });
    dialogRef.afterClosed().pipe(takeUntil(this.$unsubcriber)).subscribe(result => {
      if(result){
        this.onloadSection();
      }
    });
  }

  onDeleteSection(sectionId: number, sectionName: string){
    if(!this.sectionForm.valid){
      return;
    }
    this.addSection = false;
    const deleteDialog = this._dialogService.open(DeleteDialogComponent, {data: {title: 'Delete Section', deleteName: sectionName}});
    deleteDialog.afterClosed().pipe(takeUntil(this.$unsubcriber)).subscribe(
      result => {
        if(result){
          this.isLoading = true;
          this._sectionService.deleteSection(sectionId).pipe(takeUntil(this.$unsubcriber)).subscribe(
            res =>{
              this.isLoading = false;
              this.onloadSection();
              this.newSection.reset();
            },
            res =>{
              this.isLoading = false;
              console.log(res.error);
            }
          )
        }
      }
    )
  }

  onloadSection() {
    this.isLoading = true;
    this._sectionService
        .getSections(this.courseId).pipe(takeUntil(this.$unsubcriber)).subscribe(
          res => {
            this.numberOfContent = 0;
            this.isLoading = false;
            this.sections = res.data;
            this.numberOfSection = res.data.length;
            for(let i=0; i<res.data.length;i++){
              this.numberOfContent += res.data[i].contents.length;
            }
          },
          res => {
            this.isLoading = false;
            console.log(res.error);
          }
        )
  }

  deleteContent(content: ContentReadDto){
    const deleteDialog = this._dialogService.open(DeleteDialogComponent, {data: {title: 'Delete Content', deleteName: content.name}});
    deleteDialog.afterClosed().pipe(takeUntil(this.$unsubcriber)).subscribe(
      result => {
        if(result){
          this._contentService.deleteContent(content.id).pipe(takeUntil(this.$unsubcriber)).subscribe(
            res =>{
              this.onloadSection();
            },
            res =>{
              this.isLoading = false;
              console.log(res.error);
            }
          )
        }
      }
    )
  }

  public courseName?: string;
  getCourseName(){
    if(!this.sectionForm.valid){
      return;
    }
    this.isLoading = true;
    this._courseService.getCourseById(this.courseId).pipe(takeUntil(this.$unsubcriber)).subscribe(
      res =>{
        this.isLoading = false;
        this.courseName = res.data.name;
        this.onloadSection();
      },
      res =>{
        this.isLoading = false;
        console.log(res.error);
      }
    )
  }

  ngOnDestroy(): void {
    this.$unsubcriber.next();
    this.$unsubcriber.complete();
  }
}
