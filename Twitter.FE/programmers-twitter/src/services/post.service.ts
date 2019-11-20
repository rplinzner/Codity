import authHeader from '../helpers/auth-header';
import handleResponse from '../helpers/handle-response';
import { ServerResponse } from '../types/response';

export default function post<T>(
  dataToPost: T,
  controller: string,
  endpoint: string,
  language: string,
  connectionErrorMessage: string,
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
    method: 'POST',
    headers: requestHeaders,
    body: JSON.stringify(dataToPost),
  };
  return fetch(`${controller}${endpoint}`, requestOptions)
    .catch(() => Promise.reject([{ message: connectionErrorMessage }]))
    .then(handleResponse)
    .then(
      (response: ServerResponse) => {
        return response;
      },
      (error: Error[]) => Promise.reject(error),
    );
}
