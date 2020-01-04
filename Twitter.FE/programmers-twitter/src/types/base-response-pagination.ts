import { BaseResponse } from './base-response';

export interface BaseResponsePagination extends BaseResponse {
  pageSize: number;
  currentPage: number;
  totalPages: number;
  totalCount: number;
}
