import * as types from './notifications.types';

const initialState: types.NotificationState = {
  isConnectionOpen: false,
  isNewNotification: false,
  notifications: [],
};

export default function notificationsReducer(
  state = initialState,
  action: types.NotificationsActionTypes,
): types.NotificationState {
  switch (action.type) {
    case 'NOTIFICATIONS_ADD':
      return {
        ...state,
        isNewNotification: true,
        notifications: [...state.notifications, action.payload],
      };
    case 'NOTIFICATIONS_INIT':
      return {
        ...initialState,
        isConnectionOpen: true,
      };
    case 'NOTIFICATIONS_READ':
      return {
        ...state,
        isNewNotification: !action.payload,
      };

    default:
      return state;
  }
}
