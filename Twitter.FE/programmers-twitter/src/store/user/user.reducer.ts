import * as types from './user.types';
import User from '../../types/user';
const localUser = localStorage.getItem('user');
const user: User = localUser === null ? null : JSON.parse(localStorage.getItem('user') || '{}');

const initialState: types.UserState = user
  ? { loggedIn: true, loggingIn: false, user }
  : { loggedIn: false, loggingIn: false, user: null };

export default function userReducer(
  state = initialState,
  action: types.UserActionTypes
): types.UserState {
  switch (action.type) {
    case 'USERS_LOGIN_REQUEST':
      return {
        ...state,
        loggingIn: true,
        user: action.payload,
      };
    case 'USERS_LOGIN_SUCCESS':
      return {
        ...state,
        loggedIn: true,
        user: action.payload,
        loggingIn: false,
      };
    case 'USERS_LOGIN_FAILURE':
    case 'USERS_LOGOUT':
      return {
        user: null,
        loggedIn: false,
        loggingIn: false,
      };
    default:
      return state;
  }
}
