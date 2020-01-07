import React, { useState } from 'react';
import { PostSkeleton } from '../../custom/index';
import { PostsResponse } from '../../../types/posts-response';
import { Container } from '@material-ui/core';
interface Props {}

const Feed: React.FC<Props> = () => {
  const [postResponse, setPostResponse] = useState<PostsResponse | null>(null);
  console.log(postResponse, setPostResponse);

  return (
    <Container maxWidth="md">
      <PostSkeleton />
    </Container>
  );
};

export default Feed;
