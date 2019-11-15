import * as constants from '../constants/global.constats';
import handleResponse from '../helpers/handle-response';
import { ServerResponse } from '../types/response';

export const userService = {
  login,
  logout,
};

function login(email: string, password: string) {
  const requestOptions = {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password }),
  };

  return fetch(`${constants.server}/api/authentication/login`, requestOptions)
    .then(handleResponse)
    .then((user: ServerResponse) => {
      // store user details and jwt token in local storage to keep user logged in between page refreshes
      if (!user.isError) {
        localStorage.setItem('user', JSON.stringify(user.model));
        return user.model;
      }
      return Promise.reject(user.errors);
    })
    .catch( error => Promise.reject(['Error ocurred while communicating with server']));
}

function logout() {
  // remove user from local storage to log user out
  localStorage.removeItem('user');
}
