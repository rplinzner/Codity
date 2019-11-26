import React, { ReactNode } from 'react';
import {
  fade,
  makeStyles,
  Theme,
  createStyles,
} from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import Typography from '@material-ui/core/Typography';
import InputBase from '@material-ui/core/InputBase';
import Badge from '@material-ui/core/Badge';
import MenuIcon from '@material-ui/icons/Menu';
import SearchIcon from '@material-ui/icons/Search';
import AccountCircle from '@material-ui/icons/AccountCircle';
import MailIcon from '@material-ui/icons/Mail';
import NotificationsIcon from '@material-ui/icons/Notifications';
import MoreIcon from '@material-ui/icons/MoreVert';
import {
  Link as RouterLink,
  LinkProps as RouterLinkProps,
  withRouter,
  RouteComponentProps,
} from 'react-router-dom';
import { Link, Tooltip, Paper, ClickAwayListener } from '@material-ui/core';
import { connect } from 'react-redux';
import {
  Translate as T,
  LocalizeContextProps,
  withLocalize,
} from 'react-localize-redux';
import Popper from '@material-ui/core/Popper';

import ResponsiveDrawer from './drawer';
import { logout } from '../../store/user/user.actions';
import { layoutTranslations } from '../../translations/index';
import SearchResultCard from './search-result-card';
import MobileAccountMenu from './mobile-account-menu';
import AccountMenu from './account-menu';
import { AppState } from '../..';
import ThemeSwitch from './theme-switch';

const drawerWidth = 240;

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    grow: {
      flexGrow: 1,
    },
    menuButton: {
      marginRight: theme.spacing(2),
      [theme.breakpoints.up('md')]: {
        display: 'none',
      },
    },
    title: {
      color: 'black',
      display: 'none',
      [theme.breakpoints.up('sm')]: {
        display: 'block',
      },
    },
    search: {
      position: 'relative',
      borderRadius: theme.shape.borderRadius,
      backgroundColor: fade(theme.palette.common.white, 0.15),
      '&:hover': {
        backgroundColor: fade(theme.palette.common.white, 0.25),
      },
      marginRight: theme.spacing(2),
      marginLeft: 0,
      width: '100%',
      [theme.breakpoints.up('sm')]: {
        marginLeft: theme.spacing(3),
        width: 'auto',
      },
    },
    searchIcon: {
      width: theme.spacing(7),
      height: '100%',
      position: 'absolute',
      pointerEvents: 'none',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
    },
    inputRoot: {
      color: 'inherit',
    },
    inputInput: {
      padding: theme.spacing(1, 1, 1, 7),
      transition: theme.transitions.create('width'),
      width: '100%',

      [theme.breakpoints.up('sm')]: {
        width: 150,
        '&:focus': {
          width: 220,
        },
      },
      [theme.breakpoints.up('md')]: {
        width: 300,
        '&:focus': {
          width: 400,
        },
      },
    },
    searchPopper: {
      zIndex: theme.zIndex.modal,
      marginTop: 5,
    },
    searchPaper: {
      [theme.breakpoints.up('sm')]: {
        width: 220,
      },
      [theme.breakpoints.up('md')]: {
        width: 400,
      },
    },
    sectionDesktop: {
      display: 'none',
      [theme.breakpoints.up('md')]: {
        display: 'flex',
      },
    },
    sectionMobile: {
      display: 'flex',
      [theme.breakpoints.up('md')]: {
        display: 'none',
      },
    },
    appBar: {
      zIndex: theme.zIndex.drawer + 1,
    },
    content: {
      [theme.breakpoints.up('md')]: {
        paddingTop: '64px',
        paddingLeft: drawerWidth,
      },
      [theme.breakpoints.down('md')]: {
        paddingTop: '56px',
      },
    },
  }),
);

const Link1 = React.forwardRef<HTMLAnchorElement, RouterLinkProps>(
  (props, ref) => <RouterLink innerRef={ref} {...props} />,
);

interface Props extends LocalizeContextProps {
  children?: ReactNode;
  isLoggedIn: boolean;
  logOutAction: typeof logout;
}

function PrimarySearchAppBar(props: Props & RouteComponentProps) {
  props.addTranslation(layoutTranslations);
  const classes = useStyles();
  const [
    primaryAccountMenuAnchorEl,
    setPrimaryAccountMenuAnchorEl,
  ] = React.useState<null | HTMLElement>(null);
  const [
    mobileAccountMenuAnchorEl,
    setMobileAccountMenuAnchorEl,
  ] = React.useState<null | HTMLElement>(null);
  const [searchAnchorEl, setSearchAnchorEl] = React.useState<any | null>(null);

  const isMenuOpen = Boolean(primaryAccountMenuAnchorEl);
  const isMobileAccountMenuOpen = Boolean(mobileAccountMenuAnchorEl);
  const isSearchOpen = Boolean(searchAnchorEl);
  const searchId = isSearchOpen ? 'search-popover' : undefined;

  const [mobileDrawerOpen, setMobileDrawerOpen] = React.useState(false);

  const handleDrawerToggle = () => {
    setMobileDrawerOpen(!mobileDrawerOpen);
  };

  const handleDrawerClose = () => {
    setMobileDrawerOpen(false);
  };

  const handleMobileAccountMenuOpen = (
    event: React.MouseEvent<HTMLElement>,
  ) => {
    setPrimaryAccountMenuAnchorEl(event.currentTarget);
  };

  const handleMobileAccountMenuClose = () => {
    setMobileAccountMenuAnchorEl(null);
  };

  const handleMenuClose = () => {
    setPrimaryAccountMenuAnchorEl(null);
    handleMobileAccountMenuClose();
  };

  const handleMobileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setMobileAccountMenuAnchorEl(event.currentTarget);
  };

  const handlePopoverOpen = (
    event: React.FocusEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    setSearchAnchorEl(event.currentTarget);
  };

  const handlePopoverClose = () => {
    setSearchAnchorEl(null);
  };

  const { isLoggedIn, logOutAction } = props;

  const mobileAccountMenuId = 'primary-search-account-menu-mobile';
  const menuId = 'primary-search-account-menu';

  return (
    <div className={classes.grow}>
      <AppBar className={classes.appBar} position="fixed">
        <Toolbar>
          <IconButton
            edge="start"
            onClick={handleDrawerToggle}
            className={classes.menuButton}
            color="inherit"
            aria-label="open drawer"
          >
            <MenuIcon />
          </IconButton>
          <Link component={Link1} to={isLoggedIn ? '/MyFeed' : '/'}>
            <Typography className={classes.title} variant="h6" noWrap={true}>
              InzTwitter
            </Typography>
          </Link>
          {isLoggedIn ? (
            <div className={classes.search}>
              <div className={classes.searchIcon}>
                <SearchIcon />
              </div>
              <T>
                {({ translate }) => (
                  <ClickAwayListener onClickAway={handlePopoverClose}>
                    <div>
                      <InputBase
                        placeholder={translate('search') as string}
                        classes={{
                          root: classes.inputRoot,
                          input: classes.inputInput,
                        }}
                        inputProps={{ 'aria-label': 'search' }}
                        onFocus={handlePopoverOpen}
                        onKeyPress={e => {
                          if (e.key === 'Enter') {
                            handlePopoverClose();
                            props.history.push('/SearchResults');
                          }
                        }}
                      />
                      <Popper
                        id={searchId}
                        open={isSearchOpen}
                        anchorEl={searchAnchorEl}
                        className={classes.searchPopper}
                      >
                        <Paper className={classes.searchPaper}>
                          <div style={{ padding: '10px' }}>
                            <SearchResultCard />
                          </div>
                          <div style={{ padding: '10px' }}>
                            <SearchResultCard />
                          </div>
                          <div style={{ padding: '10px' }}>
                            <SearchResultCard />
                          </div>
                        </Paper>
                      </Popper>
                    </div>
                  </ClickAwayListener>
                )}
              </T>
            </div>
          ) : null}

          <div className={classes.grow} />
          {/* Normal menu */}
          {isLoggedIn ? (
            <div className={classes.sectionDesktop}>
              <Tooltip title={<T id="messages" />}>
                <IconButton aria-label="show 4 new mails" color="inherit">
                  <Badge badgeContent={420} color="secondary">
                    <MailIcon />
                  </Badge>
                </IconButton>
              </Tooltip>
              <Tooltip title={<T id="notifications" />}>
                <IconButton
                  aria-label="show 17 new notifications"
                  color="inherit"
                >
                  <Badge badgeContent={69} color="secondary">
                    <NotificationsIcon />
                  </Badge>
                </IconButton>
              </Tooltip>
              <Tooltip title={<T id="profile" />}>
                <IconButton
                  edge="end"
                  aria-label="account of current user"
                  aria-controls={menuId}
                  aria-haspopup="true"
                  onClick={handleMobileAccountMenuOpen}
                  color="inherit"
                >
                  <AccountCircle />
                </IconButton>
              </Tooltip>
            </div>
          ) : null}
          {/* End of normal menu */}
          {isLoggedIn ? (
            <div className={classes.sectionMobile}>
              <IconButton
                aria-label="show more"
                aria-controls={mobileAccountMenuId}
                aria-haspopup="true"
                onClick={handleMobileMenuOpen}
                color="inherit"
              >
                <MoreIcon />
              </IconButton>
            </div>
          ) : <ThemeSwitch />}
        </Toolbar>
      </AppBar>
      {isLoggedIn ? (
        <MobileAccountMenu
          anchorEl={mobileAccountMenuAnchorEl}
          id={mobileAccountMenuId}
          isOpen={isMobileAccountMenuOpen}
          onClose={handleMobileAccountMenuClose}
          handleOpen={handleMobileAccountMenuOpen}
        />
      ) : null}
      <AccountMenu
        anchorEl={primaryAccountMenuAnchorEl}
        id={menuId}
        isOpen={isMenuOpen}
        onClose={handleMenuClose}
        logOutAction={logOutAction}
      />
      <ResponsiveDrawer
        isOpen={mobileDrawerOpen}
        onDrawerChange={handleDrawerToggle}
        drawerWidth={drawerWidth}
        onDrawerClose={handleDrawerClose}
      />
      <div className={classes.content}>{props.children}</div>
    </div>
  );
}

const mapStateToProps = (state: AppState) => ({
  isLoggedIn: state.user.loggedIn,
});

const mapDispatchToProps = (dispatch: any) => {
  return {
    logOutAction: () => dispatch(logout()),
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(withLocalize(withRouter(PrimarySearchAppBar)));
