import React, { Component } from 'react';
import { TextField, Paper, Button, Grid, Link } from '@material-ui/core';
import { withStyles, Theme } from '@material-ui/core/styles';
import {
  Link as RouterLink,
  LinkProps as RouterLinkProps,
} from 'react-router-dom';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';

import { authTranslations } from '../../../translations/index';

interface Props extends LocalizeContextProps {
  classes: {
    root: string;
    form: string;
  };
}
interface State {
  email: string;
  password: string;
}

const styles = (theme: Theme) => ({
  root: {
    padding: theme.spacing(3, 2),
  },
  form: {
    margin: 'auto',
    width: '100%',
  },
});

const Link1 = React.forwardRef<HTMLAnchorElement, RouterLinkProps>(
  (props, ref) => <RouterLink innerRef={ref} {...props} />
);

class Login extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.props.addTranslation(authTranslations);
  }
  state: State = {
    email: '',
    password: '',
  };

  handleChange = (event: any) => {
    const FormValue: string = event.currentTarget.value;
    const id: any = event.currentTarget.id;
    this.setState({ [id]: FormValue } as Pick<State, keyof State>);
  };

  render() {
    const { classes } = this.props;
    return (
      <Grid
        container={true}
        justify="center"
        alignItems="center"
        style={{ height: '90vh' }} //TODO: Move to jss and account for different nav bar heights
      >
        <Grid item={true} xs={10} md={6}>
          <Paper className={classes.root}>
            <h3>
              <T id="auth-credentials" />
            </h3>
            <form noValidate={false} className={classes.form}>
              <TextField
                fullWidth={true}
                required={true}
                type="email"
                id="email"
                label="E-mail"
                margin="normal"
                value={this.state.email}
                onChange={this.handleChange}
              />

              <br />
              <TextField
                fullWidth={true}
                required={true}
                type="password"
                id="password"
                label={<T id="password" />}
                margin="normal"
                value={this.state.password}
                onChange={this.handleChange}
              />
              <br />
              <Button
                style={{ marginTop: '10px' }}
                type="submit"
                variant="contained"
                color="primary"
              >
                {<T id="sent" />}
              </Button>
            </form>
            <p>
              <T
                id="auth-newhere"
                data={{
                  link: (
                    <Link component={Link1} to="/Register">
                      <T id="here" />
                    </Link>
                  ),
                }}
              />
            </p>
          </Paper>
        </Grid>
      </Grid>
    );
  }
}

export default withStyles(styles, { withTheme: true })(withLocalize(Login));
