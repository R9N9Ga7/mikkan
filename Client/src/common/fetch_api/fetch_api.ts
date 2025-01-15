import { Account } from '../interfaces/account';
import { FetchRequestConfig } from './fetch_request_config';

type FetchApiSuccessEventCb<TResponse> = (data: TResponse | null) => void;
type FetchApiErrorEventCb = (error: string) => void;
type FetchApiFinishEventCb = () => void;
type FetchApiRequestBodyType<TBody> = TBody | null;

export class FetchApi<TBody, TResponse> {
  public constructor(
    fetchRequestConfig: FetchRequestConfig,
    account: Account | null,
    body: FetchApiRequestBodyType<TBody>,
  ) {
    this.fetchRequestConfig = fetchRequestConfig;
    this.account = account;
    this.body = body;
  }

  public async sendRequest(): Promise<void> {
    this.responseData = null;

    const request = this.createRequest();
    try {
      const response = await fetch(this.fetchRequestConfig.url, request);
      await this.parseResponse(response);
    } catch (error) {
      this.onError(`${error}`); // TODO
    }
    this.onFinish();
  }

  public getResponse(): TResponse | null {
    return this.responseData;
  }

  public addEventListenerOnSuccess(cb: FetchApiSuccessEventCb<TResponse>): void {
    this.successCbs.push(cb);
  }

  public addEventListenerOnError(cb: FetchApiErrorEventCb): void {
    this.errorCbs.push(cb);
  }

  public addEventListenerOnFinish(cb: FetchApiFinishEventCb): void {
    this.finishCbs.push(cb);
  }

  private createRequest(): RequestInit {
    const request: RequestInit = {
      method: this.fetchRequestConfig.method,
      headers: this.createHeaders(),
    };
    if (this.body) {
      request.body = JSON.stringify(this.body);
    }
    return request;
  }

  private createHeaders(): HeadersInit {
    const headers: HeadersInit = {
      'Content-Type': 'application/json',
    };

    if (this.fetchRequestConfig.isAuthRequired) {
      headers['Authorization'] = `Bearer ${this.account?.accessToken}`;
    }

    return headers;
  };

  private onSuccess(response: TResponse | null): void {
    this.successCbs.forEach(cb => {
      cb(response);
    });
  }

  private onError(error: string): void {
    this.errorCbs.forEach(cb => {
      cb(error);
    });
  }

  private onFinish(): void {
    this.finishCbs.forEach((cb) => {
      cb();
    });
  }

  private async parseResponse(response: Response): Promise<void> {
    if (response.ok) {
      try {
        this.responseData = await response.json();
      } catch {
        // TODO: Fix later (when will be more consistent response from server)
      }

      this.onSuccess(this.responseData);
    } else {
      const errorMessage = await response.text();
      this.onError(errorMessage);
    }
  }

  private fetchRequestConfig: FetchRequestConfig;
  private account: Account | null;
  private body: FetchApiRequestBodyType<TBody>;

  private responseData: TResponse | null = null;

  private successCbs: FetchApiSuccessEventCb<TResponse>[] = [];
  private errorCbs: FetchApiErrorEventCb[] = [];
  private finishCbs: FetchApiFinishEventCb[] = [];
}
