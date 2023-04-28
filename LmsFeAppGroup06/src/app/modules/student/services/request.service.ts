import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClassRequest } from 'src/app/Dto/class-request';
import { PageDataDto } from 'src/app/Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable({
  providedIn: 'root'
})
export class RequestService {

  readonly baseUrl = 'https://localhost:5001/api/request-student';

  constructor(
    private _httpClientService: HttpClient
  ) { }

  createRequest(classid: number): Observable<ResponseDto<null>> {
    return this._httpClientService.post<ResponseDto<null>>(this.baseUrl + '/', null, { params: { classid: classid } });
  }
  deleteRequest(classid: number): Observable<ResponseDto<null>> {
    return this._httpClientService.delete<ResponseDto<null>>(this.baseUrl + '/delete-class-request/' + classid);
  }
  getClassToRequest(searchValue:string):Observable<ResponseDto<PageDataDto<ClassRequest[]>>> {
    return this._httpClientService.get<ResponseDto<PageDataDto<ClassRequest[]>>>(this.baseUrl+'/class-request',{ params: {searchValue: searchValue}});
  } 
}
