import { FC, PropsWithChildren, useEffect } from 'react';
import { AccountRefreshTokensResponse } from '../api/interfaces/account';
import { Account } from '../common/interfaces/account';
import { useNavigate } from 'react-router-dom';
import { AccountValidator } from '../utils/account_validator';
import { LOGIN_FULL_URL } from '../common/consts/pages_urls';
import useFetchAccountRefreshTokens from '../hooks/api/useFetchAccountRefreshTokens';
import AccountStorage from '../utils/account_storage';

const AccountMiddleware: FC<PropsWithChildren> = ({ children }) => {
  const { fetchData } = useFetchAccountRefreshTokens({
    onSuccess: (data: AccountRefreshTokensResponse | null) => {
      AccountStorage.set(data as Account);
    },
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    onError: (_: string) => {
      goToLoginPage();
    },
  });

  const navigate = useNavigate();

  useEffect(() => {
    validateAccount(AccountStorage.get());
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const validateAccount = async (acc: Account | null): Promise<void> => {
    if (!acc) {
      goToLoginPage();
      return;
    }

    const accountValidator = new AccountValidator(acc);
    if (accountValidator.isValid()) {
      return;
    }

    if (!accountValidator.isValidRefreshToken()) {
      goToLoginPage();
    }

    if (!accountValidator.isValidAccessToken()) {
      await fetchData(acc);
    }
  };

  const goToLoginPage = (): void => {
    AccountStorage.clear();
    navigate(LOGIN_FULL_URL);
  };

  return (<>{children}</>);
};

export default AccountMiddleware;
