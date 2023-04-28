import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ReportReadDTO } from '../../../Dto/report-read-dto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class ReportService {

  readonly baseUrl = 'https://localhost:5001/api/report';

  constructor(
    private _httpCient: HttpClient
  ) { }

  getReports(classId: number): Observable<ResponseDto<ReportReadDTO[]>> {
    return this._httpCient.get<ResponseDto<ReportReadDTO[]>>(this.baseUrl + '/get-all/'+classId);
  }

}
