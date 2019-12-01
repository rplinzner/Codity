import React from 'react';
import { FormGroup, FormControlLabel, Switch } from '@material-ui/core';
import {
  Translate as T,
  withLocalize,
  LocalizeContextProps,
} from 'react-localize-redux';
import { AppState } from '../..';
import { setDarkTheme } from '../../store/settings/settings.actions';
import { connect } from 'react-redux';

interface Props extends LocalizeContextProps {
  isDarkTheme: boolean;
  setDarkThemeAction: typeof setDarkTheme;
}

const ThemeSwitch: React.FC<Props> = (props: Props) => {
  return (
    <FormGroup row>
      <FormControlLabel
        control={
          <Switch
            checked={props.isDarkTheme}
            onChange={() => props.setDarkThemeAction(!props.isDarkTheme)}
            color="secondary"
            value="isDarkTheme"
          />
        }
        label={<T id="darkTheme" />}
      />
    </FormGroup>
  );
};

const mapStateToProps = (state: AppState) => ({
  isDarkTheme: state.settings.isDarkTheme,
});

const mapDispatchToProps = (dispatch: any) => {
  return {
    setDarkThemeAction: (isDarkTheme: boolean) =>
      dispatch(setDarkTheme(isDarkTheme)),
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(withLocalize(ThemeSwitch));
