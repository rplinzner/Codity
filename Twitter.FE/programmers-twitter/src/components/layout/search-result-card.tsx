import React from 'react';
import { Card, Avatar, CardHeader, CardActionArea } from '@material-ui/core';

interface Props {}

const SearchResultCard: React.FC<Props> = () => {
  const avatar = <Avatar aria-label="person">MU</Avatar>;

  return (
    <Card>
      <CardActionArea onClick={() => alert('you clicked')}>
        <CardHeader
          avatar={avatar}
          title="Mr Unknown"
          subheader="69 followers"
        />
      </CardActionArea>
    </Card>
  );
};

export default SearchResultCard;
