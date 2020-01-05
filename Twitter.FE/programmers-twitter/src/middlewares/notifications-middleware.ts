import { HubConnectionBuilder } from '@microsoft/signalr';

import { INIT } from '../store/notifications/notifications.types';
import { addById } from '../store/notifications/notifications.actions';

const startSignalRConnection = (connection: any) =>
  connection
    .start()
    .then(() => {
      console.log('SignalR connection started');
    })
    .catch((error: any) => console.error('SignalR Connection Error: ', error));

const notificationsMiddlaware = (store: any) => (next: any) => (
  action: any,
) => {
  if (
    action.type === INIT &&
    store.getState().notifications.isConnectionOpen === false
  ) {
    const url = process.env.REACT_APP_SERVER + '/notificationHub';
    const token = store.getState().user.details.token;
    if (!token) return;
    // Create instance
    const connection = new HubConnectionBuilder()
      .withUrl(url, {
        accessTokenFactory: () => token,
      })
      .build();

    // event handlers
    connection.on('newNotification', id => store.dispatch(addById(id)));
    // re-establish the connection if connection dropped
    connection.onclose(() =>
      setTimeout(startSignalRConnection(connection), 5000),
    );

    startSignalRConnection(connection);
  }
  return next(action);
};

export default notificationsMiddlaware;
