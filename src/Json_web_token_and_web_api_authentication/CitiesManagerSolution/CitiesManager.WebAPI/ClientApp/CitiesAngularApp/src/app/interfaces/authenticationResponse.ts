export interface authenticationResponse {
  personName: string;
  email: string;
  token: string;
  expiration: string;
  refreshToken: string;
}

export interface refreshTokenResponse {
  token: string;
  refreshToken: string;
}
