import * as React from 'react';
import {
  withLocalize,
  LocalizeContextProps,
  Translate,
} from 'react-localize-redux';
import { Typography } from '@material-ui/core';

export interface Props extends LocalizeContextProps {}

class Home extends React.Component<Props> {
  public render() {
    return (
      <Typography variant="h3">
        <Translate id="greeting" />
      </Typography>
    );
  }
}
export default withLocalize(Home);
