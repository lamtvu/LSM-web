import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ResponseDto } from '../../../Dto/response';
import { SubmissionQuizReaDto } from '../../../Dto/submissionDto';

@Injectable()
export class SubmisstionQuizService {
  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/submission-quiz';

  constructor(
    private _httpClient: HttpClient
  ) { }

  getSubmission(quizid: number): Observable<ResponseDto<SubmissionQuizReaDto>> {
    return this._httpClient.get<ResponseDto<SubmissionQuizReaDto>>(this.baseUrl + '/my-quiz/' + quizid);
  }

  submitAnswer(submissonId: number, answerId: number): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + '/answer/' + submissonId, null, { params: { answerId: answerId } });
  }

  submitQuiz(submissionId: number): Observable<ResponseDto<null>> {
    return this._httpClient.put<ResponseDto<null>>(this.baseUrl + '/submit/' + submissionId, null);
  }
}
