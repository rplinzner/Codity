import React, { useState, useEffect } from 'react';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';

import { PostSkeleton, PostCard } from '../../custom/index';
import { PostsResponse } from '../../../types/posts-response';
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

interface Props extends LocalizeContextProps {}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    post: {
      margin: theme.spacing(1),
    },
  }),
);

const Feed: React.FC<Props> = (props: Props) => {
  const [postResponse, setPostResponse] = useState<PostsResponse | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const classes = useStyles();
  const pageSize = 5;

  const langCode = props.activeLanguage ? props.activeLanguage.code : 'en';

  const getLatestTweets = (page: number = 1) => {
    setIsLoading(true);
    get<PostsResponse>(
      constants.postController,
      `?pageNumber=${page}&pageSize=${pageSize}`,
      langCode,
      <T id="errorConnection" />,
      true,
    ).then(
      resp => {
        if (resp.currentPage === 1 || postResponse === null) {
          setPostResponse(resp);
        } else if (resp.totalCount > postResponse.models.length) {
          const temp = resp;
          temp.models = [...postResponse.models, ...temp.models];
          setPostResponse(temp);
        } else {
          setPostResponse(resp);
        }
        setIsLoading(false);
      },
      error => {
        displayErrors(error);
        setPostResponse(null);
        setIsLoading(false);
      },
    );
  };

  useEffect(() => {
    getLatestTweets();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <Container maxWidth="md">
      {/* LOADING */}
      <LinearProgress hidden={!isLoading} />
      {!postResponse && <PostSkeleton />}
      {/* NO DATA */}
      {postResponse && postResponse.models.length === 0 && (
        <Typography variant="h4">
          <T id="noData" />
        </Typography>
      )}
      {/* IS DATA */}
      {postResponse &&
        postResponse.models.length > 0 &&
        postResponse.models.map(item => (
          <PostCard className={classes.post} key={item.id} post={item} />
        ))}
    </Container>
  );
};

export default withLocalize(Feed);
