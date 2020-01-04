import React from 'react';
import { Paper } from '@material-ui/core';

interface Props {
  newNotificationCallback: () => void;
  className: string;
}

const Notifications: React.FC<Props> = (props: Props) => {

  return (
    <div className={props.className}>
      <Paper />
    </div>
  );
};

export default Notifications;
