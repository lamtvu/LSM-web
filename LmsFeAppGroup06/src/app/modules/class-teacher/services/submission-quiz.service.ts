import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';
import { SubmissionQuizReaDto } from '../../../Dto/submissionDto';

@Injectable()
export class SubmissionQuizService {

  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/submission-quiz';

  constructor(
    private _httpClient: HttpClient
  ) { }

  getSubmissionQuizByQuizId(quizId: number, pageIndex: number, pageSize: number, searchValue: string): Observable<ResponseDto<PageDataDto<SubmissionQuizReaDto[]>>> {
    return this._httpClient.get<ResponseDto<PageDataDto<SubmissionQuizReaDto[]>>>(this.baseUrl + '/by-quiz/' + quizId,
    {params:{page: pageIndex, limit: pageSize, searchValue: searchValue}});
  }

}
