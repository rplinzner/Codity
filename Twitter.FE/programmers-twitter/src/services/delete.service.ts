import authHeader from '../helpers/auth-header';
import handleResponse from '../helpers/handle-response';
import { Error, BaseResponse } from '../types/base-response';

export default function deleteRequest<D>(
  body: D,
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
    method: 'DELETE',
    headers: requestHeaders,
    body: JSON.stringify(body),
  };
  return fetch(`${controller}${endpoint}`, requestOptions)
    .catch(() => Promise.reject([{ message: connectionErrorMessage }]))
    .then(response => handleResponse<BaseResponse>(response))
    .then(
      (response: BaseResponse) => {
        return response;
      },
      (error: Error[]) => Promise.reject(error),
    );
}
