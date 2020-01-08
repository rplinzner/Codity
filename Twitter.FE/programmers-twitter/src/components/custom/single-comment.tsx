import React, { useState } from 'react';
import {
  CardHeader,
  CardContent,
  Typography,
  IconButton,
  Menu,
  MenuItem,
  TextField,
} from '@material-ui/core';
import MoreVertIcon from '@material-ui/icons/MoreVert';

import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';
import { RouteComponentProps, withRouter } from 'react-router-dom';

import { UserAvatar } from '../containers/profile/index';
import { connect } from 'react-redux';
import { AppState } from '../..';
import * as constants from '../../constants/global.constats';
import deleteRequest from '../../services/delete.service';
import put from '../../services/put.service';
import displayErrors from '../../helpers/display-errors';

interface Props extends LocalizeContextProps {
  authorFirstName: string;
  authorLastName: string;
  authorImage: string;
  authorId: number;
  commentDate: string;
  commentText: string;
  commentId: number;
  updateComments: () => void;
  // redux props
  userId: number | undefined;
}

const SingleComment: React.FC<Props & RouteComponentProps> = (
  props: Props & RouteComponentProps,
) => {
  const date = new Date(props.commentDate);
  const langCode = props.activeLanguage ? props.activeLanguage.code : 'en';
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [commentText, setCommentText] = useState(props.commentText);

  const handleMenuClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const removeComment = () => {
    deleteRequest(
      {},
      constants.commentController,
      `/${props.commentId}`,
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      () => props.updateComments(),
      error => displayErrors(error),
    );
  };

  const editComment = () => {
    const data = { text: commentText };
    setIsEditing(false);
    put(
      data,
      constants.commentController,
      `/${props.commentId}`,
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      () => {
        props.updateComments();
      },
      error => {
        displayErrors(error);
      },
    );
  };

  return (
    <>
      <CardHeader
        avatar={
          <UserAvatar
            firstName={props.authorFirstName}
            lastName={props.authorLastName}
            photo={props.authorImage}
          />
        }
        action={
          <div>
            <IconButton onClick={handleMenuClick} aria-label="more">
              <MoreVertIcon />
            </IconButton>
            <Menu
              id="simple-menu"
              anchorEl={anchorEl}
              keepMounted
              open={Boolean(anchorEl)}
              onClose={handleMenuClose}
            >
              <MenuItem
                onClick={() => {
                  props.history.push(`/profile?userId=${props.authorId}`);
                  handleMenuClose();
                }}
              >
                <T id="showUserProfile" />
              </MenuItem>
              {props.authorId === props.userId && (
                <div>
                  <MenuItem
                    onClick={() => {
                      setIsEditing(true);
                      handleMenuClose();
                    }}
                  >
                    <T id="editComment" />
                  </MenuItem>
                  <MenuItem
                    onClick={() => {
                      removeComment();
                      handleMenuClose();
                    }}
                  >
                    <T id="deleteComment" />
                  </MenuItem>
                </div>
              )}
            </Menu>
          </div>
        }
        title={props.authorFirstName + ' ' + props.authorLastName}
        subheader={new Intl.DateTimeFormat(langCode, {
          weekday: 'long',
          year: 'numeric',
          month: 'long',
          day: '2-digit',
          hour: 'numeric',
          minute: 'numeric',
        }).format(date)}
      />

      <CardContent>
        {!isEditing ? (
          <Typography variant="body2" color="textPrimary" component="p">
            {props.commentText}
          </Typography>
        ) : (
          <TextField
            onKeyPress={ev => {
              if (ev.key === 'Enter') {
                editComment();
                ev.preventDefault();
              }
            }}
            value={commentText}
            onChange={e => setCommentText(e.target.value)}
            multiline={true}
            label={<T id="writeComment" />}
            fullWidth={true}
          />
        )}
      </CardContent>
    </>
  );
};

const mapStateToProps = (state: AppState) => ({
  userId: state.user.details?.id,
});

export default connect(mapStateToProps)(
  withLocalize(withRouter(SingleComment)),
);
