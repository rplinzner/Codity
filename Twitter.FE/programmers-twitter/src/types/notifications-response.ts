import { BaseResponsePagination } from './base-response-pagination';
import { Notification } from './notification';

export interface NotificationsResponse extends BaseResponsePagination {
  models: Notification[];
}
