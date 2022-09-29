import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AnnouncenmentReadDto } from '../../../Dto/AnnouncenmentDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class NotificationService {
  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/announcement';

  constructor(
    private _httpClient: HttpClient
  ) { }

  getNotifications(classId: number): Observable<ResponseDto<AnnouncenmentReadDto[]>> {
    return this._httpClient.get<ResponseDto<AnnouncenmentReadDto[]>>(this.baseUrl + '/get-all-notify/' + classId);
  }

  createNotification(classId: number, createForm: { title: string, content: string }): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + '/notify/' + classId, createForm);
  }

  editNotification(notificationId: number, editForm: { title: string, content: string }): Observable<ResponseDto<null>> {
    return this._httpClient.put<ResponseDto<null>>(this.baseUrl + '/' + notificationId, editForm);
  }

  deleteNofity(id: number): Observable<ResponseDto<null>> {
    return this._httpClient.delete<ResponseDto<null>>(this.baseUrl + '/' + id);
  }
}
