import React from 'react';
import { Grid, Avatar, makeStyles, Typography } from '@material-ui/core';
import RecordVoiceOverIcon from '@material-ui/icons/RecordVoiceOver';

interface Props {}

const useStyles = makeStyles({
  avatar: {
    width: 200,
    height: 200,
    margin: '0 auto',
  },
  element: {
    marginTop: '10px',
  },
});

const UserProfile: React.FC<Props> = () => {
  const classes = useStyles();

  return (
    <Grid
      style={{ height: '90vh', padding: '20px' }}
      container
      justify="center"
      alignItems="center"
    >
      <Grid item sm={4} style={{ textAlign: 'center' }}>
        <Avatar
          src="https://s3.amazonaws.com/uifaces/faces/twitter/prrstn/128.jpg"
          className={classes.avatar}
        >
          RP
        </Avatar>
        <Typography className={classes.element} variant="subtitle1">
          <RecordVoiceOverIcon style={{padding: 'auto 0'}} />
          Followers: 69
        </Typography>
        <Typography variant="subtitle1">Following: 420</Typography>
      </Grid>
      <Grid item sm={5}>
        <Typography className={classes.element} variant="h4">
          Name and Last Name
        </Typography>
        <Typography className={classes.element} variant="h5">
          29 years old
        </Typography>
        <Typography className={classes.element} variant="h6">
          About me
        </Typography>
        <Typography variant="body1">
          Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quia,
          tempora voluptatum. Dicta inventore veritatis, laboriosam quisquam
          repudiandae est quia, aut dignissimos, cupiditate cumque repellat
          consequuntur itaque alias veniam saepe impedit.
        </Typography>
      </Grid>
    </Grid>
  );
};

export default UserProfile;
