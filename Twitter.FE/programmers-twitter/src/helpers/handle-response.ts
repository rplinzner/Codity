import { userService } from '../services/authentication.service';
import { BaseResponse } from '../types/base-response';

export default function handleResponse<T extends BaseResponse>(response: Response) {
  return response.text().then(text => {
    const data = (text && JSON.parse(text)) as T;

    if (!response.ok) {
      if (response.status === 401) {
        // auto logout if 401 response returned from api
        userService.logout();
      }

      const error =
        (data && data.isError === true && data.errors) || response.statusText;

      return Promise.reject(error);
    }
    if (data.isError) {
      return Promise.reject(data.errors);
    }
    return data;
  });
}
