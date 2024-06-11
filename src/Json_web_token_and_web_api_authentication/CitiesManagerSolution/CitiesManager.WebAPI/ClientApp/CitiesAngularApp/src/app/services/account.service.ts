import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {RegisterUser} from "../models/register-user";
import {Observable} from "rxjs";

const API_BASE_URL: string = "http://localhost:5218/api/v1.0/account";
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient) { }

  public postRegister(registerUser: RegisterUser): Observable<RegisterUser>{
    return this.httpClient.post<RegisterUser>(`${API_BASE_URL}/register`, registerUser)
  }
}
