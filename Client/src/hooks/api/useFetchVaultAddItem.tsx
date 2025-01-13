import { VaultAddItemRequest } from '../../api/interfaces/vault';
import VaultRequestConfigFactory from '../../api/request_config_factories/vault_request_config_factory';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

const useFetchVaultAddItem = (params: FetchParams<null>): UseFetchResult<VaultAddItemRequest, null> => {
  const fetchRequestConfig = VaultRequestConfigFactory.createAddItemConfig();
  return useFetch<VaultAddItemRequest, null>({ fetchRequestConfig, ...params });
};

export default useFetchVaultAddItem;
