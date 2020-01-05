import React from 'react';
import {
  Paper,
  Typography,
  Divider,
  makeStyles,
  createStyles,
  Theme,
  Grow,
} from '@material-ui/core';
import { connect } from 'react-redux';

import { AppState } from '../..';
import { Notification } from '../../types/notification';
import { withRouter, RouteComponentProps } from 'react-router-dom';

interface Props {
  closeNotifications: () => void;
  className?: string;
  isOpen: boolean;
  // redux props
  notifications: Notification[];
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    innerNotificationStyle: {
      padding: theme.spacing(1),
      '&:hover': {
        backgroundColor: theme.palette.action.hover,
      },
    },
    outerNotificationStyle: {
      cursor: 'pointer',
    },
  }),
);

const Notifications: React.FC<Props & RouteComponentProps> = (
  props: Props & RouteComponentProps,
) => {
  const classes = useStyles();

  const handleNotificationClicked = (el: Notification) => {
    props.history.push(el.redirectTo);
    props.closeNotifications();
  };

  return (
    <Grow in={props.isOpen}>
      <div className={props.className}>
        <Paper>
          {props.notifications.map((el, index) => (
            <div
              key={index}
              className={classes.outerNotificationStyle}
              onClick={() => handleNotificationClicked(el)}
            >
              <div className={classes.innerNotificationStyle}>
                <Typography variant="button">{el.label}</Typography>
                <Typography variant="body2">{el.description}</Typography>
              </div>
              <Divider />
            </div>
          ))}
        </Paper>
      </div>
    </Grow>
  );
};

const mapStateToProps = (state: AppState) => ({
  notifications: state.notifications.notifications,
});

export default connect(mapStateToProps)(withRouter(Notifications));
