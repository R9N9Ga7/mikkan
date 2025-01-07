import { Account } from '../common/interfaces/account';
import { Token } from './token';

export class AccountValidator {
  public constructor(account: Account) {
    this.refreshToken = new Token(account.refreshToken);
    this.accessToken = new Token(account.accessToken);
  }

  public isValid(): boolean {
    return this.refreshToken.isValid()
      && this.accessToken.isValid();
  }

  public isValidRefreshToken(): boolean {
    return this.refreshToken.isValid();
  }

  public isValidAccessToken(): boolean {
    return this.accessToken.isValid();
  }

  private refreshToken: Token;
  private accessToken: Token;
}
