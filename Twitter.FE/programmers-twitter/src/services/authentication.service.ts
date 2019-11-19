import * as constants from '../constants/global.constats';
import handleResponse from '../helpers/handle-response';
import { ServerResponse, Error } from '../types/response';

export const userService = {
  login,
  logout,
};

function login(email: string, password: string) {
  const requestOptions: RequestInit = {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password }),
  };

  return fetch(`${constants.server}/api/authentication/login`, requestOptions)
    .catch(() =>
      Promise.reject([
        { message: 'Error ocurred while communicating with server' },
      ]),
    )
    .then(handleResponse)
    .then(
      (user: ServerResponse) => {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem('user', JSON.stringify(user.model));
        return user.model;
      },
      (error: Error[]) => Promise.reject(error),
    );
}

function logout() {
  // remove user from local storage to log user out
  localStorage.removeItem('user');
  // eslint-disable-next-line no-restricted-globals
  location.reload(true);
}
