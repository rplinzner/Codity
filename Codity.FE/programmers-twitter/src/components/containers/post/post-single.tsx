import React, { useEffect, useState } from 'react';
import {
  makeStyles,
  Theme,
  createStyles,
  Container,
  Typography,
  LinearProgress,
} from '@material-ui/core';
import { RouteComponentProps, withRouter } from 'react-router-dom';
import {
  LocalizeContextProps,
  withLocalize,
  Translate as T,
} from 'react-localize-redux';
import { Post } from '../../../types/post';
import get from '../../../services/get.service';
import * as constants from '../../../constants/global.constats';
import { PostResponse } from '../../../types/post-response';
import displayErrors from '../../../helpers/display-errors';
import { PostCard } from '../../custom';

interface Props extends RouteComponentProps {}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    post: {
      margin: theme.spacing(1),
    },
    root: {
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2),
    },
  }),
);

const PostSingle: React.FC<Props & LocalizeContextProps> = (
  props: Props & LocalizeContextProps,
) => {
  const classes = useStyles();

  const [post, setPost] = useState<Post | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const getUrlParams = (): URLSearchParams => {
    if (!props.location.search) {
      return new URLSearchParams();
    }
    return new URLSearchParams(props.location.search);
  };

  const getPostIdSearchValue = (): string => {
    const search = getUrlParams();
    return search.get('postId') || '';
  };

  const langCode = props.activeLanguage ? props.activeLanguage.code : 'en';
  const postId = getPostIdSearchValue();

  const getPost = (id: number = 0) => {
    const finalId = id === 0 ? postId : id;
    setIsLoading(true);
    get<PostResponse>(
      constants.postController,
      `/${finalId}`,
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      resp => {
        setPost(resp.model);
        setIsLoading(false);
      },
      error => {
        setIsLoading(false);
        displayErrors(error);
      },
    );
  };

  const onPostDeleted = () => {
    props.history.push('MyFeed');
  };

  useEffect(() => {
    getPost();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.location.search]);

  return (
    <Container className={classes.root} maxWidth="md">
      {post ? (
        <PostCard
          onPostDeleted={onPostDeleted}
          post={post}
          updatePost={getPost}
          isSingle={true}
        />
      ) : isLoading ? (
        <LinearProgress />
      ) : (
        <div style={{ textAlign: 'center' }}>
          <Typography variant="h4">
            <T id="noData" />
          </Typography>
        </div>
      )}
    </Container>
  );
};

export default withLocalize(withRouter(PostSingle));
