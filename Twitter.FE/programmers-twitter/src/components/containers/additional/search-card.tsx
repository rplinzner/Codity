import React from 'react';
import {
  Card,
  CardActionArea,
  CardContent,
  Grid,
  Typography,
  CardActions,
  Button,
  createStyles,
  makeStyles,
  Theme,
} from '@material-ui/core';
import { UserAvatar } from '../profile/index';

interface Props {
  firstName: string;
  lastName: string;
  followers: number;
  photo: string | null;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      padding: theme.spacing(1, 0, 0, 1),
    },
  }),
);

const SearchCard: React.FC<Props> = (props: Props) => {
  const classes = useStyles();
  return (
    <div className={classes.root}>
      <Card>
        <CardActionArea onClick={() => alert('you clicked')}>
          <CardContent>
            <Grid container alignItems="center">
              <Grid item>
                <UserAvatar firstName={props.firstName} lastName={props.lastName} photo={props.photo} />
              </Grid>
              <Grid item>
                <Typography gutterBottom variant="h5" component="h2">
                  {props.firstName + ' ' + props.lastName}
                </Typography>
                <Typography variant="body2" color="textSecondary" component="p">
                  {props.followers} followers
                </Typography>
              </Grid>
            </Grid>
          </CardContent>
        </CardActionArea>
        <CardActions>
          <Button style={{ marginLeft: '10px' }} size="small" color="primary">
            Follow
          </Button>
        </CardActions>
      </Card>
    </div>
  );
};

export default SearchCard;
