import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {RegisterUser} from "../models/register-user";
import {Observable} from "rxjs";
import {LoginUser} from "../models/login-user";
import {authenticationResponse} from "../interfaces/authenticationResponse";

const API_BASE_URL: string = "http://localhost:5218/api/v1.0/account";
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  public currentUsername: string | null = null;
  constructor(private httpClient: HttpClient) { }

  public postRegister(registerUser: RegisterUser): Observable<authenticationResponse>{
    return this.httpClient.post<authenticationResponse>(`${API_BASE_URL}/register`, registerUser);
  }

  public postLogin(loginUser: LoginUser): Observable<authenticationResponse>{
    return this.httpClient.post<authenticationResponse>(`${API_BASE_URL}/login`, loginUser);
  }

  public getLogout(): Observable<string>{
    return this.httpClient.get<string>(`${API_BASE_URL}/logout`);
  }

  public postGenerateNewToken(): Observable<authenticationResponse> {
    const token = localStorage.getItem("token");
    const refreshToken = localStorage.getItem("refreshToken");

    return this.httpClient.post<authenticationResponse>(`${API_BASE_URL}generate-new-jwt-token`, {token: token, refreshToken: refreshToken})
  }
}
