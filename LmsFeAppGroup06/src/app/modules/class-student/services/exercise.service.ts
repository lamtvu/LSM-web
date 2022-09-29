import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ExerciseReadDto } from '../../../Dto/ExerciseDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class ExerciseService {
  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/exercise'
  exercise?: ExerciseReadDto;
  exerciseEmmit = new EventEmitter<ExerciseReadDto>();

  constructor(
    private _httpClient: HttpClient
  ) { }

  getExercises(classId: number): Observable<ResponseDto<ExerciseReadDto[]>> {
    return this._httpClient.get<ResponseDto<ExerciseReadDto[]>>(this.baseUrl + '/' + classId);
  }

  getExercise(exerciseId: number): Observable<ResponseDto<ExerciseReadDto>> {
    return this._httpClient.get<ResponseDto<ExerciseReadDto>>(this.baseUrl + '/detail/' + exerciseId);
  }

  setExercise(exercise: ExerciseReadDto) {
    this.exercise = exercise;
    this.exerciseEmmit.emit(this.exercise);
  }

}
