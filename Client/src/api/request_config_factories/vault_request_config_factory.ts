import { VAULT_ADD_ITEM_URL } from '../../common/consts/server_urls';
import { FetchMethod, FetchRequestConfig, FetchRequestConfigBuilder } from '../../common/fetch_api';

class VaultRequestConfigFactory {
  public static createAddItemConfig(): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .setMethod(FetchMethod.POST)
      .setUrl(VAULT_ADD_ITEM_URL)
      .setIsAuthRequired(true)
      .build();
  }
}

export default VaultRequestConfigFactory;
