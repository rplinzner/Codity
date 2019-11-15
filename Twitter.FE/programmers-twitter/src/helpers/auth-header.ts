import User from '../types/user';

export function authHeader() {
  // return authorization header with jwt token
  const localUser = localStorage.getItem('user');
  const user: User =
    localUser === null
      ? null
      : JSON.parse(localStorage.getItem('user') || '{}');

  if (user && user.token) {
    return { Authorization: 'Bearer ' + user.token };
  } else {
    return {};
  }
}
