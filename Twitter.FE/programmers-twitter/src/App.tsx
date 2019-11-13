import React, { Component } from 'react';
import PrimarySearchAppBar from './components/layout/navbar';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import {
  MuiThemeProvider,
  createMuiTheme,
  CssBaseline,
} from '@material-ui/core';
import { ThemeOptions } from '@material-ui/core/styles/createMuiTheme';
import { ToastContainer } from 'react-toastify';
import { renderToStaticMarkup } from 'react-dom/server';

import 'react-toastify/dist/ReactToastify.css';

import { Login } from './components/containers/authentication/index';
import { HomePage } from './components/containers/HomePage/index';
//import themeDark from './themes/dark-theme';
import themeLight from './themes/light-theme';
import { withLocalize, LocalizeContextProps } from 'react-localize-redux';
import globalTranslations from './translations/global.json';

//const darkTheme = createMuiTheme(themeDark as ThemeOptions);
const lightTheme = createMuiTheme(themeLight as ThemeOptions);

// tslint:disable-next-line: typedef
class App extends Component<LocalizeContextProps> {
  constructor(props: LocalizeContextProps) {
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
          <PrimarySearchAppBar />
          <Switch>
            <Route path="/" exact={true} component={HomePage} />
            <Route path="/Login" component={Login} />
          </Switch>
        </BrowserRouter>
      </MuiThemeProvider>
    );
  }
}

export default withLocalize(App);
