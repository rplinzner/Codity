import React, { useState, useEffect } from 'react';
import { Container, Typography, LinearProgress } from '@material-ui/core';
import { withRouter, RouteComponentProps } from 'react-router';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';

import SearchResponse from '../../../types/search-response';
import get from '../../../services/get.service';
import * as constants from '../../../constants/global.constats';
import SearchCard from './search-card';
import displayErrors from '../../../helpers/display-errors';

interface Props extends RouteComponentProps {}

const SearchResult: React.FC<Props & LocalizeContextProps> = (
  props: Props & LocalizeContextProps,
) => {
  const [profiles, setProfiles] = useState<SearchResponse | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

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

  const getUsers = (): void => {
    let lang = 'en';
    if (props.activeLanguage) {
      lang = props.activeLanguage.code;
    }
    const query = getNameSearchValue();

    if (query !== '') {
      get<SearchResponse>(
        `${constants.usersController}`,
        `/search?query=${query}`,
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

  return (
    <Container>
      {profiles && profiles.models && profiles.models.length > 0 ? (
        profiles.models.map((
          profile, //TODO: Add pagination
        ) => (
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
      ) : (
        <div style={{ textAlign: 'center' }}>
          <Typography variant="h5">
            <T id="noData" />
          </Typography>
        </div>
      )}
      {isLoading && <LinearProgress style={{ marginTop: '8px' }} />}
    </Container>
  );
};
export default withLocalize(withRouter(SearchResult));
