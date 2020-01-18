import { BaseResponsePagination } from './base-response-pagination';

export default interface SearchResponse extends BaseResponsePagination {
  models: SearchModel[];
}

export interface SearchModel {
  id: number;
  firstName: string;
  lastName: string;
  image: string | null;
  followersCount: number;
  isFollowing: boolean;
}
