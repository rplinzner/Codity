import React, { useState } from 'react';
import {
  Menu,
  MenuItem,
  IconButton,
  Badge,
  Paper,
  Modal,
  Backdrop,
  makeStyles,
  Theme,
  createStyles,
  Button,
} from '@material-ui/core';
import {
  Translate as T,
  LocalizeContextProps,
  withLocalize,
} from 'react-localize-redux';
// import MailIcon from '@material-ui/icons/Mail';
import NotificationsIcon from '@material-ui/icons/Notifications';
import AccountCircle from '@material-ui/icons/AccountCircle';
import { NotificationsActionTypes } from '../../store/notifications/notifications.types';
import Notifications from './notifications';
import { NotificationsResponse } from '../../types/notifications-response';

interface Props extends LocalizeContextProps {
  anchorEl: Element | ((element: Element) => Element) | null | undefined;
  id: string | undefined;
  isOpen: boolean;
  onClose:
    | ((event: {}, reason: 'backdropClick' | 'escapeKeyDown') => void)
    | undefined;
  handleOpen: (event: React.MouseEvent<HTMLElement, MouseEvent>) => void;

  readNotificationAction: (arg1: boolean) => NotificationsActionTypes;
  isLoadingNotification: boolean;
  isNewNotification: boolean;
  notificationClassName: string;
  getNotifications: (arg1: number) => void;
  fetchedNotifications: NotificationsResponse | null;
}
const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    modal: {
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      outline: 0,
    },
  }),
);

const MobileAccountMenu: React.FC<Props> = (props: Props) => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const classes = useStyles();

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
      {/* <MenuItem>
        <IconButton aria-label="show 4 new mails" color="inherit">
          <Badge badgeContent={420} color="secondary">
            <MailIcon />
          </Badge>
        </IconButton>
        <p>
          <T id="messages" />
        </p>
      </MenuItem> */}

      <MenuItem
        onClick={e => {
          props.readNotificationAction(true);
          setIsModalOpen(!isModalOpen);
        }}
      >
        <IconButton aria-label="show 11 new notifications" color="inherit">
          <Badge
            invisible={!props.isNewNotification}
            variant="dot"
            color="secondary"
          >
            <NotificationsIcon />
          </Badge>
        </IconButton>
        <p>
          <T id="notifications" />
        </p>
      </MenuItem>
      <Modal
        className={classes.modal}
        open={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        closeAfterTransition={true}
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}
      >
        <Paper elevation={3}>
          <Notifications
            className={props.notificationClassName}
            closeNotifications={() => setIsModalOpen(false)}
            isOpen={isModalOpen}
            getNotifications={props.getNotifications}
            oldNotifications={props.fetchedNotifications}
            isLoading={props.isLoadingNotification}
          />
          <Button
            onClick={() => setIsModalOpen(false)}
            style={{ width: '100%' }}
            color="primary"
            variant="contained"
          >
            Close notifications
          </Button>
        </Paper>
      </Modal>
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
