import { Notification } from '../../types/notification';

export interface NotificationState {
  isConnectionOpen: boolean;
  isNewNotification: boolean;
  notifications: Notification[];
}

export const INIT = 'NOTIFICATIONS_INIT';
export const READ = 'NOTIFICATIONS_READ';
export const ADD = 'NOTIFICATIONS_ADD';

interface InitAction {
  type: typeof INIT;
}

interface ReadAction {
  type: typeof READ;
  payload: boolean;
}

interface AddAction {
  type: typeof ADD;
  payload: Notification;
}

export type NotificationsActionTypes = InitAction | ReadAction | AddAction;
