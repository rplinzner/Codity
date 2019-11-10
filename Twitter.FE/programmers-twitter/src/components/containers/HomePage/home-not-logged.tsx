import * as React from 'react';
import { renderToStaticMarkup } from 'react-dom/server';
import {
  withLocalize,
  LocalizeContextProps,
  Translate,
} from 'react-localize-redux';
import globalTranslations from '../../../translations/global.json';

export interface Props extends LocalizeContextProps {}

class Home extends React.Component<Props> {
  constructor(props: Props) {
    super(props);

    this.props.initialize({
      languages: [
        { name: 'English', code: 'en' },
        { name: 'Polish', code: 'pl' },
      ],
      translation: globalTranslations,
      options: { renderToStaticMarkup },
    });
  }
  componentDidMount() {}
  public render() {
    return (
      <h1>
        <Translate id="greeting" />
      </h1>
    );
  }
}
export default withLocalize(Home);
