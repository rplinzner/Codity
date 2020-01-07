import React from 'react';
import {
  Container,
  Paper,
  makeStyles,
  Theme,
  createStyles,
  Typography,
  Divider,
  Grid,
  Button,
  Backdrop,
  Modal,
  TextField,
  LinearProgress,
} from '@material-ui/core';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';
import { connect } from 'react-redux';

import ThemeSwitch from '../../controls/theme-switch';
import LanguageSelector from '../../controls/language-selector';

import { AppState } from '../../..';
import post from '../../../services/post.service';
import put from '../../../services/put.service';
import { BaseResponse } from '../../../types/base-response';
import * as constants from '../../../constants/global.constats';
import { setHasGithubToken } from '../../../store/settings/settings.actions';
import { toast } from 'react-toastify';
import displayErrors from '../../../helpers/display-errors';
import deleteRequest from '../../../services/delete.service';

interface Props extends LocalizeContextProps {
  // REDUX props
  isDarkTheme: boolean;
  isTokenAdded: boolean;
  setHasGithubTokenAction: typeof setHasGithubToken;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    paper: {
      padding: theme.spacing(2),
      marginTop: theme.spacing(2),
    },
    containerRoot: {
      padding: theme.spacing(2),
    },
    item: {
      padding: theme.spacing(1),
    },
    modal: {
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      outline: 'none',
    },
    modalContainer: {
      padding: theme.spacing(1),
      margin: theme.spacing(1),
      width: '100%',
      maxWidth: 400,
    },
  }),
);

const Account: React.FC<Props> = (props: Props) => {
  const classes = useStyles();
  const [open, setOpen] = React.useState(false);
  const [token, setToken] = React.useState('');
  const [isLoading, setIsLoading] = React.useState(false);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const lang = props.activeLanguage ? props.activeLanguage.code : 'en';

  const setGhToken = () => {
    setIsLoading(true);
    handleClose();
    const data = { token: token };
    post<BaseResponse, typeof data>(
      data,
      constants.settingsController,
      '/personal_access_token',
      lang,
      <T id="errorConnection" />,
      true,
    ).then(
      res => {
        if (!res.isError) {
          props.setHasGithubTokenAction(true);
          toast.success(<T id="githubTokenSet" />);
          setIsLoading(false);
        }
      },
      errors => {
        displayErrors(errors);
        setIsLoading(false);
      },
    );
  };

  const deleteGhToken = () => {
    setIsLoading(true);
    deleteRequest(
      {},
      constants.settingsController,
      '/personal_access_token',
      lang,
      <T id="errorConnection" />,
      true,
    ).then(
      res => {
        if (!res.isError) {
          props.setHasGithubTokenAction(false);
          toast.success(<T id="githubTokenRemove" />);
          setIsLoading(false);
        }
      },
      errors => {
        displayErrors(errors);
        setIsLoading(false);
      },
    );
  };

  const updateSettings = () => {
    setIsLoading(true);
    const data = {
      isDarkTheme: props.isDarkTheme,
      languageCode: lang,
    };
    put<BaseResponse, typeof data>(
      data,
      constants.settingsController,
      '',
      lang,
      <T id="errorConnection" />,
      true,
    ).then(
      res => {
        if (!res.isError) {
          toast.success(<T id="settingsApplied" />);
          setIsLoading(false);
        }
      },
      errors => {
        displayErrors(errors);
        setIsLoading(false);
      },
    );
  };

  return (
    <Container className={classes.containerRoot} maxWidth="xs">
      <Typography variant="h2">
        <T id="settings" />
      </Typography>
      <Paper elevation={3}>
        <div className={classes.paper}>
          <LinearProgress hidden={!isLoading} />

          {/* BASIC SETTINGS */}
          <Typography className={classes.item} variant="h5">
            <T id="basic" />
          </Typography>
          <div className={classes.item}>
            <ThemeSwitch />
          </div>
          <div className={classes.item}>
            <LanguageSelector />
          </div>
          <div className={classes.item}>
            <Button
              onClick={updateSettings}
              variant="contained"
              color="primary"
            >
              <T id="save" />
            </Button>
          </div>
        </div>
        <Divider variant="fullWidth" />
        {/* ADDITIONAL SETTINGS */}
        <div className={classes.item}>
          <Typography className={classes.item} variant="h5">
            <T id="additional" />
          </Typography>
          <Typography className={classes.item} variant="body1">
            <T id="githubTokenState" />
            {': '}
            {props.isTokenAdded ? (
              <span style={{ color: 'green', fontWeight: 'bold' }}>
                <T id="added" />
              </span>
            ) : (
              <span style={{ color: 'red', fontWeight: 'bold' }}>
                <T id="noToken" />
              </span>
            )}
          </Typography>
          <Grid container>
            <Grid item className={classes.item}>
              <Button onClick={handleOpen} variant="contained" color="primary">
                <T id="addNew" />
              </Button>
            </Grid>
            <Grid item className={classes.item}>
              {props.isTokenAdded && (
                <Button
                  onClick={deleteGhToken}
                  variant="contained"
                  color="secondary"
                >
                  <T id="deleteToken" />
                </Button>
              )}
            </Grid>
          </Grid>
          <Modal
            className={classes.modal}
            open={open}
            onClose={handleClose}
            closeAfterTransition
            BackdropComponent={Backdrop}
            BackdropProps={{
              timeout: 500,
            }}
          >
            <Paper className={classes.modalContainer}>
              <div className={classes.item}>
                <TextField
                  value={token}
                  onChange={e => setToken(e.target.value)}
                  label={<T id="githubToken" />}
                  fullWidth={true}
                />
              </div>

              <Grid container>
                <Grid item className={classes.item}>
                  <Button
                    onClick={setGhToken}
                    variant="contained"
                    color="primary"
                  >
                    <T id="save" />
                  </Button>
                </Grid>
                <Grid item className={classes.item}>
                  <Button
                    onClick={handleClose}
                    variant="contained"
                    color="secondary"
                  >
                    <T id="cancel" />
                  </Button>
                </Grid>
              </Grid>
            </Paper>
          </Modal>
        </div>
        <Divider variant="fullWidth" />
        {/* SECURITY STUFF */}

        <div className={classes.item}>
          <Typography className={classes.item} variant="h5">
            <T id="security" />
          </Typography>
        </div>
      </Paper>
    </Container>
  );
};

const mapStateToProps = (state: AppState) => ({
  isTokenAdded: state.settings.hasGithubToken,
  isDarkTheme: state.settings.isDarkTheme,
});

const mapDispatchToProps = (dispatch: any) => {
  return {
    setHasGithubTokenAction: (hasToken: boolean) =>
      dispatch(setHasGithubToken(hasToken)),
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(withLocalize(Account));
