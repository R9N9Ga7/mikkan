import { AccountLoginResponse, AuthAccountRequest } from '../../api/interfaces/account';
import AccountRequestConfigFactory from '../../api/request_config_factories/account_request_config_factory';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

function useFetchAccountLogin(
  params: FetchParams<AccountLoginResponse>,
): UseFetchResult<AuthAccountRequest, AccountLoginResponse> {
  const fetchRequestConfig = AccountRequestConfigFactory.createLoginConfig();
  return useFetch<AuthAccountRequest, AccountLoginResponse>({ fetchRequestConfig, ...params });
}

export default useFetchAccountLogin;
