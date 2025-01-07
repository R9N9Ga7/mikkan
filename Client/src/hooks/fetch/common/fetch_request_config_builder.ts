import { FetchRequestConfig } from './fetch_request_config';
import { FetchMethod } from './fetch_method';

export class FetchRequestConfigBuilder {
  public SetUrl(url : string): FetchRequestConfigBuilder {
    this.requestConfig.url = url;
    return this;
  }

  public SetMethod(method: FetchMethod): FetchRequestConfigBuilder {
    this.requestConfig.method = method;
    return this;
  }

  public SetIsAuthRequired(isAuthRequired: boolean): FetchRequestConfigBuilder {
    this.requestConfig.isAuthRequired = isAuthRequired;
    return this;
  }

  public Build(): FetchRequestConfig {
    return this.requestConfig;
  }

  private requestConfig: FetchRequestConfig = new FetchRequestConfig();
}
