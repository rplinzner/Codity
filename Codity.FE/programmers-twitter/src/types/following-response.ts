import { BaseResponsePagination } from './base-response-pagination';

export interface FollowingResponse extends BaseResponsePagination {
  models: Model[];
}

export interface Model {
  id: number;
  firstName: string;
  lastName: string;
  image: string;
  isFollowing: boolean;
  followersCount: number;
}
