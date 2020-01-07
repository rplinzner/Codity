import React from 'react';
import { Menu, MenuItem } from '@material-ui/core';
import {
  Translate as T,
  LocalizeContextProps,
  withLocalize,
} from 'react-localize-redux';
import { UserActionTypes } from '../../store/user/user.types';
import User from '../../types/user';
import { withRouter, RouteComponentProps } from 'react-router-dom';

interface Props extends LocalizeContextProps {
  anchorEl: Element | ((element: Element) => Element) | null | undefined;
  id: string | undefined;
  isOpen: boolean;
  onClose: () => void;
  logOutAction: () => UserActionTypes;
  user: User | null;
}

const AccountMenu: React.FC<Props & RouteComponentProps> = (
  props: Props & RouteComponentProps,
) => {
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
      <MenuItem
        onClick={() => {
          props.onClose();
          props.history.push(
            `/profile?userId=${props.user ? props.user.id : ''}`,
          );
        }}
      >
        <T id="profile" />
      </MenuItem>
      <MenuItem
        onClick={() => {
          props.onClose();
          props.history.push('/MyAccount');
        }}
      >
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

export default withLocalize(withRouter(AccountMenu));
