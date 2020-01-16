import { BaseResponse } from './base-response';

export interface ProgrammingLanguageResponse extends BaseResponse {
  models: Programminglanguage[];
}

export interface Programminglanguage {
  id: number;
  name: string;
  code: string;
}
