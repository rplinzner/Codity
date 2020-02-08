import { ThunkAction } from 'redux-thunk';
import { Action } from 'redux';

import * as types from './notifications.types';
import { Notification } from '../../types/notification';
import { NotificationResponse } from '../../types/notification-response';
import { AppState } from '../..';
import displayErrors from '../../helpers/display-errors';
import get from '../../services/get.service';
import { notificationController } from '../../constants/global.constats';
import { toast } from 'react-toastify';

export function init(): types.NotificationsActionTypes {
  return {
    type: 'NOTIFICATIONS_INIT',
  };
}

export function read(isRead: boolean): types.NotificationsActionTypes {
  return {
    type: 'NOTIFICATIONS_READ',
    payload: isRead,
  };
}

export function add(
  notification: Notification,
): types.NotificationsActionTypes {
  return {
    type: 'NOTIFICATIONS_ADD',
    payload: notification,
  };
}

// Thunk actions
export function addById(
  id: number,
): ThunkAction<void, AppState, null, Action<any>> {
  return (dispatch, getState) => {
    const lang = getState().localize.languages.find(f => f.active === true);
    const langCode = lang ? lang.code : 'en';
    const langIndex = lang ? getState().localize.languages.indexOf(lang) : 0;
    const errorMessage = getState().localize.translations['errorConnection'][
      langIndex
    ];
    get<NotificationResponse>(
      notificationController,
      `/${id}`,
      langCode,
      errorMessage,
      true,
    ).then(
      notification => {
        toast(notification.model.label, { position: 'bottom-right' });
        dispatch(add(notification.model));
      },
      error => displayErrors(error),
    );
  };
}
