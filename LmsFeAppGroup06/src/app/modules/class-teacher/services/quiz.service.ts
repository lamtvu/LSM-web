import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuizCreateDto, QuizReadDto } from '../../../Dto/QuizDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class QuizService {

  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/quiz'

  constructor(
    private _httpClient: HttpClient
  ) { }

  getQuizs(classId: number): Observable<ResponseDto<QuizReadDto[]>> {
    return this._httpClient.get<ResponseDto<QuizReadDto[]>>(this.baseUrl + '/by-class/' + classId);
  }
  
  createQuiz(classId: number, createForm: QuizCreateDto): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + '/' + classId, createForm);
  }

  editQuiz(quizId: number, editForm: QuizCreateDto): Observable<ResponseDto<null>> {
    return this._httpClient.put<ResponseDto<null>>(this.baseUrl + '/' + quizId, editForm);
  }

  deleteQuiz(quizId: number): Observable<ResponseDto<null>> {
    return this._httpClient.delete<ResponseDto<null>>(this.baseUrl + '/' + quizId);
  }
}
