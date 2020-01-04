import React, { useState, useEffect } from 'react';
import {
  Container,
  Typography,
  LinearProgress,
  makeStyles,
  createStyles,
  Theme,
} from '@material-ui/core';
import { withRouter, RouteComponentProps } from 'react-router';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';
import Pagination from 'material-ui-flat-pagination';
import ArrowBack from '@material-ui/icons/ArrowBack';
import ArrowForward from '@material-ui/icons/ArrowForward';

import SearchResponse from '../../../types/search-response';
import get from '../../../services/get.service';
import * as constants from '../../../constants/global.constats';
import SearchCard from './search-card';
import displayErrors from '../../../helpers/display-errors';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    container: {
      marginBottom: theme.spacing(2),
      marginTop: theme.spacing(2),
    },
    progress: {
      marginTop: theme.spacing(1),
    },
  }),
);

interface Props extends RouteComponentProps {}

const SearchResult: React.FC<Props & LocalizeContextProps> = (
  props: Props & LocalizeContextProps,
) => {
  const [profiles, setProfiles] = useState<SearchResponse | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [currentPage, setCurrentPage] = useState<number>(1);

  const getUrlParams = (): URLSearchParams => {
    if (!props.location.search) {
      return new URLSearchParams();
    }
    return new URLSearchParams(props.location.search);
  };

  const getNameSearchValue = (): string => {
    const search = getUrlParams();
    return search.get('search') || '';
  };

  const pageSize = 5;

  const getUsers = (page: number = 0): void => {
    let lang = 'en';
    if (props.activeLanguage) {
      lang = props.activeLanguage.code;
    }
    let current = page;
    const query = getNameSearchValue();
    if (page === 0) {
      current = currentPage;
    }

    if (query !== '') {
      get<SearchResponse>(
        constants.usersController,
        `/search?query=${query}&pageNumber=${current}&pageSize=${pageSize}`,
        lang,
        <T id="errorConnection" />,
        true,
      ).then(
        resp => {
          setProfiles(resp);
          setIsLoading(false);
        },
        error => {
          displayErrors(error);
          setIsLoading(false);
        },
      );
    } else {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    setIsLoading(true);
    getUsers();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.location.search]);

  const classes = useStyles();

  return (
    <Container className={classes.container}>
      {profiles && profiles.models && profiles.models.length > 0 ? (
        profiles.models.map(profile => (
          <SearchCard
            key={profile.id}
            firstName={profile.firstName}
            lastName={profile.lastName}
            followers={profile.followersCount}
            photo={profile.image}
            isFollowing={profile.isFollowing}
            userId={profile.id}
            updateSearch={getUsers}
          />
        ))
      ) : isLoading ? (
        <LinearProgress className={classes.progress} />
      ) : (
        <div style={{ textAlign: 'center' }}>
          <Typography variant="h5">
            <T id="noData" />
          </Typography>
        </div>
      )}
      {profiles && profiles.totalPages && profiles.totalPages > 1 && (
        <div className={classes.progress}>
          <Pagination
            limit={1}
            offset={currentPage - 1}
            total={profiles.totalPages}
            onClick={(e, o, page) => {
              setCurrentPage(page);
              getUsers(page);
            }}
            nextPageLabel={<ArrowForward fontSize="inherit" />}
            previousPageLabel={<ArrowBack fontSize="inherit" />}
            size="large"
          />
        </div>
      )}
    </Container>
  );
};
export default withLocalize(withRouter(SearchResult));
