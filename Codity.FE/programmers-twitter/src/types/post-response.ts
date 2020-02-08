import { Post } from './post';
import { BaseResponse } from './base-response';

export interface PostResponse extends BaseResponse {
  model: Post;
}
