import React, { Component } from 'react';
import CardSceleton from './card-sceleton';

interface Props {}
interface State {}

class Feed extends Component<Props, State> {
  state = {};

  render() {
    return (
      <>
        <CardSceleton />
      </>
    );
  }
}

export default Feed;
