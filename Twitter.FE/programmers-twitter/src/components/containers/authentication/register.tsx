import React, { Component } from 'react';
import { Formik, Field, Form } from 'formik';
import * as Yup from 'yup';
import { toast } from 'react-toastify';
import { TextField } from 'formik-material-ui';
import { Button, Grid, Theme } from '@material-ui/core';
import ReCAPTCHA from 'react-google-recaptcha';
import {
  withLocalize,
  Translate as T,
  LocalizeContextProps,
} from 'react-localize-redux';

import * as constants from '../../../constants/global.constats';

import { authTranslations } from '../../../translations/index';
import { withStyles } from '@material-ui/styles';

interface Props extends LocalizeContextProps {
  classes: {
    button: string;
  }
}
interface State {
  canSubmit: boolean;
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
});

class Register extends Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.props.addTranslation(authTranslations);
  }

  state: State = { canSubmit: false };

  onSubmit = (values: FormValues): void => {
    toast.success('onSubmit triggered');
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

  render() {
    const { classes } = this.props;
    return (
      <Grid container justify="center">
        <Grid md={6} xs={10}>
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
                render={({ submitForm, isSubmitting }) => (
                  <Form>
                    <Field
                      name="firstName"
                      type="text"
                      label={<T id="firstName" />}
                      component={TextField}
                      margin="normal"
                      fullWidth
                    />
                    <br />
                    <Field
                      name="lastName"
                      type="text"
                      label={<T id="lastName" />}
                      component={TextField}
                      margin="normal"
                      fullWidth
                    />
                    <br />
                    <Field
                      name="email"
                      type="email"
                      label="Email"
                      component={TextField}
                      margin="normal"
                      fullWidth
                    />
                    <br />
                    <Field
                      name="password"
                      type="password"
                      label={<T id="password" />}
                      autoComplete="new-password"
                      component={TextField}
                      margin="normal"
                      fullWidth
                    />
                    <br />
                    <Field
                      name="passwordConfirm"
                      type="password"
                      autoComplete="new-password"
                      label={<T id="passwordConfirm" />}
                      component={TextField}
                      margin="normal"
                      fullWidth
                    />
                    <br />
                    <ReCAPTCHA
                      onChange={this.onCaptchaSubmitted}
                      sitekey={constants.RecaptchaSiteKey}
                    />
                    <Button
                      disabled={isSubmitting || !this.state.canSubmit}
                      variant="contained"
                      color="primary"
                      onClick={submitForm}
                    >
                      {<T id="sent" />}
                    </Button>
                    <Button className={classes.button} variant="contained" color="secondary" type="reset">
                      {<T id="reset" />}
                    </Button>
                  </Form>
                )}
              />
            )}
          </T>
        </Grid>
      </Grid>
    );
  }
}

export default withStyles(styles, { withTheme: true })(withLocalize(Register));
