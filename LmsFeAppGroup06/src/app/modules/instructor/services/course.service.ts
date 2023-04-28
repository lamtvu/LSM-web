import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentReadDto } from '../../../Dto/CommentDto';
import { CourseReadDto } from '../../../Dto/CourseDto';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable(
)
export class CourseService {


  readonly baseUrl = "https://localhost:5001/api/course";
  public editEmit = new EventEmitter<boolean>();
  constructor(
    private _httpClientService: HttpClient
  ) { }

  refreshList() {
    this.editEmit.emit(true);
  }

  createCourse(createForm: FormData): Observable<ResponseDto<null>> {
    return this._httpClientService.post<ResponseDto<null>>(this.baseUrl, createForm);
  }

  getCourses(searchValue: string, page: number, limit: number): Observable<ResponseDto<PageDataDto<CourseReadDto[]>>> {
    return this._httpClientService.get<ResponseDto<PageDataDto<CourseReadDto[]>>>(this.baseUrl + "/get-owned", { params: { searchValue: searchValue, page: page, limit: limit } });
  }

  deleteCourse(courseId: number): Observable<ResponseDto<null>> {
    return this._httpClientService.delete<ResponseDto<null>>(this.baseUrl + "/" + courseId);
  }

  editCourse(courseId: number, editForm: FormData): Observable<ResponseDto<null>> {
    return this._httpClientService.put<ResponseDto<null>>(this.baseUrl + "/" + courseId, editForm);
  }

  getCourseById(courseId: number): Observable<ResponseDto<CourseReadDto>> {
    return this._httpClientService.get<ResponseDto<CourseReadDto>>(this.baseUrl + "/" + courseId);
  }
  
  getCommentReviewAll(courseId: number, pageIndex: number, pageSize: number): Observable<ResponseDto<PageDataDto<CommentReadDto[]>>> {
    return this._httpClientService.get<ResponseDto<PageDataDto<CommentReadDto[]>>>("https://localhost:5001/api/review/get-by-page/" + courseId, { params: { page: pageIndex, limit: pageSize } });
  }


}
