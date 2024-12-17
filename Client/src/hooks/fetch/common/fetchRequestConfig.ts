import { UseFetchMethod } from './useFetchMethod';

export class FetchRequestConfig {
  public url: string = import.meta.env.VITE_BASE_URL;
  public method: UseFetchMethod = UseFetchMethod.GET;
}
