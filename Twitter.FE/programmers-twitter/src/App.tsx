import React from 'react';
import PrimarySearchAppBar from './components/layout/navbar';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import {
  MuiThemeProvider,
  createMuiTheme,
  CssBaseline,
} from '@material-ui/core';
import themeDark from './themes/dark-theme';
import { ThemeOptions } from '@material-ui/core/styles/createMuiTheme';
import { LocalizeProvider } from 'react-localize-redux';

import { Login } from './components/containers/Authentication/index';
import { HomePage } from './components/containers/HomePage/index';

const darkTheme = createMuiTheme(themeDark as ThemeOptions);

// tslint:disable-next-line: typedef
const App: React.FC = () => {
  // tslint:disable-next-line: no-console
  console.log('This app is in:', process.env.NODE_ENV, 'mode');
  return (
    <MuiThemeProvider theme={darkTheme}>
      <LocalizeProvider>
        <CssBaseline />
        <BrowserRouter>
          <PrimarySearchAppBar />
          <Switch>
            <Route path="/" exact component={HomePage} />
            <Route path="/Login" component={Login} />
          </Switch>
        </BrowserRouter>
      </LocalizeProvider>
    </MuiThemeProvider>
  );
};

export default App;
