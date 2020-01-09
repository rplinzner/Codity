import React from 'react';

import {
  Container,
  makeStyles,
  Theme,
  createStyles,
  Typography,
  Card,
  CardHeader,
  IconButton,
  CardActions,
  Badge,
  //   TextField,
  CardContent,
  Button,
} from '@material-ui/core';

import FavoriteIcon from '@material-ui/icons/Favorite';
import CommentIcon from '@material-ui/icons/Comment';
import GitHubIcon from '@material-ui/icons/GitHub';
import AceEditor from 'react-ace';
// import 'ace-builds/src-noconflict/mode-java';
import 'ace-builds/src-noconflict/mode-csharp';
// import 'ace-builds/src-noconflict/theme-github';
import 'ace-builds/src-noconflict/theme-solarized_dark';

import { UserAvatar } from '../profile';

interface Props {}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    marginStandard: {
      margin: theme.spacing(2, 0, 2, 0),
    },
    marginSmall: {
      margin: theme.spacing(0, 0, 1, 0),
    },

    root: {
      paddingTop: theme.spacing(2),
      paddingBottom: theme.spacing(2),
    },
    button: {
      marginRight: theme.spacing(1),
    },
  }),
);

const AddEditPost: React.FC<Props> = (props: Props) => {
  const classes = useStyles();

  //   const descriptionPageDescription = (
  //     <div>
  //       <Typography variant="h4">Dodaj opis</Typography>
  //       <Typography variant="h5" color="textSecondary">
  //         W paru zdaniach opisz na czym polega twój post
  //       </Typography>
  //     </div>
  //   );

  //   const descriptionPageContent = (
  //     <TextField
  //       // value={commentText}
  //       // onChange={e => setCommentText(e.target.value)}
  //       multiline={true}
  //       label={'Description'}
  //       fullWidth={true}
  //     />
  //   );

  const onChange = (newValue: string) => {
    console.log('change', newValue);
  };

  const codePageDescription = (
    <div>
      <Typography variant="h4">Dodaj snippet</Typography>
      <Typography variant="h5" color="textSecondary">
        Dodaj snippet kodu którym chcesz się pochwalić
      </Typography>
    </div>
  );

  const codePageContent = (
    <AceEditor
      style={{ width: '100%', height: '300px' }}
      placeholder="Place your code here"
      mode="csharp"
      theme="solarized_dark"
      onChange={onChange}
      editorProps={{ $blockScrolling: true }}
    />
  );

  return (
    <Container className={classes.root} maxWidth="md">
      {codePageDescription}
      <div className={classes.marginStandard}>
        <Button className={classes.button} variant="contained" color="primary">
          Poprzednia strona
        </Button>
        <Button className={classes.button} variant="contained" color="primary">
          Następna strona
        </Button>
      </div>

      <div className={classes.root}>
        <Typography className={classes.marginSmall} variant="body1">
          Twój post będzie wyglądać tak:
        </Typography>
        <Card>
          <CardHeader
            avatar={
              <UserAvatar firstName={'Your'} lastName={'Name'} photo={null} />
            }
            title={'Your Name'}
            subheader={'Now'}
          />
          <CardContent>{codePageContent}</CardContent>
          <CardActions disableSpacing={true}>
            <IconButton aria-label="like">
              <FavoriteIcon />
            </IconButton>

            <IconButton aria-label="comment">
              <Badge color="secondary">
                <CommentIcon />
              </Badge>
            </IconButton>

            <IconButton aria-label="gist">
              <GitHubIcon />
            </IconButton>
          </CardActions>
        </Card>
      </div>
    </Container>
  );
};

export default AddEditPost;
