import React from 'react';
import { makeStyles, createStyles } from '@material-ui/core/styles';
import { Typography, Link, Paper, Theme } from '@material-ui/core';
import { Link as RouterLink } from 'react-router-dom';
import { withLocalize, Translate as T } from 'react-localize-redux';

import photo1 from '../../../assets/img/dog1.jpg';
import photo2 from '../../../assets/img/dog2.jpg';
import photo3 from '../../../assets/img/dog3.jpg';
import photo4 from '../../../assets/img/dog4.jpg';
import photo5 from '../../../assets/img/dog5.jpg';
import photo6 from '../../../assets/img/dog6.jpg';


interface Props {}

const data = [
  { img: photo1 },
  { img: photo2 },
  { img: photo3 },
  { img: photo4 },
  { img: photo5 },
  { img: photo6 },
];
const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    img: {
      background: `url(${data[Math.floor(Math.random() * data.length)].img})`,
      backgroundPosition: 'center',
      backgroundRepeat: 'no-repeat',
      backgroundSize: 'cover',
      color: 'black',
      [theme.breakpoints.up('md')]: {
        height: 'calc(100vh - 64px)',
      },
      [theme.breakpoints.down('md')]: {
        height: 'calc(100vh - 56px)',
      },
    },
    text: {
      zIndex: 1,
      position: 'relative',
      left: '2%',
      top: '15%',
      maxWidth: '70%',
    },
    paper: {
      backgroundColor: 'rgba(255, 255, 255, 0.5)',
      padding: '20px',
      marginRight: '10px',
    },
    typography: {
      marginTop: '7px',
      marginBottom: '7px',
    },
  }),
);

const NotFoundPage: React.FC<Props> = () => {
  const classes = useStyles();
  return (
    <>
      <div className={classes.img}>
        <div className={classes.text}>
          <Paper className={classes.paper}>
            <Typography className={classes.typography} variant="h1">
              <T id="did404" />
            </Typography>
            <Typography className={classes.typography} variant="h2">
              <T id="seemLost" />{' '}
              <span role="img" aria-label="grumpy-emoji">
                🤔
              </span>
            </Typography>
            <Typography className={classes.typography} variant="h3">
              <T id="doNotWorry" />{' '}
              <span role="img" aria-label="love-emoji">
                🥰
              </span>
            </Typography>
            <Typography className={classes.typography} variant="h4">
              <T id="whenReady" />{' '}
              <Link color="secondary" component={RouterLink} to="/">
                <T id="here" />
              </Link>
            </Typography>
          </Paper>
        </div>
      </div>
    </>
  );
};

export default withLocalize(NotFoundPage);
