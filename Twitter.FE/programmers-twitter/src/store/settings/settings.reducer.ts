import * as types from './settings.types';

const initialState: types.SettingsState = { isDarkTheme: false };

export default function settingsReducer(
  state = initialState,
  action: types.SettingsActionTypes,
): types.SettingsState {
  switch (action.type) {
    case 'SETTINGS_SET_DARKTHEME':
      return {
        ...state,
        isDarkTheme: action.payload,
      };
    default:
      return state;
  }
}
