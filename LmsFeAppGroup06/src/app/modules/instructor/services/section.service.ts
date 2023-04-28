import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ResponseDto } from '../../../Dto/response';
import { SectionCreateDto, SectionReadDto } from '../../../Dto/SectionDto';

@Injectable()
export class SectionService {

  readonly baseUrl = "https://localhost:5001/api/Section";
  public editEmit = new EventEmitter<boolean>();
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

}
