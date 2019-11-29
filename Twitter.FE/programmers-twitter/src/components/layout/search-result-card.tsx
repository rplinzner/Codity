import React from 'react';
import { Card, CardHeader, CardActionArea } from '@material-ui/core';
import { UserAvatar } from '../containers/profile';
import {
  LocalizeContextProps,
  withLocalize,
  Translate as T,
} from 'react-localize-redux';

interface Props extends LocalizeContextProps {
  firstName: string;
  lastName: string;
  photo: string | null;
  followers: number;
}


const SearchResultCard: React.FC<Props> = (props: Props) => {
  return (
    <Card>
      <T>
        {({ translate }) => (
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
              subheader={props.followers + ' ' + translate('followers')}
            />
          </CardActionArea>
        )}
      </T>
    </Card>
  );
};

export default withLocalize(SearchResultCard);
