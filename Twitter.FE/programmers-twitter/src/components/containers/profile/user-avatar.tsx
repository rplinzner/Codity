import React from 'react';
import { Avatar } from "@material-ui/core";
interface Props {
  firstName: string;
  lastName: string;
  photo: string | null;
}

const UserAvatar: React.FC<Props> = (props: Props) => {
  return (
    <Avatar
      style={{ margin: '0px 10px', width: '60px', height: '60px' }}
      aria-label="person"
      src={props.photo === null ? '' : props.photo}
    >
      {props.firstName[0].toLocaleUpperCase() + props.lastName[0].toLocaleUpperCase()}
    </Avatar>
  );
};

export default UserAvatar;
