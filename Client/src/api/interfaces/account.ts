export interface AuthAccountRequest {
  username: string;
  password: string;
};

export interface AccountLoginResponse {
  accessToken: string;
  refreshToken: string;
}
