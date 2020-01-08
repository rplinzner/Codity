import { BaseResponsePagination } from './base-response-pagination';
import { Comment } from './comment';

export interface CommentsResponse extends BaseResponsePagination {
  models: Comment[];
}
