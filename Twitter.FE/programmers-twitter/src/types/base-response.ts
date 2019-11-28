
export interface BaseResponse {
  message: string | null;
  isError: boolean;
  errors: Error[];
}

export interface Error {
  message: string;
}
