import authHeader from '../helpers/auth-header';
import handleResponse from '../helpers/handle-response';
import { BaseResponse } from '../types/base-response';

export default function get<T extends BaseResponse>(
  controller: string,
  endpoint: string,
  language: string,
  connectionErrorMessage: string,
  isAuthorizationNeeded: boolean = false,
) {
  const requestHeaders: HeadersInit = new Headers();
  if (isAuthorizationNeeded) {
    const header = authHeader();
    if (header.name && header.value) {
      requestHeaders.append(header.name, header.value);
    }
  }
  requestHeaders.append('Accept-Language', language);
  const requestOptions: RequestInit = {
    method: 'GET',
    headers: requestHeaders,
  };
  return fetch(`${controller}${endpoint}`, requestOptions)
    .catch(() => Promise.reject([{ message: connectionErrorMessage }]))
    .then((response) => handleResponse<T>(response))
    .then(
      (response: T) => {
        return response;
      },
      (error: Error[]) => Promise.reject(error),
    );
}
