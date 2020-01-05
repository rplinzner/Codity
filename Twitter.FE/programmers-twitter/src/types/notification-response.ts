import { BaseResponse } from './base-response';
import { Notification } from './notification';

export interface NotificationResponse extends BaseResponse {
  model: Notification;
}
