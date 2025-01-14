import { VAULT_ADD_ITEM_URL, VAULT_GET_ALL_ITEMS_URL, VAULT_GET_ITEM_URL } from '../../common/consts/server_urls';
import { FetchMethod, FetchRequestConfig, FetchRequestConfigBuilder } from '../../common/fetch_api';

class VaultRequestConfigFactory {
  public static createAddItemConfig(): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .setMethod(FetchMethod.POST)
      .setUrl(VAULT_ADD_ITEM_URL)
      .setIsAuthRequired(true)
      .build();
  }

  public static createGetAllItemsConfig(): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .setMethod(FetchMethod.GET)
      .setUrl(VAULT_GET_ALL_ITEMS_URL)
      .setIsAuthRequired(true)
      .build();
  }

  public static createGetItemConfig(id: string): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .setMethod(FetchMethod.GET)
      .setUrl(`${VAULT_GET_ITEM_URL}/${id}`)
      .setIsAuthRequired(true)
      .build();
  }
}

export default VaultRequestConfigFactory;
