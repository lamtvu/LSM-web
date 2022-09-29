import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ObservedValuesFromArray } from 'rxjs';
import { ResponseDto } from '../../../Dto/response';
import { SubmissionExerciseReadDto, SubmissionQuizReaDto } from '../../../Dto/submissionDto';

@Injectable()
export class SubmisstionExerciseService {

  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/submission-exercise';

  constructor(
    private _httpClient: HttpClient
  ) { }

  getSubmisstion(exerciseId: number): Observable<ResponseDto<SubmissionExerciseReadDto>> {
    return this._httpClient.get<ResponseDto<SubmissionExerciseReadDto>>(this.baseUrl + '/' + exerciseId);
  }

  upLoadSubmisstion(exerciseId: number, formData: FormData): Observable<ResponseDto<null>> {
    return this._httpClient.post<ResponseDto<null>>(this.baseUrl + '/' + exerciseId, formData);
  }

  dowloadFile(submissionId: number): Observable<any> {
    return this._httpClient.get<any>(this.baseUrl + '/file/' + submissionId, { responseType: 'blob' as 'json' })
  }

  deleteSubmission(submission: SubmissionExerciseReadDto): Observable<ResponseDto<null>> {
    return this._httpClient.delete<ResponseDto<null>>(this.baseUrl + '/' + submission.id);
  }

  getOwnedInclass(classId: number): Observable<ResponseDto<SubmissionExerciseReadDto[]>> {
    return this._httpClient.get<ResponseDto<SubmissionExerciseReadDto[]>>(this.baseUrl + '/owned-in-class/' + classId);
  }

}
