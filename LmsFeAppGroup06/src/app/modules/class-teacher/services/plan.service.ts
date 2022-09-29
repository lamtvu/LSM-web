import { HttpClient } from '@angular/common/http';
import { ClassField } from '@angular/compiler';
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

  createPlan(classId: number, createPlanForm: { title: string, content: string }): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + '/program/' + classId, createPlanForm);
  }
  getPlansPage(pageIndex: number, limit: number, classId: number): Observable<ResponseDto<AnnouncenmentReadDto[]>> {
    return this._httpClient.get<ResponseDto<AnnouncenmentReadDto[]>>(this.baseUrl + '/get-by-page-program/' + classId, { params: { page: pageIndex, limit: limit } });
  }
  getPlans(classId: number): Observable<ResponseDto<AnnouncenmentReadDto[]>> {
    return this._httpClient.get<ResponseDto<AnnouncenmentReadDto[]>>(this.baseUrl + '/get-all-program/' + classId);
  }
  editPlans(id: number, editPlanForm: { title: string, content: string }): Observable<ResponseDto<null>> {
    return this._httpClient.put<ResponseDto<null>>(this.baseUrl + '/' + id, editPlanForm);
  }
  deletePlan(id: number): Observable<ResponseDto<null>> {
    return this._httpClient.delete<ResponseDto<null>>(this.baseUrl + '/' + id);
  }
}
