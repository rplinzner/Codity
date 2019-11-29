import React from 'react';
import { Avatar } from '@material-ui/core';
interface Props {
  firstName: string;
  lastName: string;
  photo: string | null;
  className?: string;
}

const UserAvatar: React.FC<Props> = (props: Props) => {
  return (
    <Avatar
      className={props.className || ''}
      aria-label="person"
      src={props.photo === null ? '' : props.photo}
    >
      {props.firstName[0].toLocaleUpperCase() +
        props.lastName[0].toLocaleUpperCase()}
    </Avatar>
  );
};

export default UserAvatar;
