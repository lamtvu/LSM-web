import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CourseReadDto } from '../../../Dto/CourseDto';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class ClassCourseService {

  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/course'

  constructor(
    private _httpClient: HttpClient
  ) { }

  getCourses(classId: number, pageIndex: number, pageSize: number, searchValue: string): Observable<ResponseDto<PageDataDto<CourseReadDto[]>>> {
    return this._httpClient.get<ResponseDto<PageDataDto<CourseReadDto[]>>>(this.baseUrl + '/get-by-class/' + classId, {
      params: {
        searchValue: searchValue,
        page: pageIndex,
        limit: pageSize
      }
    })
  }

  getAllCourse(pageIndex: number, pageSize: number, searchValue: string): Observable<ResponseDto<PageDataDto<CourseReadDto[]>>> {
    return this._httpClient.get<ResponseDto<PageDataDto<CourseReadDto[]>>>(this.baseUrl, {
      params: {
        searchValue: searchValue,
        page: pageIndex,
        limit: pageSize
      }
    })
  }
}
