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
  FormControlLabel,
  Checkbox,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
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
import EmojiPeopleIcon from '@material-ui/icons/EmojiPeople';

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
import GenderResponse from '../../../types/gender-response';
import FollowingFollowersModal from './following-followers-modal';

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
    container: {
      paddingTop: theme.spacing(2),
    },
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
    formControl: {
      margin: theme.spacing(1),
      minWidth: 120,
    },
  }),
);

const UserProfile: React.FC<Props & LocalizeContextProps> = (
  props: Props & LocalizeContextProps,
) => {
  const classes = useStyles();

  const [userProfile, setUserProfile] = useState<ProfileResponse | null>(null);
  const [genders, setGenders] = useState<GenderResponse>();
  const [isLoading, setIsLoading] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [isFollowingOpen, setIsFollowingOpen] = useState(false);
  const [isFollowersOpen, setIsFollowersOpen] = useState(false);

  // -- Editable states --
  const [editBirthDate, setEditBirthDate] = useState<Date | null>(new Date());
  const [showBirthDay, setShowBirthDay] = useState<boolean>(true);
  const [editAboutMe, setEditAboutMe] = useState('');
  const [editImage, setEditImage] = useState('');
  const [selectedGender, setSelectedGender] = useState<number>(0);

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
  const userId = getUserIdSearchValue();

  //#region API calls

  const getUserProfile = (): void => {
    setIsLoading(true);
    const id = userId;
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
        .then(() => {
          setIsEditing(false);
        });
    } else {
      setUserProfile(null);
      setIsLoading(false);
    }
  };

  const getGenders = () => {
    get<GenderResponse>(
      constants.genderController,
      '',
      langCode,
      <T id="errorConection" />,
      true,
    ).then(
      resp => {
        const temp = resp;
        temp.models.push({
          genderId: 0,
          genderName: 'notProvided',
        });

        setGenders(temp);
      },
      errors => displayErrors(errors),
    );
  };

  const handleEditButton = () => {
    if (!isEditing) {
      setGenderByName(userProfile ? userProfile.model.genderName : null);
      setIsEditing(true);
    } else {
      toast.info(<T id="waitForServerResponse" />);
      let birthDayToSend: string | null = null;
      if (showBirthDay && editBirthDate) {
        birthDayToSend = editBirthDate.toISOString();
      }

      const body: ProfileUpdateBody = {
        aboutMe: editAboutMe,
        birthDay: birthDayToSend,
        genderId: selectedGender === 0 ? null : selectedGender,
        image: editImage,
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
          getUserProfile();
        },
        errors => {
          displayErrors(errors);
          setIsEditing(false);
        },
      );
    }
  };
  //#endregion

  const setGenderByName = (name: string | null) => {
    if (name === null) {
      setSelectedGender(0);
    } else {
      if (genders?.models) {
        const models = genders.models;
        const tempGender = models.find(e => e.genderName === name);
        setSelectedGender(tempGender ? tempGender.genderId : 0);
      }
    }
  };

  const setEditableStates = (profile: ProfileResponse): void => {
    const { birthDay, aboutMe, image } = profile.model;
    setEditBirthDate(new Date(birthDay));
    setShowBirthDay(birthDay !== null);

    setEditAboutMe(aboutMe || '');

    setEditImage(image);
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

  const isOwnProfile = (): boolean => {
    if (
      // tslint:disable-next-line: radix
      props.user?.details?.id === parseInt(userId)
    ) {
      return true;
    }
    return false;
  };

  const handleDateChange = (date: Date | null) => {
    setEditBirthDate(date);
  };

  const handleAboutMeTextField = (
    event: React.ChangeEvent<HTMLInputElement>,
  ): void => {
    setEditAboutMe(event.target.value);
  };

  const handleImage = (image: string) => {
    setEditImage(image);
  };
  const handleCheckBox = (name: string) => (
    event: React.ChangeEvent<HTMLInputElement>,
  ): void => {
    switch (name) {
      case 'showBirthDay':
        setShowBirthDay(event.target.checked);
        break;

      default:
        break;
    }
  };

  const handleGenderChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    setSelectedGender(event.target.value as number);
  };

  useEffect(() => {
    getGenders();
    getUserProfile();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.location.search]);

  return (
    <div className={classes.container}>
      {isLoading && <LinearProgress />}
      {userProfile !== null ? (
        <Grid
          style={{ padding: '20px' }}
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
                    <T id="cancel" />
                  </Button>
                )}
              </>
            )}
            {/* User Photo */}
            {isEditing ? (
              <AddPhotoAvatar
                className={classes.avatar}
                handleImage={handleImage}
                existingPic={editImage}
              />
            ) : (
              <UserAvatar
                firstName={userProfile.model.firstName}
                lastName={userProfile.model.lastName}
                className={classes.avatar}
                photo={userProfile.model.image}
              />
            )}
            {/* User basic info */}
            {!isEditing && userProfile.model.genderName && (
              <Typography
                className={classes.typographyWithIcon}
                variant="subtitle1"
              >
                <EmojiPeopleIcon className={classes.icon} />
                {userProfile.model.genderName}
              </Typography>
            )}
            {isEditing && genders && (
              <FormControl className={classes.formControl}>
                <InputLabel id="demo-simple-select-label"><T id="gender"/></InputLabel>
                <Select value={selectedGender} onChange={handleGenderChange}>
                  {genders.models.map(model => (
                    <MenuItem key={model.genderId} value={model.genderId}>
                      {model.genderId === 0 ? (
                        <T id={model.genderName} />
                      ) : (
                        model.genderName
                      )}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            )}
            {/* FOLLOWING */}
            <Typography
              style={{ cursor: 'pointer' }}
              className={classes.typographyWithIcon}
              variant="subtitle1"
              onClick={() => setIsFollowingOpen(true)}
            >
              <RssFeedIcon className={classes.icon} />
              <T id="following" />
              {': '}
              {userProfile.model.followingCount}
            </Typography>
            <FollowingFollowersModal
              handleClose={() => setIsFollowingOpen(false)}
              isOpen={isFollowingOpen}
              mode="following"
              userId={userId}
            />
            <Typography
              className={classes.typographyWithIcon}
              style={{ cursor: 'pointer' }}
              variant="subtitle1"
              onClick={() => setIsFollowersOpen(true)}
            >
              <RecordVoiceOverIcon className={classes.icon} />
              <T id="followers" />
              {': '}
              {userProfile.model.followersCount}
            </Typography>
            <FollowingFollowersModal
              handleClose={() => setIsFollowersOpen(false)}
              isOpen={isFollowersOpen}
              mode="followers"
              userId={userId}
            />
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
            {/* -- BirthDay -- */}
            {isEditing && (
              <>
                <MuiPickersUtilsProvider
                  locale={langCode.includes('pl') ? plLocale : enLocale}
                  utils={DateFnsUtils}
                >
                  <KeyboardDatePicker
                    margin="normal"
                    id="date-picker-dialog"
                    label={<T id="birthDay" />}
                    format={
                      langCode.includes('pl') ? 'dd.MM.yyyy' : 'MM/dd/yyyy'
                    }
                    value={editBirthDate}
                    onChange={handleDateChange}
                    KeyboardButtonProps={{
                      'aria-label': 'change date',
                    }}
                  />
                </MuiPickersUtilsProvider>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={showBirthDay}
                      onChange={handleCheckBox('showBirthDay')}
                      value="showBirthDay"
                      color="primary"
                    />
                  }
                  label={<T id="showAge" />}
                />
              </>
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
                value={editAboutMe}
                onChange={handleAboutMeTextField}
                label={<T id="aboutMe" />}
                multiline={true}
                rows={7}
              />
            )}
          </Grid>
          {/*  User Posts */}
          <Divider className={classes.divider} />
          <Grid item={true} xs={12} className={classes.posts}>
            <Typography variant="h5">Recent Posts:</Typography>
            <CardSceleton />
          </Grid>
        </Grid>
      ) : (
        !isLoading && (
          <Typography variant="h4" style={{ textAlign: 'center' }}>
            <T id="noData" />
          </Typography>
        )
      )}
    </div>
  );
};

const mapStateToProps = (state: AppState) => ({
  user: state.user,
});

export default connect(mapStateToProps)(withLocalize(withRouter(UserProfile)));
