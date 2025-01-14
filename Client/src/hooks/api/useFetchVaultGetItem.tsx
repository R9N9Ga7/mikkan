import { VaultAllItemsResponse } from '../../api/interfaces/vault';
import VaultRequestConfigFactory from '../../api/request_config_factories/vault_request_config_factory';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

interface FetchVaultGetItem extends FetchParams<VaultAllItemsResponse> {
  id: string;
}

const useFetchVaultGetItem = (params: FetchVaultGetItem)
: UseFetchResult<null, VaultAllItemsResponse> => {
  const fetchRequestConfig = VaultRequestConfigFactory.createGetItemConfig(params.id);
  return useFetch<null, VaultAllItemsResponse>({ fetchRequestConfig, ...params });
};

export default useFetchVaultGetItem;
