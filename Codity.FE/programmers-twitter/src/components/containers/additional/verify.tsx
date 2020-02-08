import React from 'react';
import { RouteComponentProps } from 'react-router';
import {
  withLocalize,
  Translate as T,
  LocalizeContextProps,
} from 'react-localize-redux';
import { Typography, Container, Link } from '@material-ui/core';
import { Link as RouterLink } from 'react-router-dom';

type TParams = { type: string };

interface Props extends RouteComponentProps<TParams> {}

const Verify: React.FC<Props & LocalizeContextProps> = (
  props: Props & LocalizeContextProps,
) => {
  let message;
  switch (props.match.params.type) {
    case 'success':
      message = <T id="mailSuccess" />;
      break;
    case 'failed':
      message = <T id="mailFailed" />;
      break;
    default:
      message = <T id="somethingWrong" />;
  }

  return (
    <Container style={{ marginTop: '5%', textAlign: 'center' }}>
      <Typography variant="h2">{message}</Typography>
      <Typography variant="h4" style={{ marginTop: '10px' }}>
        <T
          data={{
            link: (
              <Link component={RouterLink} to="/">
                <T id="here" />
              </Link>
            ),
          }}
          id="clickHere"
        />
      </Typography>
    </Container>
  );
};

export default withLocalize(Verify);
