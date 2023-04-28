import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuizReadDto } from '../../../Dto/QuizDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class QuizService {
  readonly baseUrl = 'https://localhost:5001/api/quiz'

  constructor(
    private _httpClient: HttpClient
  ) { }

  getQuizs(classId: number): Observable<ResponseDto<QuizReadDto[]>> {
    return this._httpClient.get<ResponseDto<QuizReadDto[]>>(this.baseUrl + '/by-class/' + classId);
  }

  getQuiz(quizId: number): Observable<ResponseDto<QuizReadDto>> {
    return this._httpClient.get<ResponseDto<QuizReadDto>>(this.baseUrl + '/' + quizId);
  }
}
