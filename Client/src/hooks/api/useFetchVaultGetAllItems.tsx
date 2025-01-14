import { VaultAllItemsResponse } from '../../api/interfaces/vault';
import VaultRequestConfigFactory from '../../api/request_config_factories/vault_request_config_factory';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

const useFetchVaultGetAllItems = (params: FetchParams<VaultAllItemsResponse[]>)
: UseFetchResult<null, VaultAllItemsResponse[]> => {
  const fetchRequestConfig = VaultRequestConfigFactory.createGetAllItemsConfig();
  return useFetch<null, VaultAllItemsResponse[]>({ fetchRequestConfig, ...params });
};

export default useFetchVaultGetAllItems;
