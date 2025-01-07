import { useState } from 'react';
import { FetchRequestConfig, FetchApi } from '../../common/fetch_api';
import { AccountValidator } from '../../utils/account_validator';
import { AccountRefreshTokensRequest, AccountRefreshTokensResponse } from '../../api/interfaces/account';
import { Account } from '../../common/interfaces/account';
import { useNavigate } from 'react-router-dom';
import { LOGIN_FULL_URL } from '../../common/consts/pages_urls';
import AccountRequestConfigFactory from '../../api/request_config_factories/account_request_config_factory';
import AccountStorage from '../../utils/account_storage';

export interface FetchParams<TResult> {
  onSuccess?: (data: TResult | null) => void;
  onError?: (error: string) => void;
}

export interface UseFetchParams<TResult> extends FetchParams<TResult> {
  fetchRequestConfig: FetchRequestConfig,
};

export type UseFetchResult<TBody, TResult> = {
  data?: TResult | null,
  isLoading: boolean,
  error: string | null,
  fetchData: (body: TBody) => Promise<void>,
};

function useFetch<TBody, TResult>(
  {
    fetchRequestConfig,
    onSuccess,
    onError,
  }: UseFetchParams<TResult>,
): UseFetchResult<TBody, TResult> {
  const [data, setData] = useState<TResult | null>();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  const fetchData = async (body: TBody): Promise<void> => {
    const accountTokens = await getAccountTokens();

    if (fetchRequestConfig.isAuthRequired && !accountTokens) {
      goToLoginPage();
    }

    setIsLoading(true);
    setError(null);

    const fetchApi = new FetchApi<TBody, TResult>(fetchRequestConfig, accountTokens, body);

    if (onSuccess) {
      fetchApi.addEventListenerOnSuccess((response: TResult) => {
        if (onSuccess) {
          onSuccess(response);
        }
        setData(response);
      });
    }

    if (onError) {
      fetchApi.addEventListenerOnError((error: string) => {
        if (onError) {
          onError(error);
        }
        setError(error);
      });
    }

    fetchApi.addEventListenerOnFinish(() => {
      setIsLoading(false);
    });

    await fetchApi.sendRequest();
  };

  const getAccountTokens = async (): Promise<Account | null> => {
    const account = AccountStorage.get();
    if (!account) {
      return null;
    }

    const accountValidator = new AccountValidator(account);

    if (accountValidator.isValid()) {
      return account;
    }

    if (!accountValidator.isValidRefreshToken()) {
      return null;
    }

    const refreshTokensRequestConfig = AccountRequestConfigFactory.createRefreshTokensConfig();
    const fetchApi = new FetchApi<AccountRefreshTokensRequest, AccountRefreshTokensResponse>(refreshTokensRequestConfig, account, account);
    await fetchApi.sendRequest();

    const accountTokens = fetchApi.getResponse();
    AccountStorage.set(accountTokens as Account);

    return accountTokens;
  };

  const goToLoginPage = (): void => {
    navigate(LOGIN_FULL_URL);
  };

  return {
    data,
    isLoading,
    error,
    fetchData,
  };
}

export default useFetch;
