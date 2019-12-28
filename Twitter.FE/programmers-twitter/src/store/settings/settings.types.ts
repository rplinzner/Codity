export interface SettingsState {
  isDarkTheme: boolean;
  hasGithubToken: boolean;
}

export const SET_DARKTHEME = 'SETTINGS_SET_DARKTHEME';
export const SET_ALL_SETTINGS = 'SETTINGS_SET_ALL_SETTINGS';

interface SetDarkThemeAction {
  type: typeof SET_DARKTHEME;
  payload: boolean;
}

interface SetAllSettingsAction {
  type: typeof SET_ALL_SETTINGS;
  payload: SettingsState;
}

export type SettingsActionTypes = SetDarkThemeAction | SetAllSettingsAction;
