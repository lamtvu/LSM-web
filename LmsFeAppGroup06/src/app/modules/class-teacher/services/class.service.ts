import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClassReadDto } from '../../../Dto/classDto';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';
import { UserReadDto } from '../../../Dto/userDto';

@Injectable()
export class ClassService {
  public classIdEmit = new EventEmitter<number>();
  public classEmit = new EventEmitter<ClassReadDto>();
  public classId?: number;
  public class?: ClassReadDto;
  readonly baseUrl = 'https://localhost:5001/api/class';

  constructor(
    private _httpClient: HttpClient
  ) { }

  setClassId(classId: number) {
    this.classId = classId;
    this.classIdEmit.emit(classId);
  }
  setClass(_class: ClassReadDto) {
    this.class = _class;
    this.classEmit.emit(this.class);
  }

  getStudents(pageIndex: number, limit: number): Observable<ResponseDto<PageDataDto<UserReadDto[]>>> {
    return this._httpClient
      .get<ResponseDto<PageDataDto<UserReadDto[]>>>(this.baseUrl + '/students/' + this.classId,
        { params: { page: pageIndex, limit: limit } });
  }

  deleteStudent(student: { username: string }): Observable<ResponseDto<null>> {
    return this._httpClient.put<ResponseDto<null>>(this.baseUrl + '/students/' + this.classId, student);
  }

  addStudent(classId: number, student: { username: string }): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + '/students/' + classId, student);
  }

  chooseClassAdmin(student: { username: string }): Observable<ResponseDto<ClassReadDto>> {
    return this._httpClient.post<ResponseDto<ClassReadDto>>(this.baseUrl + '/class-admin/' + this.classId, student);
  }

  chooseAssistant(student: { username: string }): Observable<ResponseDto<ClassReadDto>> {
    return this._httpClient.post<ResponseDto<ClassReadDto>>(this.baseUrl + '/assistant/' + this.classId, student);
  }

  inviteStudent(student: { username: string }): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + '/students/invite/' + this.classId, student);
  }

  addCourseToclass(courseId: number): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + '/courses/' + this.classId, null
      , { params: { courseId: courseId } });
  }

  removeCourseInClass(courseId: number): Observable<ResponseDto<null>> {
    return this._httpClient.delete<ResponseDto<null>>(this.baseUrl + '/courses/' + this.classId, { params: { courseId: courseId } });
  }

}
