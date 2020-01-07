export interface Post {
  id: number;
  authorFirstName: string;
  authorLastName: string;
  authorImage: string;
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
  gistURL: string;
}
