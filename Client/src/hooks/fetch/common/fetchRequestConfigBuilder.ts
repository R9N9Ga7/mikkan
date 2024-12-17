import { FetchRequestConfig } from './fetchRequestConfig';
import { UseFetchMethod } from './useFetchMethod';

export class FetchRequestConfigBuilder {
  public SetUrl(url : string): FetchRequestConfigBuilder {
    this.requestConfig.url = url;
    return this;
  }

  public SetMethod(method: UseFetchMethod): FetchRequestConfigBuilder {
    this.requestConfig.method = method;
    return this;
  }

  public Build(): FetchRequestConfig {
    return this.requestConfig;
  }

  requestConfig: FetchRequestConfig = new FetchRequestConfig();
}
