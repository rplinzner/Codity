import React from 'react';
import { Card, CardHeader, CardContent, Theme } from '@material-ui/core';
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
  );
};
export default CardSkeleton;
