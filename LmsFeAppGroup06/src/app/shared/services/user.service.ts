
import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserReadDto } from '../../Dto/userDto';
import { ResponseDto } from '../../Dto/response';
import { AnnouncenmentReadDto } from '../../Dto/AnnouncenmentDto';
import { UserChangePasswordDto } from '../../Dto/userChangePasswordDto';
import { ChangeUserInfoDto } from '../../Dto/change-user-info-dto';

@Injectable(
  { providedIn: 'platform' }
)
export class UserService {
  readonly baseUrl = 'https://localhost:5001/api/user';

  user?: UserReadDto;
  userEmmit = new EventEmitter<UserReadDto>();

  constructor(private _httpClientService: HttpClient) { }

  getDetail(): Observable<ResponseDto<UserReadDto>> {
    return this._httpClientService.get<ResponseDto<UserReadDto>>(this.baseUrl + '/detail');
  }

  setUser(user: UserReadDto) {
    this.user = user;
    this.userEmmit.emit(user);
  }

  getAnnouncement(): Observable<ResponseDto<AnnouncenmentReadDto[]>> {
    return this._httpClientService.get<ResponseDto<AnnouncenmentReadDto[]>>('https://localhost:5001/api/announcement/get-all-notify');
  }

  changePassword(pass: UserChangePasswordDto): Observable<ResponseDto<string>> {
    return this._httpClientService.put<ResponseDto<string>>(this.baseUrl + '/change-password', pass);
  }

  // Change information
  public editEmit = new EventEmitter<boolean>();
  refreshList() {
    this.editEmit.emit(true);
  }

  changeInfo(changeUserInfor: ChangeUserInfoDto): Observable<ResponseDto<object>> {
    return this._httpClientService.put<ResponseDto<object>>(this.baseUrl + "/change-infor", changeUserInfor);
  }

  changeAvatar(formData: FormData): Observable<ResponseDto<null>> {
    return this._httpClientService.put<ResponseDto<null>>(this.baseUrl + '/change-avatar', formData);
  }

  getVerify(email: string): Observable<ResponseDto<null>> {
    return this._httpClientService.post<ResponseDto<null>>('https://localhost:5001/api/auth/getVerify', null, { params: { email: email } });
  }

  changeRole(userId: number, roleId: number): Observable<ResponseDto<null>> {
    return this._httpClientService.put<ResponseDto<null>>(this.baseUrl + '/change-role/' + userId, null, { params: { roleId: roleId } });
  }
}

