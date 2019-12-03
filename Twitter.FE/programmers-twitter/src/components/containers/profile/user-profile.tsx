import React, { useState, useEffect } from 'react';
import {
  Grid,
  makeStyles,
  Typography,
  Theme,
  createStyles,
  Divider,
  LinearProgress,
} from '@material-ui/core';
import { withRouter, RouteComponentProps } from 'react-router';

import RecordVoiceOverIcon from '@material-ui/icons/RecordVoiceOver';
import RssFeedIcon from '@material-ui/icons/RssFeed';

import FollowButton from './follow-unfollow-button';
import UserAvatar from './user-avatar';
import { ProfileResponse } from '../../../types/profile-response';
import get from '../../../services/get.service';
import * as constants from '../../../constants/global.constats';
import {
  Translate as T,
  LocalizeContextProps,
  withLocalize,
} from 'react-localize-redux';
import displayErrors from '../../../helpers/display-errors';
import CardSceleton from '../feed/card-sceleton';

interface Props extends RouteComponentProps {}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    avatar: {
      width: 200,
      height: 200,
      margin: '0 auto',
      [theme.breakpoints.down('xs')]: {
        height: 100,
        width: 100,
      },
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
    userDescription: {
      [theme.breakpoints.down('xs')]: {
        textAlign: 'center',
        paddingTop: theme.spacing(5),
      },
    },
    posts: {
      textAlign: 'center',
      [theme.breakpoints.down('xs')]: {
        paddingTop: theme.spacing(5),
      },
    },
    divider: {
      marginBottom: theme.spacing(2),
    },
  }),
);

const UserProfile: React.FC<Props & LocalizeContextProps> = (
  props: Props & LocalizeContextProps,
) => {
  const classes = useStyles();

  const [userProfile, setUserProfile] = useState<ProfileResponse | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const getUrlParams = (): URLSearchParams => {
    if (!props.location.search) {
      return new URLSearchParams();
    }
    return new URLSearchParams(props.location.search);
  };

  const getUserIdSearchValue = (): string => {
    const search = getUrlParams();
    return search.get('userId') || '';
  };

  const getUserProfile = (): void => {
    setIsLoading(true);
    let lang = 'en';
    if (props.activeLanguage) {
      lang = props.activeLanguage.code;
    }
    const id = getUserIdSearchValue();
    if (id !== '') {
      get<ProfileResponse>(
        constants.usersController,
        `/${id}`,
        lang,
        <T id="errorConnection" />,
        true,
      ).then(
        resp => {
          setUserProfile(resp);
          setIsLoading(false);
        },
        errors => {
          displayErrors(errors);
          setIsLoading(false);
        },
      );
    } else {
      setUserProfile(null);
      setIsLoading(false);
    }
  };

  const calculateAge = (date: string) => {
    var today = new Date();
    var birthDate = new Date(date);
    var age_now = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age_now--;
    }
    return age_now;
  };

  useEffect(() => {
    getUserProfile();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.location.search]);
  return (
    <>
      {userProfile !== null ? (
        <Grid
          style={{ height: '90vh', padding: '20px' }}
          container
          justify="center"
          alignItems="center"
        >
          <Grid item xs={10} sm={6} lg={5} style={{ textAlign: 'center' }}>
            <UserAvatar
              firstName={userProfile.model.firstName}
              lastName={userProfile.model.lastName}
              className={classes.avatar}
              photo={userProfile.model.image}
            />
            <Typography
              className={classes.typographyWithIcon}
              variant="subtitle1"
            >
              <RecordVoiceOverIcon className={classes.icon} />
              <T id="followers" />
              {': '}
              {userProfile.model.followersCount}
            </Typography>
            <Typography
              className={classes.typographyWithIcon}
              variant="subtitle1"
            >
              <RssFeedIcon className={classes.icon} />
              <T id="following" />
              {': '}
              {userProfile.model.followingCount}
            </Typography>
            <FollowButton
              className={classes.element}
              isFollowing={userProfile.model.isFollowing}
              reloadProfile={() => getUserProfile()}
              userId={userProfile.model.id}
            />
          </Grid>

          <Grid item xs={10} sm={5} lg={3} className={classes.userDescription}>
            <Typography className={classes.element} variant="h4">
              {userProfile.model.firstName + ' ' + userProfile.model.lastName}
            </Typography>
            <Typography className={classes.element} variant="h5">
              {userProfile.model.birthDay
                ? calculateAge(userProfile.model.birthDay) + ' '
                : null}
              {userProfile.model.birthDay ? <T id="yearsOld" /> : null}
            </Typography>
            <Typography className={classes.element} variant="h6">
              <T id="aboutMe" />
            </Typography>
            <Typography variant="body1" style={{textAlign: 'justify'}}>
              {userProfile.model.aboutMe || <T id="notWrittenYet" />}
            </Typography>
          </Grid>

          <Grid item xs={10} sm={10} lg={10} className={classes.posts}>
            <Divider className={classes.divider} />
            <Typography variant="h5">Recent Posts:</Typography>
            <CardSceleton />
          </Grid>
        </Grid>
      ) : isLoading ? (
        <LinearProgress />
      ) : (
        <Typography variant="h4" style={{ textAlign: 'center' }}>
          <T id="noData" />
        </Typography>
      )}
    </>
  );
};

export default withLocalize(withRouter(UserProfile));
