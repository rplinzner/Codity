import React, { useState, useEffect } from 'react';

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
  LinearProgress,
} from '@material-ui/core';

import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';
import { connect } from 'react-redux';
import { RouteComponentProps, withRouter } from 'react-router-dom';

import FavoriteIcon from '@material-ui/icons/Favorite';
import CommentIcon from '@material-ui/icons/Comment';
import GitHubIcon from '@material-ui/icons/GitHub';
import AceEditor from 'react-ace';
import 'ace-builds/src-noconflict/mode-java';
import 'ace-builds/src-noconflict/mode-csharp';
import 'ace-builds/src-noconflict/mode-javascript';
import 'ace-builds/src-noconflict/mode-python';
import 'ace-builds/src-noconflict/mode-c_cpp';
import 'ace-builds/src-noconflict/mode-typescript';
import 'ace-builds/src-noconflict/theme-github';
import 'ace-builds/src-noconflict/theme-tomorrow_night';

import SyntaxHighlighter from 'react-syntax-highlighter';
import {
  vs2015,
  githubGist,
} from 'react-syntax-highlighter/dist/esm/styles/hljs';

import { UserAvatar } from '../profile';
import { AppState } from '../../..';
import get from '../../../services/get.service';
import {
  ProgrammingLanguageResponse,
  Programminglanguage,
} from '../../../types/programming-language-response';
import * as constants from '../../../constants/global.constats';
import post from '../../../services/post.service';
import { PostResponse } from '../../../types/post-response';
import { toast } from 'react-toastify';
import displayErrors from '../../../helpers/display-errors';
interface PostBody {
  text: string;
  codeSnippet: CodeSnippet;
}

interface CodeSnippet {
  text: string;
  programmingLanguageId: number;
  isGist: boolean;
  gist: Gist;
}

interface Gist {
  description: string;
  fileName: string;
}

interface Props extends LocalizeContextProps {
  // redux props
  isDarkTheme: boolean;
  isTokenAdded: boolean;
}

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

const AddEditPost: React.FC<Props & RouteComponentProps> = (
  props: Props & RouteComponentProps,
) => {
  const classes = useStyles();
  const [page, setPage] = useState(1);
  const [description, setDescription] = useState('');
  const [programmingLang, setProgrammingLang] = useState('');
  const [snippet, setSnippet] = useState('');
  const [fileName, setFileName] = useState('snippet.txt');
  const [addToGists, setAddToGists] = useState(true);
  const [isLoading, setIsLoading] = useState(false);
  const [programmingLanguages, setProgrammingLanguages] = useState<
    Programminglanguage[] | null
  >(null);

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

  const lang = props.activeLanguage ? props.activeLanguage.code : 'en';

  const getProgrammingLangs = () => {
    get<ProgrammingLanguageResponse>(
      constants.programmingLanguageController,
      '',
      lang,
      <T id="errorConnection" />,
      true,
    ).then(
      resp => {
        setProgrammingLanguages(resp.models);
      },
      () => setProgrammingLanguages(null),
    );
  };

  useEffect(() => {
    getProgrammingLangs();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const addPost = () => {
    setIsLoading(true);
    const selected = programmingLanguages?.find(
      e => e.code === languages[programmingLang],
    )?.id;
    const temp = selected ? selected : 1;
    const language = programmingLanguages !== null ? temp : 1;
    const isGist = props.isTokenAdded ? addToGists : false;
    const gist: Gist = { description, fileName };
    const data: PostBody = {
      text: description,
      codeSnippet: {
        text: snippet,
        programmingLanguageId: language,
        isGist,
        gist,
      },
    };
    post<PostResponse, PostBody>(
      data,
      constants.postController,
      '',
      lang,
      <T id="errorConnection" />,
      true,
    ).then(
      resp => {
        toast.success('Post added successfully');
        props.history.push(`/Post?postId=${resp.model.id}`);
        setIsLoading(false);
      },
      error => {
        setIsLoading(false);
        displayErrors(error);
      },
    );
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
      <Typography variant="h4">
        <T id="addDescription" />
      </Typography>
      <Typography variant="h5" color="textSecondary">
        <T id="addText" />
      </Typography>
    </div>
  );

  const descriptionPageContent = (
    <TextField
      value={description}
      onChange={e => setDescription(e.target.value)}
      multiline={true}
      label={<T id="postDescription" />}
      fullWidth={true}
    />
  );

  const codePageDescription = (
    <div>
      <Typography variant="h4">
        <T id="addSnippet" />
      </Typography>
      <Typography variant="h5" color="textSecondary">
        <T id="addSnippetText" />
      </Typography>
    </div>
  );

  const codePageContent = (t: any) => (
    <div>
      <Typography variant="body2" color="textPrimary" component="p">
        {description}
      </Typography>
      <div className={classes.snippetEdit}>
        <FormControl className={classes.formControl}>
          <InputLabel htmlFor="age-native-simple">
            <T id="language" />
          </InputLabel>
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
          placeholder={t('placeCode') as string}
          mode={programmingLang}
          theme={props.isDarkTheme ? 'tomorrow_night' : 'github'}
          value={snippet}
          onChange={onEditorChange}
          editorProps={{ $blockScrolling: true }}
        />
      </div>
    </div>
  );

  const summaryPageDescription = (
    <div>
      <Typography variant="h4">
        <T id="summary" />
      </Typography>
      <Typography variant="h5" color="textSecondary">
        <T id="lookThroughPost" />
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
          style={props.isDarkTheme ? vs2015 : githubGist}
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

  const getPageContent = (t: any) => {
    switch (page) {
      case 1:
        return descriptionPageContent;
      case 2:
        return codePageContent(t);
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
    if (page === 3) {
      addPost();
    }
  };

  return (
    <Container className={classes.root} maxWidth="md">
      <LinearProgress hidden={!isLoading} />
      {getPageDescription()}
      <div className={classes.marginStandard}>
        {page !== 1 && (
          <Button
            className={classes.button}
            onClick={handlePrevButton}
            variant="contained"
            color="primary"
          >
            <T id="previousPage" />
          </Button>
        )}

        <Button
          onClick={handleNextButton}
          className={classes.button}
          variant="contained"
          color="primary"
        >
          {page !== 3 ? <T id="nextPage" /> : <T id="sent" />}
        </Button>
      </div>

      <div className={classes.root}>
        <Typography className={classes.marginSmall} variant="body1">
          <T id="postWillLook" />
        </Typography>
        <Card>
          <T>
            {({ translate }) => (
              <div>
                <CardHeader
                  avatar={
                    <UserAvatar
                      firstName={translate('your') as string}
                      lastName={translate('name') as string}
                      photo={null}
                    />
                  }
                  title={translate('yourName') as string}
                  subheader={translate('now') as string}
                />

                <CardContent> {getPageContent(translate)}</CardContent>
              </div>
            )}
          </T>

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
            {props.isTokenAdded && (
              <>
                <FormControlLabel
                  control={
                    <Checkbox
                      color="primary"
                      checked={addToGists}
                      onChange={handleAddToGists}
                    />
                  }
                  label={<T id="addToGists" />}
                />

                {addToGists && (
                  <TextField
                    defaultValue={
                      programmingLang === '' ? '.txt' : fileExt[programmingLang]
                    }
                    className={classes.formControl}
                    value={fileName}
                    onChange={e => handleFileName(e.target.value)}
                    label={<T id="fileName" />}
                  />
                )}
              </>
            )}
          </CardActions>
        </Card>
      </div>
    </Container>
  );
};

const mapStateToProps = (state: AppState) => ({
  isDarkTheme: state.settings.isDarkTheme,
  isTokenAdded: state.settings.hasGithubToken,
});

export default connect(mapStateToProps)(withLocalize(withRouter(AddEditPost)));
