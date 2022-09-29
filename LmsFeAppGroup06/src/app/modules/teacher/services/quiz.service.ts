import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QuizReadDto } from '../../../Dto/QuizDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable({
  providedIn: 'platform'
})
export class QuizService {

  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/quiz'

  quizId?: number
  quizIdEmit = new EventEmitter<number>()

  constructor(
    private _httpClient: HttpClient
  ) { }

  setQuizId(quizId: number) {
    this.quizId = quizId;
    this.quizIdEmit.emit(quizId);
  }

  getQuiz(quizId: number): Observable<ResponseDto<QuizReadDto>> {
    return this._httpClient.get<ResponseDto<QuizReadDto>>(this.baseUrl + '/' + quizId);
  }
}
