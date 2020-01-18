import { BaseResponse } from './base-response';
import { Comment } from './comment';

export interface CommentResponse extends BaseResponse {
  model: Comment;
}
