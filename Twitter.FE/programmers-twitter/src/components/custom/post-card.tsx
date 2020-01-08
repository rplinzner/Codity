import React, { useState } from 'react';
import {
  Card,
  CardHeader,
  CardActions,
  IconButton,
  CardContent,
  Typography,
  Badge,
  Tooltip,
  makeStyles,
  Theme,
  createStyles,
  Menu,
  MenuItem,
} from '@material-ui/core';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';

import SyntaxHighlighter from 'react-syntax-highlighter';
import { androidstudio } from 'react-syntax-highlighter/dist/esm/styles/hljs';

import FavoriteIcon from '@material-ui/icons/Favorite';
import CommentIcon from '@material-ui/icons/Comment';
import MoreVertIcon from '@material-ui/icons/MoreVert';
import GitHubIcon from '@material-ui/icons/GitHub';
import { red } from '@material-ui/core/colors';

import postService from '../../services/post.service';
import { Post } from '../../types/post';
import { UserAvatar } from '../containers/profile/index';
import { BaseResponse } from '../../types/base-response';
import * as constants from '../../constants/global.constats';
import displayErrors from '../../helpers/display-errors';
import deleteRequest from '../../services/delete.service';

interface Props extends LocalizeContextProps {
  post: Post;
  className?: string;
  updatePost: (arg1: number) => void;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    snippet: {
      marginTop: theme.spacing(3),
    },
  }),
);

const PostCard: React.FC<Props> = (props: Props) => {
  const [expanded, setExpanded] = useState(false);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const handleMenuClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const { post } = props;
  const langCode = props.activeLanguage ? props.activeLanguage.code : 'en';

  const classes = useStyles();

  const handleExpandClick = () => {
    setExpanded(!expanded);
  };

  const likePost = () => {
    console.log('like post');

    const data = { tweetId: post.id };
    postService<BaseResponse, typeof data>(
      data,
      constants.likeController,
      '',
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      () => props.updatePost(post.id),
      error => displayErrors(error),
    );
  };

  const dislikePost = () => {
    const data = { tweetId: post.id };
    deleteRequest(
      data,
      constants.likeController,
      '',
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      () => props.updatePost(post.id),
      error => displayErrors(error),
    );
  };

  const postDate = new Date(post.creationDate);
  return (
    <div className={props.className}>
      <Card>
        <CardHeader
          avatar={
            <UserAvatar
              firstName={post.authorFirstName}
              lastName={post.authorLastName}
              photo={post.authorImage}
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
                <MenuItem onClick={handleMenuClose}>Show User</MenuItem>
                <MenuItem onClick={handleMenuClose}>
                  Show post in new Window
                </MenuItem>
                <MenuItem onClick={handleMenuClose}>Delete post</MenuItem>
                <MenuItem onClick={handleMenuClose}>Edit post</MenuItem>
              </Menu>
            </div>
          }
          title={post.authorFirstName + ' ' + post.authorLastName}
          subheader={new Intl.DateTimeFormat(langCode, {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: '2-digit',
            hour: 'numeric',
            minute: 'numeric',
          }).format(postDate)}
        />

        <CardContent>
          <Typography variant="body2" color="textPrimary" component="p">
            {post.text}
          </Typography>
          <div className={classes.snippet}>
            <Typography variant="subtitle2" color="textSecondary">
              <T id="language" /> {': '}
              {post.codeSnippet.programmingLanguageName}
            </Typography>
            <SyntaxHighlighter
              style={androidstudio} // TODO: styles depending on theme
              language={post.codeSnippet.programmingLanguageName}
              showLineNumbers={true}
            >
              {post.codeSnippet.text}
            </SyntaxHighlighter>
          </div>
        </CardContent>
        <CardActions disableSpacing>
          <Tooltip title={<T id="giveKarma" />}>
            <IconButton
              onClick={() => {
                if (post.isLiked) {
                  dislikePost();
                  return;
                }
                likePost();
              }}
              aria-label="like"
            >
              <FavoriteIcon style={{ color: post.isLiked ? red[500] : '' }} />
            </IconButton>
          </Tooltip>

          <Tooltip title={<T id="showKarma" />}>
            <IconButton aria-label="like">
              <Typography variant="body1" color="textSecondary">
                {post.likesCount + ' Karma'}
              </Typography>
            </IconButton>
          </Tooltip>

          <Tooltip title={<T id="showComments" />}>
            <IconButton onClick={handleExpandClick} aria-label="comment">
              <Badge badgeContent={post.commentsCount} color="secondary">
                <CommentIcon />
              </Badge>
            </IconButton>
          </Tooltip>

          {post.codeSnippet.gistURL && (
            <IconButton aria-label="gist">
              <GitHubIcon />
            </IconButton>
          )}
        </CardActions>
      </Card>
    </div>
  );
};

export default withLocalize(PostCard);
