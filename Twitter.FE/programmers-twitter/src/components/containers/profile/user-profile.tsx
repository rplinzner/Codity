import React from 'react';
import {
  Grid,
  Avatar,
  makeStyles,
  Typography,
  Theme,
  createStyles,
} from '@material-ui/core';
import { withRouter, RouteComponentProps } from 'react-router';

import RecordVoiceOverIcon from '@material-ui/icons/RecordVoiceOver';
import RssFeedIcon from '@material-ui/icons/RssFeed';

import FollowButton from './follow-unfollow-button';
// import { UserAvatar } from './user-avatar';

interface Props extends RouteComponentProps {}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    avatar: {
      width: 200,
      height: 200,
      margin: '0 auto',
    },
    element: {
      marginTop: theme.spacing(1),
    },
    typographyWithIcon: {
      marginTop: theme.spacing(1),
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
    },
    icon: {
      marginRight: theme.spacing(1),
    },
  }),
);

const UserProfile: React.FC<Props> = (props: Props) => {
  const classes = useStyles();

  const getUrlParams = (): URLSearchParams => {
    if (!props.location.search) {
      return new URLSearchParams();
    }
    return new URLSearchParams(props.location.search);
  };

  const getNameSearchValue = (): string => {
    const search = getUrlParams();
    return search.get('search') || '';
  };

  return (
    <Grid
      style={{ height: '90vh', padding: '20px' }}
      container
      justify="center"
      alignItems="center"
    >
      <Grid item sm={4} style={{ textAlign: 'center' }}>
        <Avatar
          src="https://s3.amazonaws.com/uifaces/faces/twitter/prrstn/128.jpg"
          className={classes.avatar}
        >
          RP
        </Avatar>
        <Typography className={classes.typographyWithIcon} variant="subtitle1">
          <RecordVoiceOverIcon className={classes.icon} />
          {'Followers: 69'}
        </Typography>
        <Typography className={classes.typographyWithIcon} variant="subtitle1">
          <RssFeedIcon className={classes.icon} />
          Following: 420
        </Typography>
        <FollowButton
          className={classes.element}
          isFollowing={true}
          reloadProfile={() => {}}
          userId={0}
        />
      </Grid>
      <Grid item sm={5}>
        <Typography className={classes.element} variant="h4">
          Name and Last Name
        </Typography>
        <Typography className={classes.element} variant="h5">
          29 years old
        </Typography>
        <Typography className={classes.element} variant="h6">
          About me
        </Typography>
        <Typography variant="body1">
          Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quia,
          tempora voluptatum. Dicta inventore veritatis, laboriosam quisquam
          repudiandae est quia, aut dignissimos, cupiditate cumque repellat
          consequuntur itaque alias veniam saepe impedit.
        </Typography>
      </Grid>
    </Grid>
  );
};

export default withRouter(UserProfile);
