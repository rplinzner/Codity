import React, { useState } from 'react';

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
  TextField,
  CardContent,
  Button,
  InputLabel,
  Select,
} from '@material-ui/core';

import FavoriteIcon from '@material-ui/icons/Favorite';
import CommentIcon from '@material-ui/icons/Comment';
import GitHubIcon from '@material-ui/icons/GitHub';
import AceEditor from 'react-ace';
// import 'ace-builds/src-noconflict/mode-java';
import 'ace-builds/src-noconflict/mode-csharp';
import 'ace-builds/src-noconflict/mode-javascript';
import 'ace-builds/src-noconflict/mode-python';
import 'ace-builds/src-noconflict/mode-c_cpp';
import 'ace-builds/src-noconflict/mode-typescript';
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
    snippet: {
      marginTop: theme.spacing(3),
    },
  }),
);

const AddEditPost: React.FC<Props> = (props: Props) => {
  const classes = useStyles();
  const [page, setPage] = useState(1);
  const [description, setDescription] = useState('');

  // const languages = {
  //   cs: 'csharp',
  //   javascript: 'javascript',
  //   python: 'python',
  //   cpp: 'c_cpp',
  //   typescript: 'typescript',
  //   java: 'java',
  // };

  const descriptionPageDescription = (
    <div>
      <Typography variant="h4">Dodaj opis</Typography>
      <Typography variant="h5" color="textSecondary">
        W paru zdaniach opisz na czym polega twój post
      </Typography>
    </div>
  );

  const descriptionPageContent = (
    <TextField
      value={description}
      onChange={e => setDescription(e.target.value)}
      multiline={true}
      label={'Description'}
      fullWidth={true}
    />
  );

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
    <div>
      <Typography variant="body2" color="textPrimary" component="p">
        {description}
      </Typography>
      <div className={classes.snippet}>
        <InputLabel htmlFor="age-native-simple">Język</InputLabel>
        <Select>
          <option>C#</option>
          <option>JavaScript</option>
          <option>Python</option>
          <option>Typescript</option>
          <option>Java</option>
        </Select>
        <AceEditor
          style={{ width: '100%', height: '300px' }}
          placeholder="Place your code here"
          mode="csharp"
          theme="solarized_dark"
          onChange={onChange}
          editorProps={{ $blockScrolling: true }}
        />
      </div>
    </div>
  );

  const summaryPageDescription = (
    <div>
      <Typography variant="h4">Podsumowanie</Typography>
      <Typography variant="h5" color="textSecondary">
        Przejżyj swój post i zastanów się, czy chcesz go dodać do twoich Gistów
        na platformie Github (dostępne tylko jeśli posiadasz dodany token w
        profilu)
      </Typography>
    </div>
  );

  const getPageDescription = () => {
    switch (page) {
      case 1:
        return descriptionPageDescription;
      case 2:
        return codePageDescription;
      case 3:
        return summaryPageDescription;

      default:
        return null;
    }
  };

  const getPageContent = () => {
    switch (page) {
      case 1:
        return descriptionPageContent;
      case 2:
        return codePageContent;

      default:
        return null;
    }
  };

  const handlePrevButton = () => {
    if (page > 1) {
      setPage(page - 1);
    }
  };

  const handleNextButton = () => {
    if (page < 3) {
      setPage(page + 1);
    }
  };

  return (
    <Container className={classes.root} maxWidth="md">
      {getPageDescription()}
      <div className={classes.marginStandard}>
        {page !== 1 && (
          <Button
            className={classes.button}
            onClick={handlePrevButton}
            variant="contained"
            color="primary"
          >
            Poprzednia strona
          </Button>
        )}

        <Button
          onClick={handleNextButton}
          className={classes.button}
          variant="contained"
          color="primary"
        >
          {page !== 3 ? 'Następna strona' : 'Wyślij'}
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
          <CardContent> {getPageContent()}</CardContent>
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
