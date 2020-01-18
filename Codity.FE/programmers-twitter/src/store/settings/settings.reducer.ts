import * as types from './settings.types';

const initialState: types.SettingsState = {
  isDarkTheme: false,
  hasGithubToken: false,
};

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
    case 'SETTINGS_SET_GITHUB_TOKEN':
      return {
        ...state,
        hasGithubToken: action.payload,
      };
    case 'SETTINGS_SET_ALL_SETTINGS':
      return {
        isDarkTheme: action.payload.isDarkTheme,
        hasGithubToken: action.payload.hasGithubToken,
      };
    default:
      return state;
  }
}
