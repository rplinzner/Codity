import React from 'react';
import { Card, CardHeader, CardContent, Theme, Grid } from '@material-ui/core';
import { makeStyles, createStyles } from '@material-ui/styles';
import Skeleton from '@material-ui/lab/Skeleton';

interface Props {}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    card: {
      margin: theme.spacing(2),
    },
    media: {
      height: 190,
    },
  }),
);
const CardSkeleton: React.FC<Props> = () => {
  const classes = useStyles();

  return (
    <Grid container justify="center">
      <Grid item sm={8} xs={12} md={8} lg={5} xl={4}>
        <Card className={classes.card}>
          <CardHeader
            avatar={<Skeleton variant="circle" width={40} height={40} />}
            title={<Skeleton height={6} width="80%" />}
            subheader={<Skeleton height={6} width="40%" />}
          />
          <Skeleton variant="rect" className={classes.media} />
          <CardContent>
            <React.Fragment>
              <Skeleton height={6} />
              <Skeleton height={6} width="80%" />
            </React.Fragment>
          </CardContent>
        </Card>
      </Grid>
    </Grid>
  );
};
export default CardSkeleton;
