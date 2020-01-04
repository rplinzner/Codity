import React, { useState, useEffect } from 'react';
import { makeStyles, Theme, createStyles } from '@material-ui/core/styles';
import Modal from '@material-ui/core/Modal';
import Backdrop from '@material-ui/core/Backdrop';
import Fade from '@material-ui/core/Fade';
import InfiniteScroll from 'react-infinite-scroller';

import { FollowingResponse } from '../../../types/following-response';
import get from '../../../services/get.service';
import * as constants from '../../../constants/global.constats';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';
import { Typography, CircularProgress } from '@material-ui/core';
import { UserCardSimple } from '../additional/index';

interface Props extends LocalizeContextProps {
  isOpen: boolean;
  handleClose: () => void;
  userId: string;
  mode: 'following' | 'followers';
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    modal: {
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
    },
    paper: {
      backgroundColor: theme.palette.background.paper,
      border: '2px solid #000',
      boxShadow: theme.shadows[5],
      padding: theme.spacing(2),
      outline: 0,
    },
    card: {},
    item: {
      margin: theme.spacing(1, 1, 1, 0),
    },
    scrollable: {
      height: 'auto',
      maxHeight: '50vh',
      overflow: 'auto',
    },
  }),
);

function FollowingFollowersModal(props: Props) {
  const classes = useStyles();
  const [profiles, setProfiles] = useState<FollowingResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const pageSize = 5;

  const lang = props.activeLanguage.code;

  const getFollowing = (page: number = 1): void => {
    if (profiles !== null && page > profiles.totalPages) {
      return;
    }
    get<FollowingResponse>(
      constants.usersController,
      `/${props.userId}/${props.mode}?pageNumber=${page}&pageSize=${pageSize}`,
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
      () => {
        setProfiles(null);
        setLoading(false);
      },
    );
  };

  useEffect(() => {
    getFollowing();
    setLoading(true);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.userId, props.mode, props.isOpen]);

  return (
    <>
      <Modal
        aria-labelledby="transition-modal-title"
        className={classes.modal}
        open={props.isOpen}
        onClose={props.handleClose}
        closeAfterTransition={true}
        BackdropComponent={Backdrop}
        BackdropProps={{
          timeout: 500,
        }}
      >
        <Fade in={props.isOpen}>
          <div className={classes.paper}>
            {loading ? (
              <div style={{ textAlign: 'center' }}>
                <CircularProgress />
              </div>
            ) : profiles === null ? (
              <Typography variant="h5">
                <T id="noData" />
              </Typography>
            ) : (
              <div id="scrollableDiv" className={classes.scrollable}>
                <InfiniteScroll
                  pageStart={1}
                  hasMore={!(profiles.currentPage === profiles.totalPages)}
                  loader={
                    <Typography variant="caption" key="h4">
                      <T id="loading" />
                    </Typography>
                  }
                  loadMore={page => getFollowing(page)}
                  useWindow={false}
                  threshold={20}
                >
                  {profiles.models.map((model, index) => (
                    <div key={model.id} className={classes.item}>
                      <UserCardSimple
                        key={index}
                        firstName={model.firstName}
                        lastName={model.lastName}
                        followers={model.followersCount}
                        photo={model.image}
                        userId={model.id}
                        handleModalClose={props.handleClose}
                        className={classes.card}
                      />
                    </div>
                  ))}
                </InfiniteScroll>
              </div>
            )}
          </div>
        </Fade>
      </Modal>
    </>
  );
}

export default withLocalize(FollowingFollowersModal);
