import { AuthAccountRequest } from '../../api/interfaces/account';
import AccountRequestConfigFactory from '../../api/request_config_factories/account_request_config_factory';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

function useFetchCreateAccount(
  params: FetchParams<null> = {},
): UseFetchResult<AuthAccountRequest, null> {
  const fetchRequestConfig = AccountRequestConfigFactory.createCreateAccountConfig();
  return useFetch<AuthAccountRequest, null>({ fetchRequestConfig, ...params });
}

export default useFetchCreateAccount;
