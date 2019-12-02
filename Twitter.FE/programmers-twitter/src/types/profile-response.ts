import { BaseResponse } from './base-response';

export interface ProfileResponse extends BaseResponse {
  model: Model;
}
export interface Model {
  aboutMe: string;
  birthDay: string;
  genderName: string;
  followingCount: number;
  latestTweets: LatestTweet[];
  id: number;
  firstName: string;
  lastName: string;
  image: string;
  isFollowing: boolean;
  followersCount: number;
}

export interface LatestTweet {
  id: number;
  authorFirstName: string;
  authorLastName: string;
  creationDate: string;
  text: string;
  codeSnippetId: number;
  codeSnippet: CodeSnippet;
  likesCount: number;
  commentsCount: number;
}

export interface CodeSnippet {
  text: string;
  programmingLanguageId: number;
  programmingLanguageName: string;
}
