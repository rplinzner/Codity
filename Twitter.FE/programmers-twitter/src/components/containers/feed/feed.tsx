import React, { useState, useEffect } from 'react';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';

import { PostSkeleton, PostCard } from '../../custom/index';
import { PostsResponse } from '../../../types/posts-response';
import { PostResponse } from '../../../types/post-response';
import {
  Container,
  LinearProgress,
  Typography,
  makeStyles,
  Theme,
  createStyles,
} from '@material-ui/core';
import get from '../../../services/get.service';
import * as constants from '../../../constants/global.constats';
import displayErrors from '../../../helpers/display-errors';
import { toast } from 'react-toastify';
import { Post } from '../../../types/post';

interface Props extends LocalizeContextProps {}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    post: {
      margin: theme.spacing(1),
    },
    root: {
      marginTop: theme.spacing(2),
    },
  }),
);

const Feed: React.FC<Props> = (props: Props) => {
  const [posts, setPosts] = useState<Post[] | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const classes = useStyles();
  const pageSize = 5;

  const langCode = props.activeLanguage ? props.activeLanguage.code : 'en';

  const getLatestPosts = (page: number = 1) => {
    setIsLoading(true);
    get<PostsResponse>(
      constants.postController,
      `?pageNumber=${page}&pageSize=${pageSize}`,
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      resp => {
        if (resp.currentPage === 1 || posts === null) {
          setPosts(resp.models);
        } else if (resp.totalCount > posts.length) {
          const temp = [...posts, ...resp.models];
          setPosts(temp);
        } else {
          setPosts(resp.models);
        }
        setIsLoading(false);
      },
      error => {
        displayErrors(error);
        setPosts(null);
        setIsLoading(false);
      },
    );
  };

  const updatePost = (postId: number) => {
    console.log('update post');

    get<PostResponse>(
      constants.postController,
      `/${postId}`,
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      resp => {
        console.log(resp);

        if (posts === null) {
          toast.error('No data to update');
          return;
        }
        const index = posts.findIndex(e => e.id === postId);
        const temp = posts.map((item, iIndex) => {
          if (iIndex !== index) {
            return item;
          }
          return {
            ...item,
            ...resp.model,
          };
        });
        setPosts(temp);
        console.log(posts);
      },
      error => displayErrors(error),
    );
  };

  useEffect(() => {
    getLatestPosts();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <Container className={classes.root} maxWidth="md">
      {/* LOADING */}
      <LinearProgress hidden={!isLoading} />
      {!posts && <PostSkeleton />}
      {/* NO DATA */}
      {posts && posts.length === 0 && (
        <Typography variant="h4">
          <T id="noData" />
        </Typography>
      )}
      {/* IS DATA */}
      {posts &&
        posts.length > 0 &&
        posts.map(item => (
          <PostCard
            updatePost={updatePost}
            className={classes.post}
            key={item.id}
            post={item}
          />
        ))}
    </Container>
  );
};

export default withLocalize(Feed);
