import VaultRequestConfigFactory from '../../api/request_config_factories/vault_request_config_factory';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

interface FetchVaultRemoveItem extends FetchParams<null> {
  id: string;
}

const useFetchVaultRemoveItem = (params: FetchVaultRemoveItem)
: UseFetchResult<null, null> => {
  const fetchRequestConfig = VaultRequestConfigFactory.createRemoveItemConfig(params.id);
  return useFetch<null, null>({ fetchRequestConfig, ...params });
};

export default useFetchVaultRemoveItem;
