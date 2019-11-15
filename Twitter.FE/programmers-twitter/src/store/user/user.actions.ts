import { ThunkAction } from 'redux-thunk';
import { Action } from 'redux';

import * as types from './user.types';
import User from '../../types/user';
import { userService } from '../../services/authentication.service';
import { toast } from 'react-toastify';
import { AppState } from '../..';

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
      (error: string[]) => {
        console.log(error);

        dispatch(failure(error));
        error.forEach((element: string) => {
          toast.error(element);
        });
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
  function failure(error: string[]): types.UserActionTypes {
    return {
      type: 'USERS_LOGIN_FAILURE',
      payload: error,
    };
  }
}

export function logout(): types.UserActionTypes {
  userService.logout();
  return {
    type: 'USERS_LOGOUT',
  };
}
