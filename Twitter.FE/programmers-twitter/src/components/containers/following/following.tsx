import React, { useState, useEffect } from 'react';
import { RouteComponentProps, withRouter } from 'react-router-dom';
import {
  withLocalize,
  Translate as T,
  LocalizeContextProps,
} from 'react-localize-redux';
import InfiniteScroll from 'react-infinite-scroller';

import get from '../../../services/get.service';
import { FollowingResponse } from '../../../types/following-response';
import * as constants from '../../../constants/global.constats';
import {
  LinearProgress,
  Typography,
  Grid,
  makeStyles,
  Theme,
  createStyles,
  CircularProgress,
} from '@material-ui/core';
import { UserCard } from '../additional/index';
import displayErrors from '../../../helpers/display-errors';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    marginTopBottom: {
      margin: theme.spacing(2, 0, 2, 0),
    },
    container: {
      padding: theme.spacing(2),
    },
  }),
);

interface Props extends RouteComponentProps {}

const Following: React.FC<Props & LocalizeContextProps> = (
  props: Props & LocalizeContextProps,
) => {
  const [isLoading, setIsLoading] = useState(false);
  const [profiles, setProfiles] = useState<FollowingResponse | null>(null);

  const pageSize = 10;

  const classes = useStyles();

  const getUrlParams = (): URLSearchParams => {
    if (!props.location.search) {
      return new URLSearchParams();
    }
    return new URLSearchParams(props.location.search);
  };

  const getUserIdSearchValue = (): string => {
    const search = getUrlParams();
    return search.get('userId') || '';
  };

  const userId = getUserIdSearchValue();
  const lang = props.activeLanguage ? props.activeLanguage.code : 'en';

  const getFollowing = (page: number = 1): void => {
    const id = userId;
    if (id !== '') {
      get<FollowingResponse>(
        constants.usersController,
        `/${id}/following?pageNumber=${page}&pageSize=${pageSize}`,
        lang,
        <T id="errorConnection" />,
        true,
      ).then(
        resp => {
          if (resp.currentPage === 1 || profiles === null) {
            setProfiles(resp);
          } else if (resp.totalCount > profiles.models.length) {
            const temp = resp;
            temp.models = [...profiles.models, ...temp.models];
            setProfiles(temp);
          } else {
            setProfiles(resp);
          }
          setIsLoading(false);
        },
        error => {
          displayErrors(error);
          setProfiles(null);
          setIsLoading(false);
        },
      );
    }
  };

  useEffect(() => {
    setIsLoading(true);
    getFollowing();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.location.search]);
  return (
    <>
      {isLoading ? (
        <LinearProgress className={classes.marginTopBottom} />
      ) : profiles === null ? (
        <div
          style={{ textAlign: 'center' }}
          className={classes.marginTopBottom}
        >
          <Typography variant="h4">
            <T id="noData" />
          </Typography>
        </div>
      ) : (
        <InfiniteScroll
          pageStart={1}
          hasMore={!(profiles.currentPage === profiles.totalPages)}
          loader={
            <div key="div" style={{ textAlign: 'center' }}>
              <CircularProgress key="spinner" />
            </div>
          }
          loadMore={page => getFollowing(page)}
          useWindow={true}
          threshold={50}
        >
          <Grid className={classes.container} container={true}>
            {profiles.models.map(model => (
              <Grid key={model.id} item={true} xs={12} md={6} lg={4}>
                <UserCard
                  key={model.id}
                  firstName={model.firstName}
                  followers={model.followersCount}
                  isFollowing={model.isFollowing}
                  lastName={model.lastName}
                  photo={model.image}
                  userId={model.id}
                  updateSearch={() => {}}
                  unfollowButton={false}
                />
              </Grid>
            ))}
          </Grid>
        </InfiniteScroll>
      )}
    </>
  );
};

export default withRouter(withLocalize(Following));
