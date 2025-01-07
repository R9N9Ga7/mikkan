import { FetchRequestConfig } from './fetch_request_config';
import { FetchMethod } from './fetch_method';

export class FetchRequestConfigBuilder {
  public setUrl(url : string): FetchRequestConfigBuilder {
    this.requestConfig.url = url;
    return this;
  }

  public setMethod(method: FetchMethod): FetchRequestConfigBuilder {
    this.requestConfig.method = method;
    return this;
  }

  public setIsAuthRequired(isAuthRequired: boolean): FetchRequestConfigBuilder {
    this.requestConfig.isAuthRequired = isAuthRequired;
    return this;
  }

  public build(): FetchRequestConfig {
    return this.requestConfig;
  }

  private requestConfig: FetchRequestConfig = new FetchRequestConfig();
}
