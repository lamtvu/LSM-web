import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { RequestStudentReadDto } from '../../../Dto/requestDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class RequestStudentService {
  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/request-student'

  constructor(
    private _httpCient: HttpClient,
  ) { }

  getRequests(classId: number, pageIndex: number, pageSize: number): Observable<ResponseDto<PageDataDto<RequestStudentReadDto[]>>> {
    return this._httpCient.get<ResponseDto<PageDataDto<RequestStudentReadDto[]>>>(this.baseUrl + '/get-by-page/' + classId, { params: { page: pageIndex, limit: pageSize } });
  }

  deleteAllRequest(classId: number): Observable<ResponseDto<null>> {
    return this._httpCient.delete<ResponseDto<null>>(this.baseUrl + '/delete-all/' + classId);
  }

  deleteRequest(requestId: number): Observable<ResponseDto<null>> {
    return this._httpCient.delete<ResponseDto<null>>(this.baseUrl + '/' + requestId);
  }
}
