import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClassReadDto } from '../../../Dto/classDto';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class TeacherService {
  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/class';

  currentClass?: ClassReadDto;

  constructor(private _httpClient: HttpClient) { }


  getOwnedClass(page: number, limit: number, searchValue: string): Observable<ResponseDto<PageDataDto<ClassReadDto[]>>> {
    return this._httpClient.get<ResponseDto<PageDataDto<ClassReadDto[]>>>(this.baseUrl + '/owned', { params: { page: page, limit: limit, searchValue: searchValue } })
  }

  createClass(_class: { name: string, description: string }): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl, _class);
  }

  deleteClass(id: number): Observable<ResponseDto<null>> {
    return this._httpClient.delete<ResponseDto<null>>(this.baseUrl + '/' + id);
  }

  changeClass(id: number, _class: { name: string, description: string }): Observable<ResponseDto<null>> {
    return this._httpClient.put<ResponseDto<null>>(this.baseUrl + '/' + id, _class);
  }

  getClass(id: number): Observable<ResponseDto<ClassReadDto>> {
    return this._httpClient.get<ResponseDto<ClassReadDto>>(this.baseUrl + '/' + id);
  }
}
