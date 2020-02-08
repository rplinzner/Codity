import { RouteProps, Route, Redirect, RouteComponentProps } from 'react-router';
import React from 'react';

interface PrivateRouteProps extends RouteProps {
  component: any;
}

const PrivateRoute = (props: PrivateRouteProps) => {
  const { component: Component, ...rest } = props;
  return (
    <Route
      {...rest}
      render={(props: RouteComponentProps) => {
        const user = localStorage.getItem('user');
        if (user) {
          return <Component {...props} />;
        } else {
          return (
            <Redirect
              to={{
                pathname: '/',
                state: { from: props.location },
              }}
            />
          );
        }
      }}
    />
  );
};
export default PrivateRoute;
