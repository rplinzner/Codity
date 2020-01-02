import React, { useState } from 'react';
import { RouteComponentProps, withRouter } from 'react-router-dom';

import get from '../../../services/get.service';
import { FollowingResponse } from '../../../types/following-response';
import * as constants from '../../../constants/global.constats';

interface Props extends RouteComponentProps {}

const Following: React.FC<Props> = (props: Props) => {
  const [isLoading, setIsLoading] = useState(false);
  const [profiles, setProfiles] = useState<FollowingResponse | null>(null);

  const pageSize = 10;

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

  const getFollowing = (page: number = 1): void => {
    setIsLoading(true);
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
          setLoading(false);
        },
        error => {
          setProfiles(null);
          setLoading(false);
        },
      );
    }
  };

  return <div />;
};

export default withRouter(Following);
