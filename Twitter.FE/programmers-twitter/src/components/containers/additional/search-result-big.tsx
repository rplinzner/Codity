import React, { useState, useEffect } from 'react';
import { Container, Typography } from '@material-ui/core';
import { withRouter, RouteComponentProps } from 'react-router';

import SearchResponse from '../../../types/search-response';
import get from '../../../services/get.service';
import * as constants from '../../../constants/global.constats';
import SearchCard from './search-card';
interface Props extends RouteComponentProps {}

const SearchResult: React.FC<Props> = (props: Props) => {
  const [profiles, setProfiles] = useState<SearchResponse | null>(null);

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
    const query = getNameSearchValue();
    if (query !== '') {
      get<SearchResponse>(
        `${constants.server}/api/User`,
        `/search?query=${query}`,
        'pl',
        'lol',
        true,
      ).then(resp => {
        setProfiles(resp);
      });
    }
  };

  useEffect(() => {
    getUsers();
  }, [props.location.search]);

  return (
    <Container>
      {profiles && profiles.models && profiles.models.length > 0 ? (
        profiles.models.map(profile => (
          <SearchCard
            key={profile.id}
            firstName={profile.firstName}
            lastName={profile.lastName}
            followers={profile.followersCount}
          />
        ))
      ) : (
        <Typography variant="h5">No data to show</Typography>
      )}
    </Container>
  );
};
export default withRouter(SearchResult);
