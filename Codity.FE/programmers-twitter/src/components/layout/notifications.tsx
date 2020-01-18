import React from 'react';
import {
  Paper,
  Typography,
  Divider,
  makeStyles,
  createStyles,
  Theme,
  Grow,
  Button,
  CircularProgress,
} from '@material-ui/core';
import { connect } from 'react-redux';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';
import { timeUnitsTranslations } from '../../translations/index';
import { withRouter, RouteComponentProps } from 'react-router-dom';

import { AppState } from '../..';
import { Notification } from '../../types/notification';
import { NotificationsResponse } from '../../types/notifications-response';

interface Props extends LocalizeContextProps {
  closeNotifications: () => void;
  getNotifications: (arg0: number) => void;
  oldNotifications: NotificationsResponse | null;
  className?: string;
  isOpen: boolean;
  isLoading: boolean;
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
    standardPadding: {
      padding: theme.spacing(1),
    },
  }),
);

const Notifications: React.FC<Props & RouteComponentProps> = (
  props: Props & RouteComponentProps,
) => {
  props.addTranslation(timeUnitsTranslations);

  const classes = useStyles();

  const handleNotificationClicked = (el: Notification) => {
    props.history.push(el.redirectTo);
    props.closeNotifications();
  };

  const { oldNotifications, getNotifications } = props;

  if (oldNotifications === null) {
    getNotifications(1);
  }

  const timeAgo = (date: Date, translate: any) => {
    const oneSecond = 1000;
    const oneMinute = oneSecond * 60;
    const oneHour = oneMinute * 60;
    const oneDay = oneHour * 24;
    const oneMonth = oneDay * 30;
    const now = new Date();

    const diffSeconds = Math.round(
      Math.abs((date.getTime() - now.getTime()) / oneSecond),
    );

    if (diffSeconds < 60) {
      return `${diffSeconds} ${
        diffSeconds === 1
          ? translate('second')
          : diffSeconds < 5
          ? translate('seconds1')
          : translate('seconds')
      } ${translate('ago')}`;
    }

    const diffMinutes = Math.round(
      Math.abs((date.getTime() - now.getTime()) / oneMinute),
    );
    if (diffMinutes < 60) {
      return `${diffMinutes} ${
        diffMinutes === 1
          ? translate('minute')
          : diffMinutes < 5
          ? translate('minutes1')
          : translate('minutes')
      } ${translate('ago')}`;
    }

    const diffHour = Math.round(
      Math.abs((date.getTime() - now.getTime()) / oneHour),
    );
    if (diffHour < 24) {
      return `${diffHour} ${
        diffHour === 1
          ? translate('hour')
          : diffHour < 5
          ? translate('hours1')
          : translate('hours')
      } ${translate('ago')}`;
    }

    const diffDays = Math.round(
      Math.abs((date.getTime() - now.getTime()) / oneDay),
    );
    if (diffDays < 30) {
      return `${diffDays} ${
        diffDays === 1 ? translate('day') : translate('days')
      } ${translate('ago')}`;
    }

    const diffMonths = Math.round(
      Math.abs((date.getTime() - now.getTime()) / oneMonth),
    );
    if (diffMonths < 12) {
      return `${diffMonths} ${
        diffMonths === 1
          ? translate('month')
          : diffMonths < 5
          ? translate('months1')
          : translate('months')
      } ${translate('ago')}`;
    }

    return translate('overYear');
  };

  const notification = (el: Notification, translate: any) => (
    <div
      key={el.id}
      className={classes.outerNotificationStyle}
      onClick={() => handleNotificationClicked(el)}
    >
      <div className={classes.innerNotificationStyle}>
        <Typography variant="h6">{el.label}</Typography>
        <Typography variant="subtitle1">
          {timeAgo(new Date(el.createdTime), translate)}
        </Typography>
        <Typography variant="body2">{el.description}</Typography>
      </div>
      <Divider />
    </div>
  );

  const hasMore = () => {
    if (oldNotifications) {
      return !(oldNotifications.currentPage === oldNotifications.totalPages);
    }
    return false;
  };

  return (
    <Grow in={props.isOpen}>
      <div style={{ overflow: 'auto' }} className={props.className}>
        <Paper elevation={3}>
          <T>
            {({ translate }) => (
              <div>
                {props.notifications.map(el => notification(el, translate))}

                {oldNotifications &&
                  oldNotifications.models.map(el =>
                    notification(el, translate),
                  )}

                {hasMore() === true && !props.isLoading && (
                  <div style={{ textAlign: 'center' }}>
                    <Button
                      style={{ width: '100%' }}
                      variant="text"
                      color="secondary"
                      onClick={() =>
                        getNotifications(
                          oldNotifications
                            ? oldNotifications.currentPage + 1
                            : 1,
                        )
                      }
                    >
                      <T id="loadMore" />
                    </Button>
                  </div>
                )}
                {props.isLoading && (
                  <div
                    style={{ textAlign: 'center' }}
                    className={classes.standardPadding}
                  >
                    <CircularProgress />
                  </div>
                )}

                {((!oldNotifications && props.notifications.length === 0) ||
                  (oldNotifications &&
                    oldNotifications.models.length === 0)) && (
                  <div className={classes.standardPadding}>
                    <Typography variant="button">
                      <T id="noData" />
                    </Typography>
                  </div>
                )}
              </div>
            )}
          </T>
        </Paper>
      </div>
    </Grow>
  );
};

const mapStateToProps = (state: AppState) => ({
  notifications: state.notifications.notifications,
});

export default connect(mapStateToProps)(
  withRouter(withLocalize(Notifications)),
);
