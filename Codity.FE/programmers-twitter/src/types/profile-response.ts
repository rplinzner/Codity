import { BaseResponse } from './base-response';
import { Post } from './post';

export interface ProfileResponse extends BaseResponse {
  model: Model;
}
export interface Model {
  aboutMe: string;
  birthDay: string;
  genderName: string;
  followingCount: number;
  latestPosts: Post[];
  id: number;
  firstName: string;
  lastName: string;
  image: string;
  isFollowing: boolean;
  followersCount: number;
}
