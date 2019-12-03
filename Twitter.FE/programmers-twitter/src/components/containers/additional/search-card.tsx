import React from 'react';
import {
  Card,
  CardActionArea,
  CardContent,
  Grid,
  Typography,
  CardActions,
  Button,
  createStyles,
  makeStyles,
  Theme,
} from '@material-ui/core';
import { UserAvatar } from '../profile/index';
import {
  LocalizeContextProps,
  withLocalize,
  Translate as T,
} from 'react-localize-redux';
import post from '../../../services/post.service';
import del from '../../../services/delete.service';
import { usersController } from '../../../constants/global.constats';
import displayErrors from '../../../helpers/display-errors';
import { withRouter, RouteComponentProps } from 'react-router-dom';

interface Props extends LocalizeContextProps {
  firstName: string;
  lastName: string;
  followers: number;
  photo: string | null;
  isFollowing: boolean;
  userId: number;
  updateSearch: () => void;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      padding: theme.spacing(1, 0, 0, 1),
    },
    avatar: {
      margin: theme.spacing(0, 1),
      width: 60,
      height: 60,
    },
  }),
);

const SearchCard: React.FC<Props & RouteComponentProps> = (
  props: Props & RouteComponentProps,
) => {
  const classes = useStyles();

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
          props.updateSearch();
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
          props.updateSearch();
        },
        error => displayErrors(error),
      );
    }
  };

  return (
    <div className={classes.root}>
      <Card>
        <CardActionArea
          onClick={() => props.history.push(`/profile/?userId=${props.userId}`)}
        >
          <CardContent>
            <Grid container alignItems="center">
              <Grid item>
                <UserAvatar
                  firstName={props.firstName}
                  lastName={props.lastName}
                  photo={props.photo}
                  className={classes.avatar}
                />
              </Grid>
              <Grid item>
                <Typography gutterBottom variant="h5" component="h2">
                  {props.firstName + ' ' + props.lastName}
                </Typography>
                <Typography variant="body2" color="textSecondary" component="p">
                  {props.followers} <T id="followers" />
                </Typography>
              </Grid>
            </Grid>
          </CardContent>
        </CardActionArea>
        <CardActions>
          <Button
            onClick={handleFollow}
            style={{ marginLeft: '10px' }}
            size="small"
            color="primary"
          >
            {props.isFollowing ? <T id="unfollow" /> : <T id="follow" />}
          </Button>
        </CardActions>
      </Card>
    </div>
  );
};

export default withLocalize(withRouter(SearchCard));
