import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClassReadDto } from '../../../Dto/classDto';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';
import { UserReadDto } from '../../../Dto/userDto';

@Injectable({
  providedIn: 'root'
})
export class ClassService {
  public classIdEmit = new EventEmitter<number>();
  public classEmit = new EventEmitter<ClassReadDto>();
  public classId?: number;
  public class?: ClassReadDto;
  readonly baseUrl = 'https://localhost:5001/api/class';

  constructor(
    private _httpClient: HttpClient
  ) { }

  setClassId(classId: number) {
    this.classId = classId;
    this.classIdEmit.emit(classId);
  }
  setClass(_class: ClassReadDto) {
    this.class = _class;
    this.classEmit.emit(this.class);
  }

  getStudents(pageIndex: number, limit: number): Observable<ResponseDto<PageDataDto<UserReadDto[]>>> {
    return this._httpClient
      .get<ResponseDto<PageDataDto<UserReadDto[]>>>(this.baseUrl + '/students/' + this.classId,
        { params: { page: pageIndex, limit: limit } });
  }

  getClass(): Observable<ResponseDto<ClassReadDto>> {
    return this._httpClient.get<ResponseDto<ClassReadDto>>(this.baseUrl + '/' + this.classId);
  }



  
}
