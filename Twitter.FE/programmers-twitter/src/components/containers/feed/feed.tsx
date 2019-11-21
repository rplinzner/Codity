import React, { Component } from 'react';
import { withStyles, Theme } from '@material-ui/core/styles';
import Skeleton from '@material-ui/lab/Skeleton';
import { CardHeader, Card, CardContent, Grid } from '@material-ui/core';

interface Props {
  classes: {
    card: string;
    media: string;
  };
}
interface State {}

const styles = (theme: Theme) => ({
  card: {
    margin: theme.spacing(2),
  },
  media: {
    height: 190,
  },
});

class Feed extends Component<Props, State> {
  state = {};

  render() {
    const { classes } = this.props;
    const sketeton = (
      <Grid container justify="center">
        <Grid item sm={8} xs={12} md={6}>
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
    return (
      <>
        {sketeton}
        {sketeton}
      </>
    );
  }
}

export default withStyles(styles, { withTheme: true })(Feed);
