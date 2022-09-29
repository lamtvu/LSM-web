import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuestionCreateDto, QuestionReadDto } from '../../../Dto/QuestionDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class questionService {

  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/question'

  constructor(
    private _httpClient: HttpClient
  ) { }

  getQuestions(questionId: number): Observable<ResponseDto<{ id: number }[]>> {
    return this._httpClient.get<ResponseDto<{ id: number }[]>>(this.baseUrl + '/by-quiz-id/' + questionId);
  }

  createQuestion(quizId: number, createForm: QuestionCreateDto): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + '/' + quizId, createForm);
  }

  editQuestion(quizId: number, editForm: QuestionCreateDto): Observable<ResponseDto<QuestionReadDto>> {
    return this._httpClient.put<ResponseDto<QuestionReadDto>>(this.baseUrl + '/' + quizId, editForm);
  }

  getQuestion(questionId: number): Observable<ResponseDto<QuestionReadDto>> {
    return this._httpClient.get<ResponseDto<QuestionReadDto>>(this.baseUrl + '/' + questionId);
  }

  deleteQuestion(questionId: number): Observable<ResponseDto<null>> {
    return this._httpClient.delete<ResponseDto<null>>(this.baseUrl + '/' + questionId);
  }
}
