import { BaseResponse } from './base-response';
export default interface SettingsResponse extends BaseResponse {
  model: {
    isDarkTheme: boolean;
    languageCode: string;
    hasGithubToken: boolean;
  };
}
