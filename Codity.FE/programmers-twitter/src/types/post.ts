export interface Post {
  id: number;
  authorId: number;
  authorFirstName: string;
  authorLastName: string;
  authorImage: string;
  creationDate: string;
  text: string;
  codeSnippetId: number;
  codeSnippet: CodeSnippet;
  isLiked: boolean;
  likesCount: number;
  commentsCount: number;
}

export interface CodeSnippet {
  text: string;
  programmingLanguageId: number;
  programmingLanguageName: string;
  gistURL: string;
  programmingLanguageCode: string;
}
