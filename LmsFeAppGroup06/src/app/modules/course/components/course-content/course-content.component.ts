import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { SectionReadDto } from '../../../../Dto/SectionDto';
import { SectionService } from '../../services/section.service';


@Component({
  selector: 'app-course-content',
  templateUrl: './course-content.component.html',
  styleUrls: ['./course-content.component.scss']
})
export class CourseContentComponent implements OnInit {

  panelOpenState = false;

  private $unsubcriber = new Subject();
  isLoading: boolean = false;


  public sections?: SectionReadDto[];

  public sectionId!: number;
  constructor(
    private _sectionService: SectionService,
    public activatedRouteService: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.handleParams();
  }

  isShow: boolean = true;
  onShowSideNav(){
    this.isShow = !this.isShow;
  }


  handleParams(){
    this.activatedRouteService.params.pipe(takeUntil(this.$unsubcriber)).subscribe((params: Params)=>{
      // console.log(params['id']);
      this.sectionId = parseInt(params['id']);
      this.onloadSection();
    });
  }

  onloadSection() {
    this.isLoading = true;
    this._sectionService
        .getSections(this.sectionId).pipe(takeUntil(this.$unsubcriber)).subscribe(
          res => {
            this.isLoading = false;
            this.sections = res.data;
            console.log(this.sections);
          },
          res => {
            this.isLoading = false;
            console.log(res.error);
          }
        )
  }

}
