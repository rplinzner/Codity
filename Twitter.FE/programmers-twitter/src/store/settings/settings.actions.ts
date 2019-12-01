import * as types from './settings.types';

export function setDarkTheme(isDark: boolean): types.SettingsActionTypes {
  return {
    type: 'SETTINGS_SET_DARKTHEME',
    payload: isDark,
  };
}
