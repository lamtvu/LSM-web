import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentReadDto } from '../../../Dto/CommentDto';
import { CourseCreateDto, CourseReadDto } from '../../../Dto/CourseDto';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';
import { UserReadDto } from '../../../Dto/userDto';

@Injectable(
)
export class CourseService {


  readonly baseUrl = "https://localhost:5001/api/course";
  courseId?: number;
  course?: CourseReadDto;
  courseEmit = new EventEmitter<CourseReadDto>();

  setCourse(course: CourseReadDto) {
    this.course = course;
    this.courseEmit.emit(course);
  }

  public editEmit = new EventEmitter<boolean>();
  constructor(
    private _httpClientService: HttpClient
  ) { }

  refreshList() {
    this.editEmit.emit(true);
  }

  createCourse(createForm: CourseCreateDto): Observable<ResponseDto<null>> {
    return this._httpClientService.post<ResponseDto<null>>(this.baseUrl, createForm);
  }

  getCourses(searchValue: string, page: number, limit: number): Observable<ResponseDto<PageDataDto<CourseReadDto[]>>> {
    return this._httpClientService.get<ResponseDto<PageDataDto<CourseReadDto[]>>>(this.baseUrl + "/get-owned", { params: { searchValue: searchValue, page: page, limit: limit } });
  }

  deleteCourse(courseId: number): Observable<ResponseDto<null>> {
    return this._httpClientService.delete<ResponseDto<null>>(this.baseUrl + "/" + courseId);
  }

  editCourse(courseId: number, editForm: CourseCreateDto): Observable<ResponseDto<null>> {
    return this._httpClientService.put<ResponseDto<null>>(this.baseUrl + "/" + courseId, editForm);
  }

  getCourseById(courseId: number): Observable<ResponseDto<CourseReadDto>> {
    return this._httpClientService.get<ResponseDto<CourseReadDto>>(this.baseUrl + "/" + courseId);
  }

  createcourseReview(courseId: number, createReviewForm: { star: number, comment: string }): Observable<ResponseDto<null>> {
    return this._httpClientService.post<ResponseDto<null>>('https://localhost:5001/api/review', createReviewForm, { params: { courseId: courseId } });
  }

  getcoursesReview(searchValue: string, courseId: number, page: number, limit: number): Observable<ResponseDto<CommentReadDto[]>> {
    return this._httpClientService.get<ResponseDto<CommentReadDto[]>>("https://localhost:5001/api/review/get-by-page", { params: { searchValue: searchValue, courseId: courseId, page: page, limit: limit } });
  }
  getcoursesReviewAll(courseId: number): Observable<ResponseDto<CommentReadDto[]>> {
    return this._httpClientService.get<ResponseDto<CommentReadDto[]>>("https://localhost:5001/api/review/get-all", { params: { courseId: courseId } });
  }

  getUserById(userId: number): Observable<ResponseDto<UserReadDto>> {
    return this._httpClientService.get<ResponseDto<UserReadDto>>('https://localhost:5001/api/user/' + userId);
  }

}
