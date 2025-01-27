import { EditVaultItemRequest } from '../../api/interfaces/vault';
import VaultRequestConfigFactory from '../../api/request_config_factories/vault_request_config_factory';
import useFetch, { FetchParams, UseFetchResult } from '../fetch/useFetch';

const useFetchVaultEditItem = (params: FetchParams<null>)
: UseFetchResult<EditVaultItemRequest, null> => {
  const fetchRequestConfig = VaultRequestConfigFactory.createEditItemConfig();
  return useFetch<EditVaultItemRequest, null>({
    fetchRequestConfig,
    successMessage: 'The item has been updated.',
    ...params,
  });
};

export default useFetchVaultEditItem;
