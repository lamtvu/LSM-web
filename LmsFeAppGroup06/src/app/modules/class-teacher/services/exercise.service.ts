import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ExerciseReadDto } from '../../../Dto/ExerciseDto';
import { ResponseDto } from '../../../Dto/response';

@Injectable()
export class ExerciseService {
  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/exercise'

  constructor(
    private _httpClientService: HttpClient
  ) { }

  getExercises(classId: number): Observable<ResponseDto<ExerciseReadDto[]>> {
    return this._httpClientService.get<ResponseDto<ExerciseReadDto[]>>(this.baseUrl + '/' + classId);
  }

  createExercise(classId: number, createForm: ExerciseReadDto): Observable<ResponseDto<null>> {
    return this._httpClientService.post<ResponseDto<null>>(this.baseUrl + '/' + classId, createForm);
  }

  editExercise(exerciseId: number, editForm: ExerciseReadDto): Observable<ResponseDto<null>> {
    return this._httpClientService.put<ResponseDto<null>>(this.baseUrl + '/' + exerciseId, editForm);
  }

  deleteExercise(exerciseId: number): Observable<ResponseDto<null>> {
    return this._httpClientService.delete<ResponseDto<null>>(this.baseUrl + '/' + exerciseId);
  }
}
