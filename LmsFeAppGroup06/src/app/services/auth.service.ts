import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { regiserDto } from '../Dto/register';
import { ResponseDto } from '../Dto/response';
import { TokenDto } from '../Dto/token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  readonly baseUrl = 'https://lmstechbe.azurewebsites.net/api/auth';
  constructor(private _httpClientService: HttpClient) { }

  login(loginForm: { username: string, password: string }): Observable<ResponseDto<TokenDto>> {
    return this._httpClientService.post<ResponseDto<TokenDto>>(this.baseUrl + '/login', loginForm);
  }

  register(registerForm: regiserDto): Observable<ResponseDto<TokenDto>> {
    return this._httpClientService.post<ResponseDto<TokenDto>>(this.baseUrl + '/register', registerForm);
  }

  getVerifyGmail(email: string): Observable<ResponseDto<string>> {
    return this._httpClientService.post<ResponseDto<string>>(this.baseUrl + '/getVerify', null, { params: { 'email': email } });
  }

}
