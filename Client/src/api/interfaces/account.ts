import { Account } from '../../common/interfaces/account';

export type AuthAccountRequest = {
  username: string;
  password: string;
};

export type AccountLoginResponse = Account;

export type AccountRefreshTokensRequest = Account;

export type AccountRefreshTokensResponse = Account;
