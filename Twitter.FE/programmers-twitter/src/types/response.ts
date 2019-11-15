import User from './user';

type ResponseModel = User;

export interface ServerResponse {
  model: ResponseModel | null;
  message: string | null;
  isError: boolean;
  errors: Error[];
}

export interface Error {
  message: string;
}