import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChangeUserInfoDto } from '../../../Dto/change-user-info-dto';
import { CourseReadDto } from '../../../Dto/CourseDto';
import { PageDataDto } from '../../../Dto/pageDataDto';
import { ResponseDto } from '../../../Dto/response';
import { UserChangePasswordDto } from '../../../Dto/userChangePasswordDto';
import { UserReadDto } from '../../../Dto/userDto';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  readonly baseUrl = "https://lmstechbe.azurewebsites.net/api/user";
  public editEmit = new EventEmitter<boolean>();
  constructor(
    private _httpClientService: HttpClient
  ) { }

  refreshList(){
    this.editEmit.emit(true);
  }

  getAccountList(searchValue:string): Observable<ResponseDto<PageDataDto<UserReadDto[]>>> {
    return this._httpClientService.get<ResponseDto<PageDataDto<UserReadDto[]>>>(this.baseUrl +"/get-all", {params:{searchValue : searchValue}});
  }

  changeLock(id: number): Observable<ResponseDto<string>> {
    return this._httpClientService.put<ResponseDto<string>>(this.baseUrl + "/change-lock/"+id,{params:{id:id}});
  }

  changeAvatar(id: number, formData: FormData): Observable<ResponseDto<string>> {
    return this._httpClientService.put<ResponseDto<string>>(this.baseUrl + "/change-avatar/"+id,formData,{params:{id:id}});
  }

  getAvatar(id: number): Observable<ResponseDto<object>> {
    return this._httpClientService.get<ResponseDto<object>>(this.baseUrl + "/avatar/"+id,{params:{id:id}});
  }

  changeInfo(changeUserInfor: ChangeUserInfoDto): Observable<ResponseDto<object>> {
    return this._httpClientService.put<ResponseDto<object>>(this.baseUrl + "/change-infor",changeUserInfor);
  }

  changePassword(pass: UserChangePasswordDto):Observable<ResponseDto<string>> {
    return this._httpClientService.put<ResponseDto<string>>(this.baseUrl + '/change-password', pass);
  }


  getCourseList(searchValue:string): Observable<ResponseDto<PageDataDto<CourseReadDto[]>>> {
    return this._httpClientService.get<ResponseDto<PageDataDto<CourseReadDto[]>>>("https://lmstechbe.azurewebsites.net/api/course/get-all", {params:{searchValue : searchValue}});
  }

  changeLockCourse(id: number): Observable<ResponseDto<string>> {
    return this._httpClientService.put<ResponseDto<string>>("https://lmstechbe.azurewebsites.net/api/course/change-lock/"+id,{params:{id:id}});
  }

}
