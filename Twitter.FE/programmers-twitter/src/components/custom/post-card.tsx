import React, { useState, useEffect } from 'react';
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
  Collapse,
  Divider,
  Button,
  TextField,
  CircularProgress,
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
import { UserAvatar } from '../containers/profile/';
import { CommentsResponse } from '../../types/comments-response';
import { CommentResponse } from '../../types/comment-response';
import SingleComment from './single-comment';
import * as constants from '../../constants/global.constats';
import displayErrors from '../../helpers/display-errors';
import deleteRequest from '../../services/delete.service';
import get from '../../services/get.service';
import { programmingLanguagesTranslations } from '../../translations';

interface Props extends LocalizeContextProps {
  post: Post;
  className?: string;
  updatePost: (arg1: number) => void;
  commentsOpen?: boolean;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    snippet: {
      marginTop: theme.spacing(3),
      maxHeight: 300,
      overflow: 'auto',
    },
    commentBox: {
      padding: theme.spacing(2),
      width: '100%',
    },
  }),
);

const PostCard: React.FC<Props> = (props: Props) => {
  const [expanded, setExpanded] = useState(props.commentsOpen || false);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [
    commentsResponse,
    setCommentsResponse,
  ] = useState<CommentsResponse | null>(null);
  const [commentText, setCommentText] = useState('');
  const [isCommentAdding, setIsCommentAdding] = useState(false);

  let commentNumber = 3;
  if (props.commentsOpen) {
    commentNumber = props.commentsOpen ? 7 : 3;
  }

  const handleMenuClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const { post } = props;
  const langCode = props.activeLanguage ? props.activeLanguage.code : 'en';
  props.addTranslation(programmingLanguagesTranslations);

  const classes = useStyles();

  const handleExpandClick = () => {
    setExpanded(!expanded);
  };

  const likePost = () => {
    const data = { tweetId: post.id };
    postService(
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

  const getComments = () => {
    get<CommentsResponse>(
      constants.postController,
      `/${post.id}/comment/?pageNumber=1&pageSize=${commentNumber}`,
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      resp => {
        setCommentsResponse(resp);
      },
      error => displayErrors(error),
    );
  };

  const addComment = () => {
    setIsCommentAdding(true);
    const data = { tweetId: post.id, text: commentText };
    setCommentText('');
    postService<CommentResponse, typeof data>(
      data,
      constants.commentController,
      '',
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      resp => {
        const temp = { ...commentsResponse } as CommentsResponse;
        temp.models = [resp.model, ...temp.models];
        setCommentsResponse(temp);
        setIsCommentAdding(false);
        props.updatePost(post.id);
      },
      error => {
        displayErrors(error);
        setIsCommentAdding(false);
      },
    );
  };

  useEffect(() => {
    getComments();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

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
                keepMounted={true}
                open={Boolean(anchorEl)}
                onClose={handleMenuClose}
              >
                <MenuItem onClick={handleMenuClose}>
                  <T id="showUserProfile" />
                </MenuItem>
                <MenuItem onClick={handleMenuClose}>
                  <T id="showPostNewWindow" />
                </MenuItem>
                <MenuItem onClick={handleMenuClose}>
                  <T id="deletePost" />
                </MenuItem>
                <MenuItem onClick={handleMenuClose}>
                  <T id="editPost" />
                </MenuItem>
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
              <T id={post.codeSnippet.programmingLanguageName} />
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
        <CardActions disableSpacing={true}>
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

          <Tooltip
            title={
              !expanded ? <T id="showComments" /> : <T id="hideComments" />
            }
          >
            <IconButton onClick={handleExpandClick} aria-label="comment">
              <Badge badgeContent={post.commentsCount} color="secondary">
                <CommentIcon />
              </Badge>
            </IconButton>
          </Tooltip>

          {post.codeSnippet.gistURL && (
            <Tooltip title={<T id="showGist" />}>
              <IconButton aria-label="gist">
                <GitHubIcon />
                {/* TODO: Add translation */}
              </IconButton>
            </Tooltip>
          )}
        </CardActions>
        <Collapse in={expanded} timeout="auto" unmountOnExit={true}>
          <Divider />
          <div className={classes.commentBox}>
            {isCommentAdding ? (
              <CircularProgress />
            ) : (
              <TextField
                onKeyPress={ev => {
                  if (ev.key === 'Enter') {
                    addComment();
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
          </div>
          {commentsResponse &&
            commentsResponse.models &&
            commentsResponse.models.length > 0 &&
            commentsResponse.models.map(item => (
              <div key={item.id}>
                <Divider />
                <SingleComment
                  authorFirstName={item.authorFirstName}
                  authorLastName={item.authorLastName}
                  authorImage={item.authorImage}
                  commentDate={item.creationDate}
                  commentText={item.text}
                />
              </div>
            ))}
          {commentsResponse &&
            !(commentsResponse.currentPage === commentsResponse.totalPages) && (
              <div>
                <Divider />
                <Button
                  style={{ width: '100%' }}
                  variant="text"
                  color="secondary"
                >
                  <T id="loadMore" />
                </Button>
              </div>
            )}
        </Collapse>
      </Card>
    </div>
  );
};

export default withLocalize(PostCard);
