import User from '../../types/user';
export interface UserState {
  loggedIn: boolean;
  loggingIn: boolean;
  user: User | null;
}

export const LOGIN = 'USERS_LOGIN';
export const LOGOUT = 'USERS_LOGOUT';

export const LOGIN_REQUEST = 'USERS_LOGIN_REQUEST';
export const LOGIN_SUCCESS = 'USERS_LOGIN_SUCCESS';
export const LOGIN_FAILURE = 'USERS_LOGIN_FAILURE';

interface LogoutAction {
  type: typeof LOGOUT;
}

interface LoginRequestAction {
  type: typeof LOGIN_REQUEST;
  payload: User;
}

interface LoginSuccessAction {
  type: typeof LOGIN_SUCCESS;
  payload: User;
}

interface LoginFailureAction {
  type: typeof LOGIN_FAILURE;
  payload: string[];
}

export type UserActionTypes =
  | LoginRequestAction
  | LoginSuccessAction
  | LoginFailureAction
  | LogoutAction;
