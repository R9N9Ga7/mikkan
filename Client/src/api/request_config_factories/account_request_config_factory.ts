import { CREATE_ACCOUNT_URL } from '../../common/consts/pages_urls';
import { ACCOUNT_LOGIN_URL, ACCOUNT_REFRESH_TOKENS_URL } from '../../common/consts/server_urls';
import { FetchRequestConfig, FetchRequestConfigBuilder, FetchMethod } from '../../common/fetch_api';

class AccountRequestConfigFactory {
  public static createRefreshTokensConfig(): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .setMethod(FetchMethod.POST)
      .setUrl(ACCOUNT_REFRESH_TOKENS_URL)
      .setIsAuthRequired(false)
      .build();
  }

  public static createLoginConfig(): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .setMethod(FetchMethod.POST)
      .setUrl(ACCOUNT_LOGIN_URL)
      .setIsAuthRequired(false)
      .build();
  }

  public static createCreateAccountConfig(): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .setMethod(FetchMethod.POST)
      .setUrl(CREATE_ACCOUNT_URL)
      .setIsAuthRequired(false)
      .build();
  }
}

export default AccountRequestConfigFactory;
