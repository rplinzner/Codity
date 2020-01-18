import { BaseResponse } from './base-response';

export default interface GenderResponse extends BaseResponse {
  models: Model[];
}

export interface Model {
  genderId: number;
  genderName: string;
}
