import React, { Component } from 'react';
import { TextField, Paper, Container, Button } from '@material-ui/core';

interface Props {}
interface State {
  //   email: string;
  //   password: string;
}

export default class extends Component<Props, State> {
  //   state = { email: '', password: '' };

  render() {
    return (
      <Container maxWidth="sm">
        <Paper>
          <form noValidate={false}>
            <TextField required={true} type="email" id="email" label="E-mail" />
            <TextField
              required={true}
              type="password"
              id="password"
              label="Password"
            />
            <Button type="submit" variant="contained" color="primary">
              Wyślij
            </Button>
          </form>
        </Paper>
      </Container>
    );
  }
}
