import { AccountLoginResponse, AuthAccountRequest } from '../../api/interfaces/account';
import { ACCOUNT_LOGIN_URL } from '../../consts/serverUrls';
import { FetchRequestConfigBuilder } from '../fetch/common/fetchRequestConfigBuilder';
import { UseFetchMethod } from '../fetch/common/useFetchMethod';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

function useFetchAccountLogin(
  params: FetchParams<AccountLoginResponse>,
): UseFetchResult<AuthAccountRequest, AccountLoginResponse> {
  const fetchRequestConfig = new FetchRequestConfigBuilder()
    .SetMethod(UseFetchMethod.POST)
    .SetUrl(ACCOUNT_LOGIN_URL)
    .Build();
  return useFetch<AuthAccountRequest, AccountLoginResponse>({ fetchRequestConfig, ...params });
}

export default useFetchAccountLogin;
