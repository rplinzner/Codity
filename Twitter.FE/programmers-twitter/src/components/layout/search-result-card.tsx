import React from 'react';
import { Card, CardHeader, CardActionArea } from '@material-ui/core';
import { UserAvatar } from '../containers/profile';

interface Props {
  firstName: string;
  lastName: string;
  photo: string | null;
  followers: number;
}

const SearchResultCard: React.FC<Props> = (props: Props) => {
  return (
    <Card>
      <CardActionArea onClick={() => alert('you clicked')}>
        <CardHeader
          avatar={
            <UserAvatar
              firstName={props.firstName}
              lastName={props.lastName}
              photo={props.photo}
            />
          }
          title={props.firstName + ' ' + props.lastName}
          subheader={props.followers + " followers"}
        />
      </CardActionArea>
    </Card>
  );
};

export default SearchResultCard;
