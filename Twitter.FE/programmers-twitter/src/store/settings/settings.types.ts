export interface SettingsState {
  isDarkTheme: boolean;
}

export const SET_DARKTHEME = 'SETTINGS_SET_DARKTHEME';

interface SetDarkThemeAction {
  type: typeof SET_DARKTHEME;
  payload: boolean;
}

export type SettingsActionTypes = SetDarkThemeAction;
