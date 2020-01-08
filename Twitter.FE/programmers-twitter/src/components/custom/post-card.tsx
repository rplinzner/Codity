import React from 'react';
import { Card, CardHeader, CardActions, IconButton, CardContent, Typography } from '@material-ui/core';
import FavoriteIcon from '@material-ui/icons/Favorite';

import { withLocalize, LocalizeContextProps } from 'react-localize-redux';

import { Post } from '../../types/post';
import { UserAvatar } from '../containers/profile/index';
import { red } from '@material-ui/core/colors';

interface Props extends LocalizeContextProps {
  post: Post;
  className?: string;
}

const PostCard: React.FC<Props> = (props: Props) => {
  //   const [expanded, setExpanded] = useState(false);

  const { post } = props;
  const langCode = props.activeLanguage ? props.activeLanguage.code : 'en';

  const postDate = new Date(post.creationDate);
  return (
    <div className={props.className}>
      <Card>
        <CardHeader
          avatar={
            <UserAvatar
              firstName={post.authorFirstName}
              lastName={post.authorLastName}
              photo={post.authorImage}
            />
          }
          title={post.authorFirstName + ' ' + post.authorLastName}
          subheader={new Intl.DateTimeFormat(langCode, {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: '2-digit',
            hour: 'numeric',
            minute: 'numeric',
          }).format(postDate)}
        />
        <CardContent>
          <Typography variant="body2" color="textPrimary" component="p">
            {post.text}
          </Typography>
        </CardContent>
        <CardActions disableSpacing>
          <IconButton aria-label="add to favorites">
            <FavoriteIcon style={{ color: red[500] }} />
          </IconButton>
        </CardActions>
      </Card>
    </div>
  );
};

export default withLocalize(PostCard);
