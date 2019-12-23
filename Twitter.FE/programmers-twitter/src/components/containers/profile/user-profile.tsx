import React, { useState, useEffect } from 'react';
import {
  Grid,
  makeStyles,
  Typography,
  Theme,
  createStyles,
  Divider,
  LinearProgress,
  Button,
  TextField,
} from '@material-ui/core';
import { withRouter, RouteComponentProps } from 'react-router';
import {
  MuiPickersUtilsProvider,
  KeyboardDatePicker,
} from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';
import enLocale from 'date-fns/locale/en-US';
import plLocale from 'date-fns/locale/pl';

import RecordVoiceOverIcon from '@material-ui/icons/RecordVoiceOver';
import RssFeedIcon from '@material-ui/icons/RssFeed';

import FollowButton from './follow-unfollow-button';
import UserAvatar from './user-avatar';
import { ProfileResponse } from '../../../types/profile-response';
import get from '../../../services/get.service';
import put from '../../../services/put.service';
import * as constants from '../../../constants/global.constats';
import {
  Translate as T,
  LocalizeContextProps,
  withLocalize,
} from 'react-localize-redux';
import displayErrors from '../../../helpers/display-errors';
import CardSceleton from '../feed/card-sceleton';
import { AppState } from '../../..';
import { connect } from 'react-redux';
import { UserState } from '../../../store/user/user.types';
import AddPhotoAvatar from './add-photo-avatar';
import { toast } from 'react-toastify';
import { BaseResponse } from '../../../types/base-response';

interface Props extends RouteComponentProps {
  user: UserState;
}

interface ProfileUpdateBody {
  image: string | null;
  aboutMe: string | null;
  birthDay: string | null;
  genderId: number | null;
}

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
      margin: theme.spacing(2, 0, 2, 0),
      width: '100%',
    },
    editButton: {
      margin: theme.spacing(2),
    },
    aboutMeTextField: { width: '100%', margin: theme.spacing(2, 0, 1, 0) },
  }),
);

const UserProfile: React.FC<Props & LocalizeContextProps> = (
  props: Props & LocalizeContextProps,
) => {
  const classes = useStyles();

  const [userProfile, setUserProfile] = useState<ProfileResponse | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isEditing, setIsEditing] = useState(false);

  // -- Editable states --
  const [birthDate, setBirthDate] = useState<Date | null>(new Date());
  const [aboutMe, setAboutMe] = useState('');
  const [image, setImage] = useState('');
  // TODO: Add gender

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

  const langCode = props.activeLanguage ? props.activeLanguage.code : 'en';

  const getUserProfile = (): void => {
    setIsLoading(true);
    const id = getUserIdSearchValue();
    if (id !== '') {
      get<ProfileResponse>(
        constants.usersController,
        `/${id}`,
        langCode,
        <T id="errorConnection" />,
        true,
      )
        .then(
          resp => {
            setUserProfile(resp);
            setIsLoading(false);
            setEditableStates(resp);
          },
          errors => {
            displayErrors(errors);
            setIsLoading(false);
          },
        )
        .then(() => setIsEditing(false));
    } else {
      setUserProfile(null);
      setIsLoading(false);
    }
  };

  const setEditableStates = (profile: ProfileResponse): void => {
    const { birthDay, aboutMe } = profile.model;
    if (birthDay !== null) {
      setBirthDate(new Date(birthDay));
    }
    if (aboutMe !== null) {
      setAboutMe(aboutMe);
    }
  };

  const calculateAge = (date: string) => {
    const today = new Date();
    const birthDate = new Date(date);
    let age_now = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age_now--;
    }
    return age_now;
  };

  const handleDateChange = (date: Date | null) => {
    setBirthDate(date);
  };

  const isOwnProfile = (): boolean => {
    if (
      props.user &&
      props.user.user &&
      // tslint:disable-next-line: radix
      props.user.user.id === parseInt(getUserIdSearchValue())
    ) {
      return true;
    }
    return false;
  };

  const handleEditButton = () => {
    if (!isEditing) {
      setIsEditing(true);
    } else {
      toast.info('Please wait for server response'); //TODO: translation
      const body: ProfileUpdateBody = {
        aboutMe,
        birthDay: birthDate !== null ? birthDate.toISOString() : null,
        genderId: 1,
        image,
      };
      put<BaseResponse, ProfileUpdateBody>(
        body,
        constants.usersController,
        '/profile',
        langCode,
        <T id="errorConnection" />,
        true,
      ).then(
        () => {
          toast.success('profile successfully updated');
          getUserProfile();
        },
        errors => {
          displayErrors(errors);
          setIsEditing(false);
        },
      );
    }
  };

  const handleAboutMeTextField = (
    event: React.ChangeEvent<HTMLInputElement>,
  ) => {
    setAboutMe(event.target.value);
  };

  const handleImage = (image: string) => {
    setImage(image);
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
          container={true}
          justify="center"
          alignItems="center"
        >
          <Grid
            item={true}
            xs={10}
            sm={6}
            lg={5}
            style={{ textAlign: 'center' }}
          >
            {isOwnProfile() && (
              <>
                <Button
                  className={classes.editButton}
                  color="primary"
                  variant="contained"
                  onClick={handleEditButton}
                >
                  <T id={isEditing ? 'save' : 'editProfile'} />
                </Button>
                {isEditing && (
                  <Button
                    className={classes.editButton}
                    color="secondary"
                    variant="contained"
                    onClick={() => setIsEditing(false)}
                  >
                    Cancel
                    {/* TODO: Translation */}
                  </Button>
                )}
              </>
            )}
            {/* User Photo */}
            {isEditing ? (
              <>
                <AddPhotoAvatar
                  className={classes.avatar}
                  handleImage={handleImage}
                />
              </>
            ) : (
              <UserAvatar
                firstName={userProfile.model.firstName}
                lastName={userProfile.model.lastName}
                className={classes.avatar}
                photo={userProfile.model.image}
              />
            )}
            {/* User basic info */}
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
            {!isOwnProfile() && (
              <FollowButton
                className={classes.element}
                isFollowing={userProfile.model.isFollowing}
                reloadProfile={() => getUserProfile()}
                userId={userProfile.model.id}
              />
            )}
          </Grid>

          <Grid
            item={true}
            xs={10}
            sm={5}
            lg={3}
            className={classes.userDescription}
          >
            {/* User more info */}
            <Typography className={classes.element} variant="h4">
              {userProfile.model.firstName + ' ' + userProfile.model.lastName}
            </Typography>
            {/* -- Age -- */}
            <Typography className={classes.element} variant="h5">
              {userProfile.model.birthDay && !isEditing
                ? calculateAge(userProfile.model.birthDay) + ' '
                : null}
              {userProfile.model.birthDay && !isEditing ? (
                <T id="yearsOld" />
              ) : null}
            </Typography>
            {isEditing && (
              <MuiPickersUtilsProvider
                locale={langCode.includes('pl') ? plLocale : enLocale}
                utils={DateFnsUtils}
              >
                <KeyboardDatePicker
                  margin="normal"
                  id="date-picker-dialog"
                  label={<T id="birthDay" />}
                  format={langCode.includes('pl') ? 'dd.MM.yyyy' : 'MM/dd/yyyy'}
                  value={birthDate}
                  onChange={handleDateChange}
                  KeyboardButtonProps={{
                    'aria-label': 'change date',
                  }}
                />
              </MuiPickersUtilsProvider>
            )}
            {/* About me section */}
            {!isEditing ? (
              <>
                <Typography className={classes.element} variant="h6">
                  <T id="aboutMe" />
                </Typography>
                <Typography variant="body1">
                  {userProfile.model.aboutMe || <T id="notWrittenYet" />}
                </Typography>
              </>
            ) : (
              <TextField
                variant="outlined"
                className={classes.aboutMeTextField}
                value={aboutMe}
                onChange={handleAboutMeTextField}
                label={<T id="aboutMe" />}
                multiline
                rows={5}
              />
            )}
          </Grid>
          {/*  User Posts */}
          <Divider className={classes.divider} />
          <Grid item={true} xs={10} sm={10} lg={10} className={classes.posts}>
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

const mapStateToProps = (state: AppState) => ({
  user: state.user,
});

export default connect(mapStateToProps)(withLocalize(withRouter(UserProfile)));
