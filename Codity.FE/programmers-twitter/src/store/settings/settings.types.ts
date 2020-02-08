export interface SettingsState {
  isDarkTheme: boolean;
  hasGithubToken: boolean;
}

export const SET_DARKTHEME = 'SETTINGS_SET_DARKTHEME';
export const SET_GITHUB_TOKEN = 'SETTINGS_SET_GITHUB_TOKEN';
export const SET_ALL_SETTINGS = 'SETTINGS_SET_ALL_SETTINGS';

interface SetDarkThemeAction {
  type: typeof SET_DARKTHEME;
  payload: boolean;
}

interface SetHasGithubToken {
  type: typeof SET_GITHUB_TOKEN;
  payload: boolean;
}

interface SetAllSettingsAction {
  type: typeof SET_ALL_SETTINGS;
  payload: SettingsState;
}

export type SettingsActionTypes =
  | SetDarkThemeAction
  | SetAllSettingsAction
  | SetHasGithubToken;
