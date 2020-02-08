import React from 'react';
import { Button } from '@material-ui/core';
import {
  Translate as T,
  withLocalize,
  LocalizeContextProps,
} from 'react-localize-redux';

import del from '../../../services/delete.service';
import post from '../../../services/post.service';
import { usersController } from '../../../constants/global.constats';
import displayErrors from '../../../helpers/display-errors';

interface Props extends LocalizeContextProps {
  isFollowing: boolean;
  userId: number;
  className: string;
  reloadProfile: () => void;
}

const FollowUnfollowButton: React.FC<Props> = (props: Props) => {
  const handleFollow = () => {
    if (props.isFollowing) {
      del(
        { followingId: props.userId },
        usersController,
        '/following',
        props.activeLanguage.code,
        <T id="errorConnection" />,
        true,
      ).then(
        () => {
          props.reloadProfile();
        },
        error => displayErrors(error),
      );
    } else {
      post(
        { followingId: props.userId },
        usersController,
        '/following',
        props.activeLanguage.code,
        <T id="errorConnection" />,
        true,
      ).then(
        () => {
          props.reloadProfile();
        },
        error => displayErrors(error),
      );
    }
  };
  return (
    <Button
      className={props.className}
      onClick={handleFollow}
      variant={props.isFollowing ? 'outlined' : 'contained'}
      color="primary"
    >
      {props.isFollowing ? <T id="unfollow" /> : <T id="follow" />}
    </Button>
  );
};

export default withLocalize(FollowUnfollowButton);
