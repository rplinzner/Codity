import User from './user';
import { BaseResponse } from './base-response';

type ResponseModel = User;

export interface AuthResponse extends BaseResponse {
  model: ResponseModel | null;  
}
