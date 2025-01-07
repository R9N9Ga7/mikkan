export interface AuthAccountRequest {
  username: string;
  password: string;
};

export type Account = {
  accessToken: string;
  refreshToken: string;
};

export interface AccountLoginResponse extends Account {}

export interface AccountRefreshTokensRequest extends Account {}

export interface AccountRefreshTokensResponse extends Account {}

