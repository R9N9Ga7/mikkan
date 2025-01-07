import { BASE_URL } from '../consts/server_urls';
import { FetchMethod } from './fetch_method';

export class FetchRequestConfig {
  public url: string = BASE_URL;
  public method: FetchMethod = FetchMethod.GET;
  public isAuthRequired: boolean = true;
}
