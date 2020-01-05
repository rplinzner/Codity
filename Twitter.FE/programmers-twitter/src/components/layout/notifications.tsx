import React from 'react';
import { Paper } from '@material-ui/core';
import { connect } from 'react-redux';

import { AppState } from '../..';
import { init } from '../../store/notifications/notifications.actions';
import { Notification } from '../../types/notification';

// import { HubConnectionBuilder } from '@microsoft/signalr';
// import * as constants from '../../constants/global.constats';

interface Props {
  newNotificationCallback?: () => void;
  className?: string;
  token: string | undefined;
  isConnected: boolean;
  notifications: Notification[];
  initAction: typeof init;
}

const Notifications: React.FC<Props> = (props: Props) => {
  if (!props.isConnected) {
    props.initAction();
  }
  return (
    <div className={props.className}>
      <Paper />
    </div>
  );
};

const mapStateToProps = (state: AppState) => ({
  isConnected: state.notifications.isConnectionOpen,
  notifications: state.notifications.notifications,
});

const mapDispatchToProps = (dispatch: any) => {
  return {
    initAction: () => dispatch(init()),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(Notifications);
