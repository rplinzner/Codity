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
import { withLocalize, LocalizeContextProps } from 'react-localize-redux';
import { globalTranslations } from './translations/index';

import 'react-toastify/dist/ReactToastify.css';

import * as authentication from './components/containers/authentication/index';
import * as additional from './components/containers/additional/index';
import * as main from './components/containers/feed/index';

// import themeDark from './themes/dark-theme';
import themeLight from './themes/light-theme';
import { PrivateRoute } from './components/containers/authentication/index';

// const darkTheme = responsiveFontSizes(createMuiTheme(themeDark as ThemeOptions));
const lightTheme = responsiveFontSizes(
  createMuiTheme(themeLight as ThemeOptions),
);

interface Props extends LocalizeContextProps {
  browserLanguage: string;
}

// tslint:disable-next-line: typedef
class App extends Component<Props> {
  constructor(props: Props) {
    super(props);
    this.props.initialize({
      languages: [
        { name: 'English', code: 'en' },
        { name: 'Polish', code: 'pl' },
      ],
      translation: globalTranslations,
      options: {
        renderToStaticMarkup,
        renderInnerHtml: true,
        defaultLanguage: props.browserLanguage,
      },
    });
  }
  render() {
    // tslint:disable-next-line: no-console
    console.log('This app is in:', process.env.NODE_ENV, 'mode');
    return (
      <MuiThemeProvider theme={lightTheme}>
        <CssBaseline />
        <ToastContainer />
        <BrowserRouter>
          <PrimarySearchAppBar>
            <Switch>
              {/* Routes */}
              <Route path="/" exact={true} component={authentication.Login} />              
              <Route path="/Register" component={authentication.Register} />
              <Route path="/Verify/:type" component={additional.Verify} />
              {/* Private Routes */}
              <PrivateRoute path="/MyFeed" component={main.Feed} />
              
              <Route component={additional.NotFound} />
            </Switch>
          </PrimarySearchAppBar>
        </BrowserRouter>
      </MuiThemeProvider>
    );
  }
}

export default withLocalize(App);
