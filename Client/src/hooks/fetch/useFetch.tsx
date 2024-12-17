import { useState } from 'react';
import { FetchRequestConfig } from './common/fetchRequestConfig';

export interface FetchParams<TResult> {
  onSuccess?: (data: TResult | null) => void;
  onError?: (error: string) => void;
}

export interface UseFetchParams<TResult> extends FetchParams<TResult> {
  fetchRequestConfig: FetchRequestConfig,
};

export type UseFetchResult<TBody, TResult> = {
  data?: TResult | null,
  isLoading: boolean,
  error: string | null,
  fetchData: (body: TBody) => void,
};

function useFetch<TBody, TResult>(
  {
    fetchRequestConfig,
    onSuccess,
    onError,
  }: UseFetchParams<TResult>,
): UseFetchResult<TBody, TResult> {
  const [data, setData] = useState<TResult | null>();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchData = async (body: TBody) => {
    const request: RequestInit = {
      method: fetchRequestConfig.method,
      headers: {
        'Content-Type': 'application/json',
      },
    };

    if (body) {
      request.body = JSON.stringify(body);
    }

    try {
      setIsLoading(true);
      setError(null);

      const response = await fetch(fetchRequestConfig.url, request);

      if (response.ok) {
        let responseData: TResult | null = null;
        try {
          responseData = await response.json();
        } catch {
          // TODO: Fix later (when will be more consistent response from server)
        }

        setData(responseData);

        if (onSuccess) {
          onSuccess(responseData);
        }
      } else {
        const errorMessage = await response.text();
        setError(errorMessage);
        if (onError) {
          onError(errorMessage);
        }
      }
    } catch (error) {
      console.error(error); // TODO
    }

    setIsLoading(false);
  };

  return {
    data,
    isLoading,
    error,
    fetchData,
  };
}

export default useFetch;
