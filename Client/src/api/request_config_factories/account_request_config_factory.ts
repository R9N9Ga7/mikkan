import { CREATE_ACCOUNT_URL } from '../../consts/pages_urls';
import { ACCOUNT_LOGIN_URL, ACCOUNT_REFRESH_TOKENS_URL } from '../../consts/server_urls';
import { FetchRequestConfig } from '../../hooks/fetch/common/fetch_request_config';
import { FetchRequestConfigBuilder } from '../../hooks/fetch/common/fetch_request_config_builder';
import { FetchMethod } from '../../hooks/fetch/common/fetch_method';

class AccountRequestConfigFactory {
  public static createRefreshTokensConfig(): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .SetMethod(FetchMethod.POST)
      .SetUrl(ACCOUNT_REFRESH_TOKENS_URL)
      .SetIsAuthRequired(false)
      .Build();
  }

  public static createLoginConfig(): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .SetMethod(FetchMethod.POST)
      .SetUrl(ACCOUNT_LOGIN_URL)
      .SetIsAuthRequired(false)
      .Build();
  }

  public static createCreateAccountConfig(): FetchRequestConfig {
    return new FetchRequestConfigBuilder()
      .SetMethod(FetchMethod.POST)
      .SetUrl(CREATE_ACCOUNT_URL)
      .SetIsAuthRequired(false)
      .Build();
  }
}

export default AccountRequestConfigFactory;
