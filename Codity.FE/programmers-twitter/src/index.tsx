import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import thunk from 'redux-thunk';
import { createStore, compose, applyMiddleware, combineReducers } from 'redux';
import { LocalizeProvider, localizeReducer } from 'react-localize-redux';

import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';
import * as stores from './store/index';
import notificationsMiddlaware from './middlewares/notifications-middleware';

declare global {
  interface Window {
    __REDUX_DEVTOOLS_EXTENSION_COMPOSE__?: typeof compose;
  }
}

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const rootReducer = combineReducers({
  localize: localizeReducer,
  user: stores.userReducer,
  settings: stores.settingsReducer,
  notifications: stores.notificationsReducer,
});

const language = navigator.language;

export type AppState = ReturnType<typeof rootReducer>;

const store = createStore(
  rootReducer,
  composeEnhancers(applyMiddleware(thunk, notificationsMiddlaware)),
);

ReactDOM.render(
  <Provider store={store}>
    <LocalizeProvider store={store}>
      <App browserLanguage={language.includes('pl') ? 'pl' : 'en'} />
    </LocalizeProvider>
  </Provider>,
  document.getElementById('root'),
);

serviceWorker.register();
