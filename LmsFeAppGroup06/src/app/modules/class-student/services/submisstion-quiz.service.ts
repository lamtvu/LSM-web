import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ResponseDto } from '../../../Dto/response';
import { SubmissionQuizReaDto } from '../../../Dto/submissionDto';

@Injectable()
export class SubmisstionQuizService {

  readonly baseUrl = 'https://localhost:5001/api/submission-quiz';

  constructor(
    private _httpClient: HttpClient
  ) { }

  addSubmission(quizId: number): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + "/" + quizId, null);
  }

  getSubmission(quizid: number): Observable<ResponseDto<SubmissionQuizReaDto>> {
    return this._httpClient.get<ResponseDto<SubmissionQuizReaDto>>(this.baseUrl + '/my-quiz/' + quizid);
  }

  getOwnedSubmissions(classId: number): Observable<ResponseDto<SubmissionQuizReaDto[]>> {
    return this._httpClient.get<ResponseDto<SubmissionQuizReaDto[]>>(this.baseUrl + '/my-quiz-in-class/' + classId);
  }
}
