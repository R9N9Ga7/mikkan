import AccountRequestConfigFactory from '../../api/request_config_factories/account_request_config_factory';
import { AccountRefreshTokensRequest, AccountRefreshTokensResponse } from '../../api/interfaces/account';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

function useFetchAccountRefreshTokens(
  params: FetchParams<AccountRefreshTokensResponse>,
): UseFetchResult<AccountRefreshTokensRequest, AccountRefreshTokensResponse> {
  const fetchRequestConfig = AccountRequestConfigFactory.createRefreshTokensConfig();
  return useFetch<AccountRefreshTokensRequest, AccountRefreshTokensResponse>({ fetchRequestConfig, ...params });
}

export default useFetchAccountRefreshTokens;
