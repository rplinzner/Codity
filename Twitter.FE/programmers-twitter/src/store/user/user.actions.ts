import { ThunkAction } from 'redux-thunk';
import { Action } from 'redux';

import * as types from './user.types';
import User from '../../types/user';
import { userService } from '../../services/authentication.service';
import { AppState } from '../..';
import displayErrors from '../../helpers/display-errors';

export function login(
  email: string,
  password: string
): ThunkAction<void, AppState, null, Action<any>> {
  return dispatch => {
    dispatch(request({ id: 0, token: email }));

    userService.login(email, password).then(
      user => {
        dispatch(success(user as User));
      },
      (error: Error[]) => {
        dispatch(failure());
        displayErrors(error);
      }
    );
  };

  function request(user: User): types.UserActionTypes {
    return {
      type: 'USERS_LOGIN_REQUEST',
      payload: user,
    };
  }
  function success(user: User): types.UserActionTypes {
    return {
      type: 'USERS_LOGIN_SUCCESS',
      payload: user,
    };
  }
  function failure(): types.UserActionTypes {
    return {
      type: 'USERS_LOGIN_FAILURE',
    };
  }
}

export function logout(): types.UserActionTypes {
  userService.logout();
  return {
    type: 'USERS_LOGOUT',
  };
}
