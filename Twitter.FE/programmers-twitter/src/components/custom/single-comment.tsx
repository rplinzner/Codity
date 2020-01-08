import React from 'react';
import {
  CardHeader,
  CardActionArea,
  CardContent,
  Typography,
} from '@material-ui/core';
import { withLocalize, LocalizeContextProps } from 'react-localize-redux';
import { UserAvatar } from '../containers/profile/index';

interface Props extends LocalizeContextProps {
  authorFirstName: string;
  authorLastName: string;
  authorImage: string;
  commentDate: string;
  commentText: string;
}

const SingleComment: React.FC<Props> = (props: Props) => {
  const date = new Date(props.commentDate);
  const langCode = props.activeLanguage ? props.activeLanguage.code : 'en';

  return (
    <>
      <CardActionArea onClick={() => window.alert('go to user')}>
        <CardHeader
          avatar={
            <UserAvatar
              firstName={props.authorFirstName}
              lastName={props.authorLastName}
              photo={props.authorImage}
            />
          }
          title={props.authorFirstName + ' ' + props.authorLastName}
          subheader={new Intl.DateTimeFormat(langCode, {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: '2-digit',
            hour: 'numeric',
            minute: 'numeric',
          }).format(date)}
        />
      </CardActionArea>
      <CardContent>
        <Typography variant="body2" color="textPrimary" component="p">
          {props.commentText}
        </Typography>
      </CardContent>
    </>
  );
};

export default withLocalize(SingleComment);
