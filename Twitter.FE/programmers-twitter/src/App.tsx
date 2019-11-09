import React from 'react';
import PrimarySearchAppBar from './components/layout/navbar';
import { BrowserRouter, Switch } from 'react-router-dom';
import {
  MuiThemeProvider,
  createMuiTheme,
  CssBaseline,
} from '@material-ui/core';
import themeDark from './themes/dark-theme';
import { ThemeOptions } from '@material-ui/core/styles/createMuiTheme';

import { Login } from './components/containers/Authentication/index';

const darkTheme = createMuiTheme(themeDark as ThemeOptions);

// tslint:disable-next-line: typedef
const App: React.FC = () => {
  // tslint:disable-next-line: no-console
  console.log('This app is in:', process.env.NODE_ENV, 'mode');
  return (
    <MuiThemeProvider theme={darkTheme}>
      <CssBaseline />
      <BrowserRouter>
        <PrimarySearchAppBar />
        <Switch>
          <Login />
        </Switch>
      </BrowserRouter>
    </MuiThemeProvider>
  );
};

export default App;
