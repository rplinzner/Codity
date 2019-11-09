import React, { Component } from 'react';
import { TextField, Paper, Container } from '@material-ui/core';

interface Props {}
interface State {
  //   email: string;
  //   password: string;
}

export default class Login extends Component<Props, State> {
  //   state = { email: '', password: '' };

  render() {
    return (
      <Container maxWidth="sm">
        <Paper>
          <form noValidate={true}>
            <TextField required={true} type="text" id="email" label="E-mail" />
            <TextField
              required={true}
              type="password"
              id="password"
              label="Password"
            />
          </form>
        </Paper>
      </Container>
    );
  }
}
