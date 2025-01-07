import { Account } from '../api/interfaces/account';

class AccountStorage {
  public static set(account: Account): void {
    localStorage.setItem(this.REFRESH_TOKEN, account.refreshToken);
    localStorage.setItem(this.ACCESS_TOKEN, account.accessToken);

    this.account = account;
  }

  public static get(): Account | null {
    if (this.account) {
      return this.account;
    }

    const accessToken = localStorage.getItem(this.ACCESS_TOKEN);
    const refreshToken = localStorage.getItem(this.REFRESH_TOKEN);

    if (accessToken && refreshToken) {
      this.account = { accessToken, refreshToken };
    }

    return this.account;
  }

  public static clear(): void {
    localStorage.removeItem(this.ACCESS_TOKEN);
    localStorage.removeItem(this.REFRESH_TOKEN);

    this.account = null;
  }

  private static REFRESH_TOKEN = 'refresh-token';
  private static ACCESS_TOKEN = 'access-token';

  private static account: Account | null = null;
}

export default AccountStorage;
