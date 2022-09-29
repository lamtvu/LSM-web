import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AnnouncenmentReadDto } from '../../../Dto/AnnouncenmentDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class PlanService {
  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/announcement';
  constructor(
    private _httpClient: HttpClient
  ) { }

  getPlans(classId: number): Observable<ResponseDto<AnnouncenmentReadDto[]>> {
    return this._httpClient.get<ResponseDto<AnnouncenmentReadDto[]>>(this.baseUrl + '/get-all-program/' + classId);
  }
  
  getNotifications(classId: number): Observable<ResponseDto<AnnouncenmentReadDto[]>> {
    return this._httpClient.get<ResponseDto<AnnouncenmentReadDto[]>>(this.baseUrl + '/get-all-notify/' + classId);
  }
}
