import React, { Component } from 'react';
import { Formik, Field, Form } from 'formik';
import * as Yup from 'yup';
import { TextField } from 'formik-material-ui';
import {
  Button,
  Grid,
  Theme,
  Modal,
  Fade,
  Backdrop,
  LinearProgress,
} from '@material-ui/core';
import ReCAPTCHA from 'react-google-recaptcha';
import {
  withLocalize,
  Translate as T,
  LocalizeContextProps,
} from 'react-localize-redux';
import { authTranslations } from '../../../translations/index';
import { withStyles } from '@material-ui/styles';

import * as constants from '../../../constants/global.constats';

import post from '../../../services/post.service';
import displayErrors from '../../../helpers/display-errors';

interface Props extends LocalizeContextProps {
  classes: {
    button: string;
    modal: string;
    paper: string;
  };
  theme: Theme;
}
interface State {
  canSubmit: boolean;
  isSubmitting: boolean;
  isOpen: boolean;
}

interface FormValues {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

const styles = (theme: Theme) => ({
  button: {
    margin: theme.spacing(1),
  },
  modal: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
  },
  paper: {
    backgroundColor: theme.palette.background.paper,
    border: '2px solid #000',
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
  },
});

class Register extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.props.addTranslation(authTranslations);
  }

  state: State = { canSubmit: false, isOpen: false, isSubmitting: false };

  onSubmit = (values: FormValues): void => {
    this.setState({ isSubmitting: true });
    post(values, `${constants.server}/api/authentication`, '/register').then(
      () => this.setState({ isOpen: true, isSubmitting: false }),
      error => {
        this.setState({ isSubmitting: false });
        displayErrors(error);
      },
    );
  };

  onCaptchaSubmitted = () => {
    this.setState({ canSubmit: true });
  };

  getSchema = (translate: any) => {
    return Yup.object({
      firstName: Yup.string().required(translate('firstNameRequired')),
      lastName: Yup.string().required(translate('lastNameRequired')),
      email: Yup.string()
        .email(translate('emailValid'))
        .required(translate('emailRequired')),
      password: Yup.string().required(translate('passwordRequired')),
      passwordConfirm: Yup.string()
        .oneOf([Yup.ref('password'), null], translate('passwordsMustMatch'))
        .required(translate('passwordConfirmRequired')),
    });
  };

  handleClose = () => {
    this.setState({ isOpen: false });
  };

  render() {
    const { classes, theme } = this.props;
    return (
      <>
        <Grid container={true} justify="center">
          <Grid item={true} xs={10} md={6} lg={5} xl={4}>
            <T>
              {({ translate }) => (
                <Formik
                  initialValues={{
                    firstName: '',
                    lastName: '',
                    email: '',
                    password: '',
                    passwordConfirm: '',
                  }}
                  validationSchema={this.getSchema(translate)}
                  onSubmit={this.onSubmit}
                  render={({ submitForm, handleSubmit }) => (
                    <Form onSubmit={handleSubmit}>
                      <Field
                        name="firstName"
                        type="text"
                        label={<T id="firstName" />}
                        component={TextField}
                        margin="normal"
                        fullWidth={true}
                      />
                      <br />
                      <Field
                        name="lastName"
                        type="text"
                        label={<T id="lastName" />}
                        component={TextField}
                        margin="normal"
                        fullWidth={true}
                      />
                      <br />
                      <Field
                        name="email"
                        type="email"
                        label="Email"
                        component={TextField}
                        margin="normal"
                        fullWidth={true}
                      />
                      <br />
                      <Field
                        name="password"
                        type="password"
                        label={<T id="password" />}
                        autoComplete="new-password"
                        component={TextField}
                        margin="normal"
                        fullWidth={true}
                      />
                      <br />
                      <Field
                        name="passwordConfirm"
                        type="password"
                        autoComplete="new-password"
                        label={<T id="passwordConfirm" />}
                        component={TextField}
                        margin="normal"
                        fullWidth={true}
                      />
                      <br />
                      <ReCAPTCHA
                        theme={theme.palette.type}
                        onChange={this.onCaptchaSubmitted}
                        sitekey={constants.RecaptchaSiteKey}
                      />
                      <Button
                        disabled={
                          this.state.isSubmitting || !this.state.canSubmit
                        }
                        variant="contained"
                        color="primary"
                        onClick={submitForm}
                      >
                        {<T id="sent" />}
                      </Button>
                      <Button
                        className={classes.button}
                        variant="contained"
                        color="secondary"
                        type="reset"
                      >
                        {<T id="reset" />}
                      </Button>
                    </Form>
                  )}
                />
              )}
            </T>
            {this.state.isSubmitting && <LinearProgress />}
          </Grid>
        </Grid>
        <Modal
          aria-labelledby="register-modal-title"
          aria-describedby="register-modal-description"
          className={classes.modal}
          open={this.state.isOpen}
          onClose={this.handleClose}
          closeAfterTransition={true}
          BackdropComponent={Backdrop}
          BackdropProps={{
            timeout: 500,
          }}
        >
          <Fade in={this.state.isOpen}>
            <div className={classes.paper}>
              <h2 id="register-modal-title">
                <T id="registrationCompleted" />
              </h2>
              <p id="register-modal-description">
                <T id="checkMailBox" />
              </p>
              <Button
                variant="contained"
                color="primary"
                onClick={this.handleClose}
              >
                <T id="close" />
              </Button>
            </div>
          </Fade>
        </Modal>
      </>
    );
  }
}

export default withStyles(styles, { withTheme: true })(withLocalize(Register));
