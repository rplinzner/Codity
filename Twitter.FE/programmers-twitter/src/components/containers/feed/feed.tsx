import React, { Component } from 'react';
import { PostSkeleton } from '../../custom/index';

interface Props {}
interface State {}

class Feed extends Component<Props, State> {
  state = {};

  render() {
    return (
      <>
        <PostSkeleton />
      </>
    );
  }
}

export default Feed;
