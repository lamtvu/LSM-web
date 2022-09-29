import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClassReadDto } from '../../../Dto/classDto';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class StudentService {
  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api';

  public editEmit = new EventEmitter<boolean>();
  constructor(
    private _httpClientService: HttpClient
  ) { }

  refreshList() {
    this.editEmit.emit(true);
  }

  getStudingClass(page: number, limit: number): Observable<ResponseDto<PageDataDto<ClassReadDto[]>>> {
    console.log(page);
    console.log(limit);
    return this._httpClientService.get<ResponseDto<PageDataDto<ClassReadDto[]>>>(this.baseUrl + '/class/studing', { params: { page: page, limit: limit } });
  }


}
