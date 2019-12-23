import React, { Component } from 'react';
import PrimarySearchAppBar from './components/layout/navbar';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import {
  MuiThemeProvider,
  createMuiTheme,
  CssBaseline,
  responsiveFontSizes,
} from '@material-ui/core';
import { ThemeOptions } from '@material-ui/core/styles/createMuiTheme';
import { ToastContainer } from 'react-toastify';
import { renderToStaticMarkup } from 'react-dom/server';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';
import { globalTranslations } from './translations/index';

import 'react-toastify/dist/ReactToastify.css';

import * as authentication from './components/containers/authentication/index';
import * as additional from './components/containers/additional/index';
import * as main from './components/containers/feed/index';
import * as user from './components/containers/profile/index';
import SearchResultCard from './components/layout/search-result-card';
import { settingsController } from './constants/global.constats';
import { PrivateRoute } from './components/containers/authentication/index';
import { connect } from 'react-redux';
import { AppState } from '.';
import get from './services/get.service';
import SettingsResponse from './types/settings-response';
import { setDarkTheme } from './store/settings/settings.actions';

import themeDark from './themes/dark-theme';
import themeLight from './themes/light-theme';
import displayErrors from './helpers/display-errors';

const darkTheme = responsiveFontSizes(
  createMuiTheme(themeDark as ThemeOptions),
);
const lightTheme = responsiveFontSizes(
  createMuiTheme(themeLight as ThemeOptions),
);

interface Props extends LocalizeContextProps {
  browserLanguage: string;
  isDarkTheme: boolean;
  isLoggedIn: boolean;
  setDarkThemeAction: typeof setDarkTheme;
}

// tslint:disable-next-line: typedef
class App extends Component<Props> {
  constructor(props: Props) {
    super(props);
    this.props.initialize({
      languages: [
        { name: 'English', code: 'en' },
        { name: 'Polski', code: 'pl' },
      ],
      translation: globalTranslations,
      options: {
        renderToStaticMarkup,
        renderInnerHtml: true,
        defaultLanguage: props.browserLanguage,
      },
    });
  }

  // shouldComponentUpdate(nextProps: Props, nextState: any) {
  //   if (nextProps.isDarkTheme === this.props.isDarkTheme) {
  //     return false;
  //   }
  //   return true;
  // }

  downloadSettings = () => {
    get<SettingsResponse>(
      settingsController,
      '',
      this.props.browserLanguage,
      <T id="errorConnection" />,
      true,
    ).then(
      response => {
        this.props.setActiveLanguage(response.model.languageCode);
        this.props.setDarkThemeAction(response.model.isDarkTheme);
      },
      error => displayErrors(error),
    );
  };
  componentDidMount() {
    if (this.props.isLoggedIn) {
      this.downloadSettings();
    }
  }

  render() {
    // tslint:disable-next-line: no-console
    console.log('This app is in:', process.env.NODE_ENV, 'mode');
    return (
      <MuiThemeProvider theme={this.props.isDarkTheme ? darkTheme : lightTheme}>
        <CssBaseline />
        <ToastContainer />
        <BrowserRouter>
          <PrimarySearchAppBar>
            <Switch>
              {/* Routes */}
              <Route path="/" exact={true} component={authentication.Login} />
              <Route path="/Register" component={authentication.Register} />
              <Route path="/Verify/:type" component={additional.Verify} />
              <Route path="/test" component={SearchResultCard} />
              {/* Private Routes */}
              <PrivateRoute path="/MyFeed" component={main.Feed} />
              <PrivateRoute path="/Profile" component={user.Profile} />
              <PrivateRoute
                path="/SearchResults"
                component={additional.SearchResults}
              />

              <Route component={additional.NotFound} />
            </Switch>
          </PrimarySearchAppBar>
        </BrowserRouter>
      </MuiThemeProvider>
    );
  }
}

const mapStateToProps = (state: AppState) => ({
  isDarkTheme: state.settings.isDarkTheme,
  isLoggedIn: state.user.loggedIn,
});

const mapDispatchToProps = (dispatch: any) => {
  return {
    setDarkThemeAction: (isDark: boolean) => dispatch(setDarkTheme(isDark)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(withLocalize(App));
