import { PropsWithChildren, useEffect, useState } from 'react';
import AccountContext, { Account } from '../contexts/AccountContext';

const REFRESH_TOKEN = 'refresh-token';
const ACCESS_TOKEN = 'access-token';

function AccountProvider({ children }: PropsWithChildren) {
  const [account, setAccount] = useState<Account | null>(null);

  useEffect(() => {
    loadAccount();
  }, []);

  useEffect(() => {
    saveAccount();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [account]);

  const saveAccount = () => {
    if (account === null) {
      return;
    }

    localStorage.setItem(REFRESH_TOKEN, account.refreshToken);
    localStorage.setItem(ACCESS_TOKEN, account.accessToken);
  };

  const loadAccount = () => {
    const accessToken = localStorage.getItem(ACCESS_TOKEN);
    const refreshToken = localStorage.getItem(REFRESH_TOKEN);

    let loadedAccount: Account | null = null;

    if (accessToken && refreshToken) {
      loadedAccount = { accessToken, refreshToken };
    }

    setAccount(loadedAccount);
  };

  return (
    <AccountContext.Provider value={{ account, setAccount }}>
      { children }
    </AccountContext.Provider>
  );
}

export default AccountProvider;
