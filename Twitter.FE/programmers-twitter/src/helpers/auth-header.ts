import User from '../types/user';

export default function authHeader(): { name?: string; value?: string } {
  // return authorization header with jwt token
  const localUser = localStorage.getItem('user');
  const user: User =
    localUser === null
      ? null
      : JSON.parse(localStorage.getItem('user') || '{}');

  if (user && user.token) {
    return { name: 'Authorization', value: `Bearer ${user.token}` };
  } else {
    return {};
  }
}
