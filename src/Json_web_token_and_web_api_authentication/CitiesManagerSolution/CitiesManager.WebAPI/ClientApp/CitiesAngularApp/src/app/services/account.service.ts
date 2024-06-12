import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {RegisterUser} from "../models/register-user";
import {Observable} from "rxjs";
import {LoginUser} from "../models/login-user";

const API_BASE_URL: string = "http://localhost:5218/api/v1.0/account";
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient) { }

  public postRegister(registerUser: RegisterUser): Observable<RegisterUser>{
    return this.httpClient.post<RegisterUser>(`${API_BASE_URL}/register`, registerUser);
  }

  public postLogin(loginUser: LoginUser): Observable<LoginUser>{
    return this.httpClient.post<LoginUser>(`${API_BASE_URL}/login`, loginUser);
  }

  public getLogout(): Observable<string>{
    return this.httpClient.get<string>(`${API_BASE_URL}/logout`);
  }
}
