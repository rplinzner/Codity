import React from 'react';
import {
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  makeStyles,
  Theme,
} from '@material-ui/core';
import {
  withLocalize,
  LocalizeContextProps,
  Translate as T,
} from 'react-localize-redux';

interface Props extends LocalizeContextProps {}

const useStyles = makeStyles((theme: Theme) => ({
  formControl: {
    margin: theme.spacing(1),
    minWidth: 120,
  },
  selectEmpty: {
    marginTop: theme.spacing(2),
  },
}));

const LanguageSelector: React.FC<Props> = (props: Props) => {
  const { languages, activeLanguage, setActiveLanguage } = props;
  const classes = useStyles();
  const inputLabel = React.useRef<HTMLLabelElement>(null);
  const [labelWidth, setLabelWidth] = React.useState(0);
  React.useEffect(() => {
    setLabelWidth(inputLabel.current!.offsetWidth);
  }, []);

  const handleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    setActiveLanguage(event.target.value as string);
  };

  return (
    <FormControl className={classes.formControl}>
      <InputLabel ref={inputLabel} id="demo-simple-select-outlined-label">
        <T id="language" />
      </InputLabel>
      <Select
        labelId="demo-simple-select-outlined-label"
        id="demo-simple-select-outlined"
        value={(activeLanguage && activeLanguage.code) || ''}
        onChange={handleChange}
        labelWidth={labelWidth}
      >
        {languages.map((lang) => (
          <MenuItem key={lang.code} value={lang.code}>{lang.name}</MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};

export default withLocalize(LanguageSelector);
