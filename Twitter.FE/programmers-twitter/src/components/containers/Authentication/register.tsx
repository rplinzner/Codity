import React, { Component } from 'react';
import { Formik, Field, Form } from 'formik';
import * as Yup from 'yup';
import { toast } from 'react-toastify';
import { TextField } from 'formik-material-ui';
import { Button, Container } from '@material-ui/core';

interface Props {}
interface State {
  canSubmit: boolean;
}

interface FormValues {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export default class extends Component<Props, State> {
  state: State = { canSubmit: false };

  onSubmit = (values: FormValues): void => {
    toast.success('onSubmit triggered');
  };

  render() {
    const schema = Yup.object({
      firstName: Yup.string().required('To be filled'),
      lastName: Yup.string().required('Firld '),
      email: Yup.string()
        .email('valid must be')
        .required(),
      password: Yup.string().required(),
      passwordConfirm: Yup.string()
        .oneOf([Yup.ref('password'), null], 'Passwords must match translation')
        .required('confirm your password'),
    });

    return (
      <Container>
        <Formik
          initialValues={{
            firstName: '',
            lastName: '',
            email: '',
            password: '',
            passwordConfirm: '',
          }}
          validationSchema={schema}
          onSubmit={this.onSubmit}
          render={({ submitForm, isSubmitting }) => (
            <Form>
              <Field
                name="firstName"
                type="text"
                label="First Name translation"
                component={TextField}
                margin="normal"
                fullWidth
              />
              <br />
              <Field
                name="lastName"
                type="text"
                label="Last Name translation"
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
                label="password translation"
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
                label="passwordConfirm translation"
                component={TextField}
                margin="normal"
                fullWidth
              />
              <br />
              <Button variant="contained" color="primary" onClick={submitForm}>
                Submit
              </Button>
            </Form>
          )}
        />
      </Container>
    );
  }
}
