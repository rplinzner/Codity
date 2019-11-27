import { BaseResponse } from './base-response';

export default interface SearchResponse extends BaseResponse {
  pageSize: number;
  currentPage: number;
  totalPages: number;
  totalCount: number;
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
