import React from 'react';
import { Card, CardHeader, CardActionArea } from '@material-ui/core';
import { UserAvatar } from '../containers/profile';
import {
  LocalizeContextProps,
  withLocalize,
  Translate as T,
} from 'react-localize-redux';
import { withRouter, RouteComponentProps } from 'react-router-dom';

interface Props extends LocalizeContextProps {
  firstName: string;
  lastName: string;
  photo: string | null;
  followers: number;
  userId: number;
}

const SearchResultCard: React.FC<Props & RouteComponentProps> = (
  props: Props & RouteComponentProps,
) => {
  return (
    <Card>
      <T>
        {({ translate }) => (
          <CardActionArea
            onClick={() =>
              props.history.push(`/profile/?userId=${props.userId}`)
            }
          >
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

export default withLocalize(withRouter(SearchResultCard));
