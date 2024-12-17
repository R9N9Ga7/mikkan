import { CreateAccountRequest } from '../../api/interfaces/account';
import { CREATE_ACCOUNT_URL } from '../../consts/serverUrls';
import { FetchRequestConfigBuilder } from '../fetch/common/fetchRequestConfigBuilder';
import { UseFetchMethod } from '../fetch/common/useFetchMethod';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

function useFetchCreateAccount(
  params: FetchParams<null> = {},
): UseFetchResult<CreateAccountRequest, null> {
  const fetchRequestConfig = new FetchRequestConfigBuilder()
    .SetMethod(UseFetchMethod.POST)
    .SetUrl(CREATE_ACCOUNT_URL)
    .Build();
  return useFetch<CreateAccountRequest, null>({ fetchRequestConfig, ...params });
}

export default useFetchCreateAccount;
