import { Post } from './post';
import { BaseResponsePagination } from './base-response-pagination';

export interface PostsResponse extends BaseResponsePagination {
  models: Post[];
}
