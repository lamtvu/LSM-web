import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { ContentReadDto } from '../../../../Dto/ContentDto';


@Component({
  selector: 'app-course-content-dialog',
  templateUrl: './course-content-dialog.component.html',
  styleUrls: ['./course-content-dialog.component.scss']
})
export class CourseContentDialogComponent implements OnInit {

  private $unsubcriber = new Subject();
  isLoading: boolean = false;
  public content!: ContentReadDto;
  public url: string = 'https://lmstechbe.azurewebsites.net/api/Content/file/';

  constructor(
    @Inject(MAT_DIALOG_DATA) public contentData: ContentReadDto
  ) { }

  ngOnInit(): void {
    this.content = this.contentData;
    this.url = this.url + this.content.id;
  }
}
