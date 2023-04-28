import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuestionReadDto } from '../../../Dto/QuestionDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class QuestionService {
  readonly baseUrl = 'https://localhost:5001/api/question';

  constructor(
    private _httpClient: HttpClient
  ) { }

  getQuestions(quizId: number): Observable<ResponseDto<{ id: number }[]>> {
    return this._httpClient.get<ResponseDto<{ id: number }[]>>(this.baseUrl + '/by-quiz-id/' + quizId);
  }

  getQuestion(questionId: number): Observable<ResponseDto<QuestionReadDto>> {
    return this._httpClient.get<ResponseDto<QuestionReadDto>>(this.baseUrl + '/' + questionId);
  }

}
