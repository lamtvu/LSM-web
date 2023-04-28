import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ContentCreateDto } from '../../../Dto/ContentDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class ContentService {

  readonly baseUrl = "https://localhost:5001/api/Content";
  public editEmit = new EventEmitter<boolean>();
  constructor(
    private _httpClientService: HttpClient
  ) { }

  refreshList(){
    this.editEmit.emit(true);
  }

  createContent(sectionId: number,formData: FormData): Observable<ResponseDto<null>> {
    return this._httpClientService.post<ResponseDto<null>>(this.baseUrl+"/"+sectionId, formData);
  }

  deleteContent(contentId: number): Observable<ResponseDto<null>> {
    return this._httpClientService.delete<ResponseDto<null>>(this.baseUrl+"/"+contentId);
  }

  editContent(contentId: number,contentForm: ContentCreateDto): Observable<ResponseDto<null>> {
    return this._httpClientService.put<ResponseDto<null>>(this.baseUrl+"/"+contentId, contentForm);
  }
  editFileContent(contentId: number,formData: FormData): Observable<ResponseDto<null>> {
    return this._httpClientService.put<ResponseDto<null>>(this.baseUrl+"/file/"+contentId, formData);
  }
}
