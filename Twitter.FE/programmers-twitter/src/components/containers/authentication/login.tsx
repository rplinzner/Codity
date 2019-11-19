import React, { Component } from 'react';
import {
  TextField,
  Paper,
  Button,
  Grid,
  Link,
  Typography,
  LinearProgress,
} from '@material-ui/core';
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
import { AppState } from '../../../index';
import { UserState } from '../../../store/user/user.types';
import { login } from '../../../store/user/user.actions';
import { connect } from 'react-redux';

interface Props extends LocalizeContextProps {
  classes: {
    root: string;
    form: string;
    container: string;
    typo: string;
  };
  user: UserState;
  isLoggingIn: boolean;
  loginAction: typeof login;
}
interface State {
  email: string;
  password: string;
}

const styles = (theme: Theme) => ({
  root: {
    padding: theme.spacing(3, 2),
  },
  container: {
    height: '90vh',
  },
  form: {
    margin: 'auto',
    width: '100%',
  },
  typo: {
    [theme.breakpoints.down('xs')]: {
      top: '1vh',
    },
    [theme.breakpoints.up('sm')]: {
      top: '10vh',
    },
    [theme.breakpoints.up('md')]: {
      top: '12vh',
    },
  },
});

const Link1 = React.forwardRef<HTMLAnchorElement, RouterLinkProps>(
  (props, ref) => <RouterLink innerRef={ref} {...props} />,
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

  handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    this.props.loginAction(this.state.email, this.state.password);
  };

  render() {
    const { classes } = this.props;
    return (
      <Grid
        container={true}
        justify="center"
        alignItems="flex-start"
        className={classes.container}
      >
        <Grid item className={classes.root}>
          <Typography
            style={{ position: 'relative' }}
            className={classes.typo}
            align="center"
            variant="h3"
          >
            <T id="greeting" />
          </Typography>
        </Grid>
        <Grid item={true} xs={10} md={6}>
          <Paper className={classes.root}>
            <Typography variant="h5">
              <T id="auth-credentials" />
            </Typography>
            <form
              noValidate={false}
              onSubmit={this.handleSubmit}
              className={classes.form}
            >
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
            {this.props.isLoggingIn && <LinearProgress />}
          </Paper>
        </Grid>
      </Grid>
    );
  }
}

const mapStateToProps = (state: AppState) => ({
  user: state.user,
  isLoggingIn: state.user.loggingIn,
});

const mapDispatchToProps = (dispatch: any) => {
  return {
    loginAction: (email: string, password: string) =>
      dispatch(login(email, password)),
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(withStyles(styles, { withTheme: true })(withLocalize(Login)));
