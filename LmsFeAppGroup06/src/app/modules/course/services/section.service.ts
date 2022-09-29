import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ContentReadDto } from '../../../Dto/ContentDto';
import { ResponseDto } from '../../../Dto/response';
import { SectionCreateDto, SectionReadDto } from '../../../Dto/SectionDto';

@Injectable()
export class SectionService {

  readonly baseUrl = "https://lmstechbe.azurewebsites.net/api/Section";
  public editEmit = new EventEmitter<boolean>();
  public section?: SectionReadDto[];
  constructor(
    private _httpClientService: HttpClient
  ) { }

  refreshList(){
    this.editEmit.emit(true);
  }

  createSection(sectionId: number,createForm: SectionCreateDto): Observable<ResponseDto<null>> {
    return this._httpClientService.post<ResponseDto<null>>(this.baseUrl+"/"+sectionId, createForm);
  }

  getSections(courseId: number): Observable<ResponseDto<SectionReadDto[]>> {
    return this._httpClientService.get<ResponseDto<SectionReadDto[]>>(this.baseUrl +"/get-by-courseid"+"/"+courseId);
  }

  deleteSection(sectionId: number): Observable<ResponseDto<null>> {
    return this._httpClientService.delete<ResponseDto<null>>(this.baseUrl+"/"+sectionId);
  }

  editSection(sectionId: number, editForm: SectionCreateDto): Observable<ResponseDto<null>> {
    return this._httpClientService.put<ResponseDto<null>>(this.baseUrl+"/"+sectionId, editForm);
  }

  getContent(contentId: number, data: {responseType: string}): Observable<ResponseDto<ContentReadDto>>{
    return this._httpClientService.get<ResponseDto<ContentReadDto>>('https://lmstechbe.azurewebsites.net/api/Content/file/'+contentId);
  }

}
