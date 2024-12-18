import { AuthAccountRequest } from '../../api/interfaces/account';
import { CREATE_ACCOUNT_URL } from '../../consts/serverUrls';
import { FetchRequestConfigBuilder } from '../fetch/common/fetchRequestConfigBuilder';
import { UseFetchMethod } from '../fetch/common/useFetchMethod';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

function useFetchCreateAccount(
  params: FetchParams<null> = {},
): UseFetchResult<AuthAccountRequest, null> {
  const fetchRequestConfig = new FetchRequestConfigBuilder()
    .SetMethod(UseFetchMethod.POST)
    .SetUrl(CREATE_ACCOUNT_URL)
    .Build();
  return useFetch<AuthAccountRequest, null>({ fetchRequestConfig, ...params });
}

export default useFetchCreateAccount;
