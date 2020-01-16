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
  FormControl,
  MenuItem,
  FormControlLabel,
  Checkbox,
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
import 'ace-builds/src-noconflict/theme-tomorrow_night';

import SyntaxHighlighter from 'react-syntax-highlighter';
import {
  vs2015,
  // githubGist,
} from 'react-syntax-highlighter/dist/esm/styles/hljs';

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
    snippetEdit: {
      marginTop: theme.spacing(3),
    },
    snippet: {
      marginTop: theme.spacing(3),
      maxHeight: 300,
      overflow: 'auto',
    },
    formControl: {
      margin: theme.spacing(1),
      minWidth: 120,
    },
  }),
);

const AddEditPost: React.FC<Props> = (props: Props) => {
  const classes = useStyles();
  const [page, setPage] = useState(1);
  const [description, setDescription] = useState('');
  const [programmingLang, setProgrammingLang] = useState('');
  const [snippet, setSnippet] = useState('');
  const [fileName, setFileName] = useState('snippet.txt');
  const [addToGists, setAddToGists] = useState(true);

  const languages = {
    csharp: 'cs',
    javascript: 'javascript',
    python: 'python',
    c_cpp: 'cpp',
    typescript: 'typescript',
    java: 'java',
  };

  const fileExt = {
    csharp: '.cs',
    javascript: '.js',
    python: '.py',
    c_cpp: '.cpp',
    typescript: '.ts',
    java: '.java',
  };

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const lang = event.target.value as string;
    setProgrammingLang(lang);
    const split = fileName.split('.');
    setFileName(split[0] + fileExt[lang]);
  };

  const onEditorChange = (newValue: string) => {
    setSnippet(newValue);
  };

  const handleAddToGists = (event: React.ChangeEvent<HTMLInputElement>) => {
    setAddToGists(event.target.checked);
  };

  const handleFileName = (newValue: string) => {
    setFileName(newValue);
  };

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
      <div className={classes.snippetEdit}>
        <FormControl className={classes.formControl}>
          <InputLabel htmlFor="age-native-simple">Język</InputLabel>
          <Select value={programmingLang} onChange={handleChange}>
            <MenuItem value="csharp">C#</MenuItem>
            <MenuItem value="javascript">JavaScript</MenuItem>
            <MenuItem value="c_cpp">C++</MenuItem>
            <MenuItem value="python">Python</MenuItem>
            <MenuItem value="typescript">Typescript</MenuItem>
            <MenuItem value="java">Java</MenuItem>
          </Select>
        </FormControl>
        <AceEditor
          style={{ width: '100%', height: '300px' }}
          placeholder="Place your code here"
          mode={programmingLang}
          theme="tomorrow_night"
          value={snippet}
          onChange={onEditorChange}
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

  const summaryPageContent = (
    <div>
      <Typography variant="body2" color="textPrimary" component="p">
        {description}
      </Typography>
      <div className={classes.snippet}>
        <SyntaxHighlighter
          style={vs2015}
          language={languages[programmingLang]}
          showLineNumbers={true}
        >
          {snippet}
        </SyntaxHighlighter>
      </div>
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
      case 3:
        return summaryPageContent;

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
            <FormControlLabel
              control={
                <Checkbox
                  color="primary"
                  checked={addToGists}
                  onChange={handleAddToGists}
                />
              }
              label="Add to your Gists"
            />
            {addToGists && (
              <TextField
                defaultValue={
                  programmingLang === '' ? '.txt' : fileExt[programmingLang]
                }
                className={classes.formControl}
                value={fileName}
                onChange={e => handleFileName(e.target.value)}
                label={'File Name'}
              />
            )}
          </CardActions>
        </Card>
      </div>
    </Container>
  );
};

export default AddEditPost;
