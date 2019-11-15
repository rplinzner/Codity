import { userService } from '../services/authentication.service';
import { ServerResponse } from '../types/response';

export default function handleResponse(response: Response) {
  return response.text().then(text => {
    const data = (text && JSON.parse(text)) as ServerResponse;

    if (!response.ok) {
      if (response.status === 401) {
        // auto logout if 401 response returned from api
        userService.logout();
        // eslint-disable-next-line no-restricted-globals
        location.reload(true);
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
