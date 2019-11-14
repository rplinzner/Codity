import * as React from 'react';
import {
  withLocalize,
  LocalizeContextProps,
  Translate,
} from 'react-localize-redux';

export interface Props extends LocalizeContextProps {}

class Home extends React.Component<Props> {
  public render() {
    return (
      <h1>
        <Translate id="greeting" />
      </h1>
    );
  }
}
export default withLocalize(Home);
