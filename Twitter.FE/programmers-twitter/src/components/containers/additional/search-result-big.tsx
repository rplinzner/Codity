import React from 'react';
import { Container } from '@material-ui/core';
import UserCard from './../../layout/search-result-card';

interface Props {}

const SearchResult: React.FC<Props> = () => {
  return (
    <Container>
      {/* TODO: Do special card with follow button */}
      <UserCard />
      <UserCard />
      <UserCard />
    </Container>
  );
};
export default SearchResult;
