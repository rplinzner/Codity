import authHeader from '../helpers/auth-header';
import handleResponse from '../helpers/handle-response';
import { Error, BaseResponse } from '../types/base-response';

export default function post<T extends BaseResponse, D>(
  dataToPost: D,
  controller: string,
  endpoint: string,
  language: string,
  connectionErrorMessage: string | any,
  isAuthorizationNeeded: boolean = false,
) {
  const requestHeaders: HeadersInit = new Headers();
  requestHeaders.set('Content-Type', 'application/json');
  if (isAuthorizationNeeded) {
    const header = authHeader();
    if (header.name && header.value) {
      requestHeaders.append(header.name, header.value);
    }
  }
  requestHeaders.append('Accept-Language', language);
  const requestOptions: RequestInit = {
    method: 'PUT',
    headers: requestHeaders,
    body: JSON.stringify(dataToPost),
  };
  return fetch(`${controller}${endpoint}`, requestOptions)
    .catch(() => Promise.reject([{ message: connectionErrorMessage }]))
    .then(response => handleResponse<T>(response))
    .then(
      (response: T) => {
        return response;
      },
      (error: Error[]) => Promise.reject(error),
    );
}
