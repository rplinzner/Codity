import React from 'react';
import { Menu, MenuItem, IconButton, Badge } from '@material-ui/core';
import {
  Translate as T,
  LocalizeContextProps,
  withLocalize,
} from 'react-localize-redux';
import MailIcon from '@material-ui/icons/Mail';
import NotificationsIcon from '@material-ui/icons/Notifications';
import AccountCircle from '@material-ui/icons/AccountCircle';

interface Props extends LocalizeContextProps {
  anchorEl: Element | ((element: Element) => Element) | null | undefined;
  id: string | undefined;
  isOpen: boolean;
  onClose:
    | ((event: {}, reason: 'backdropClick' | 'escapeKeyDown') => void)
    | undefined;
  handleOpen: (event: React.MouseEvent<HTMLElement, MouseEvent>) => void;
}

const MobileAccountMenu: React.FC<Props> = (props: Props) => {
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
      <MenuItem>
        <IconButton aria-label="show 4 new mails" color="inherit">
          <Badge badgeContent={420} color="secondary">
            <MailIcon />
          </Badge>
        </IconButton>
        <p>
          <T id="messages" />
        </p>
      </MenuItem>
      <MenuItem>
        <IconButton aria-label="show 11 new notifications" color="inherit">
          <Badge badgeContent={69} color="secondary">
            <NotificationsIcon />
          </Badge>
        </IconButton>
        <p>
          <T id="notifications" />
        </p>
      </MenuItem>
      <MenuItem onClick={props.handleOpen}>
        <IconButton
          aria-label="account of current user"
          aria-controls="primary-search-account-menu"
          aria-haspopup="true"
          color="inherit"
        >
          <AccountCircle />
        </IconButton>
        <p>
          <T id="profile" />
        </p>
      </MenuItem>
    </Menu>
  );
};

export default withLocalize(MobileAccountMenu);
