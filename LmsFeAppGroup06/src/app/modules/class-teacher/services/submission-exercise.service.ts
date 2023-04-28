import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';
import { SubmissionExerciseReadDto } from '../../../Dto/submissionDto';

@Injectable()
export class SubmissionExerciseService {

  readonly baseUrl = 'https://localhost:5001/api/submission-exercise';

  constructor(
    private _httpClient: HttpClient
  ) { }

  getSubmissionByExerciseId(exerciseId: number, pageIndex: number, pageSize: number, searchValue: string): Observable<ResponseDto<PageDataDto<SubmissionExerciseReadDto[]>>> {
    return this._httpClient.get<ResponseDto<PageDataDto<SubmissionExerciseReadDto[]>>>(this.baseUrl + '/by-page/' + exerciseId, {
      params: {
        page: pageIndex, limit: pageSize, searchValue: searchValue
      }
    });
  }

  changeScore(submissionId: number, changeForm: { core: number, comment: string }): Observable<ResponseDto<null>> {
    return this._httpClient.put<ResponseDto<null>>(this.baseUrl + '/change-score/' + submissionId, changeForm);
  }

  dowloadFile(submissionId: number): Observable<any> {
    return this._httpClient.get<any>(this.baseUrl + '/file/' + submissionId, { responseType: 'blob' as 'json' })
  }
}
