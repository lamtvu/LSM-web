import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ReportReadDTO } from '../../../Dto/report-read-dto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class ReportService {

  readonly baseUrl = 'https://localhost:5001/api/report';
  constructor(
    private _httpClient: HttpClient
  ) { }

  createReport(classId: number, formData:ReportReadDTO): Observable<ResponseDto<ReportReadDTO>> {
    return this._httpClient.post<ResponseDto<ReportReadDTO>>('https://localhost:5001/api/report',{params:{classId: classId,formData:formData}});
  }
}
