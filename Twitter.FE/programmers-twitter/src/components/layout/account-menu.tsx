import React from 'react';
import { Menu, MenuItem } from '@material-ui/core';
import {
  Translate as T,
  LocalizeContextProps,
  withLocalize,
} from 'react-localize-redux';
import { UserActionTypes } from '../../store/user/user.types';

interface Props extends LocalizeContextProps {
  anchorEl: Element | ((element: Element) => Element) | null | undefined;
  id: string | undefined;
  isOpen: boolean;
  onClose: () => void;
  logOutAction: () => UserActionTypes;
}

const AccountMenu: React.FC<Props> = (props: Props) => {
  return (
    <Menu
      anchorEl={props.anchorEl}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
      id={props.id}
      keepMounted={true}
      transformOrigin={{ vertical: 'top', horizontal: 'right' }}
      open={props.isOpen}
      onClose={props.onClose}
    >
      <MenuItem onClick={props.onClose}>
        <T id="profile" />
      </MenuItem>
      <MenuItem onClick={props.onClose}>
        <T id="myAccount" />
      </MenuItem>
      <MenuItem
        onClick={() => {
          props.onClose();
          props.logOutAction();
        }}
      >
        <T id="logout" />
      </MenuItem>
    </Menu>
  );
};

export default withLocalize(AccountMenu);
