import * as types from './settings.types';
import { SettingsState } from './settings.types';

export function setDarkTheme(isDark: boolean): types.SettingsActionTypes {
  return {
    type: 'SETTINGS_SET_DARKTHEME',
    payload: isDark,
  };
}
export function setAllSettings(
  settings: SettingsState,
): types.SettingsActionTypes {
  return {
    type: 'SETTINGS_SET_ALL_SETTINGS',
    payload: settings,
  };
}
