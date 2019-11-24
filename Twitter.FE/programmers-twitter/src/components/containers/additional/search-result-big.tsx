import React from 'react';
import {
  Container,
  CardActionArea,
  // CardHeader,
  Card,
  Avatar,
  makeStyles,
  createStyles,
  Button,
  Theme,
  CardActions,
  CardContent,
  Typography,
  Grid,
} from '@material-ui/core';

interface Props {}
const avatar = (
  <Avatar
    style={{ margin: '0px 10px', width: '60px', height: '60px' }}
    aria-label="person"
  >
    MU
  </Avatar>
);

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      padding: theme.spacing(1, 0, 0, 1),
    },
  }),
);

const SearchResult: React.FC<Props> = () => {
  const classes = useStyles();

  const card = (
    <div className={classes.root}>
      <Card>
        <CardActionArea onClick={() => alert('you clicked')}>
          {/* <CardHeader
            avatar={avatar}
            // action={
            //   <Button style={{ zIndex: 2 }} variant="contained">
            //     Follow
            //   </Button>
            // }
          /> */}
          <CardContent>
            <Grid container alignItems="center">
              <Grid item>{avatar}</Grid>
              <Grid item>
                <Typography gutterBottom variant="h5" component="h2">
                  Mr Unknown
                </Typography>
                <Typography variant="body2" color="textSecondary" component="p">
                  69 followers
                </Typography>
              </Grid>
            </Grid>
          </CardContent>
        </CardActionArea>
        <CardActions>
          <Button style={{marginLeft: '10px'}} size="small" color="primary">
            Follow
          </Button>
        </CardActions>
      </Card>
    </div>
  );
  return (
    <Container>
      {card}
      {card}
      {card}
      {/* TODO: Do special card with follow button */}
    </Container>
  );
};
export default SearchResult;
