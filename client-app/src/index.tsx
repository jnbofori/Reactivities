// import React from 'react';
import ReactDOM from 'react-dom/client';
import 'semantic-ui-css/semantic.min.css';
import 'react-calendar/dist/Calendar.css'
import './app/layout/styles.css';
import 'react-toastify/dist/ReactToastify.min.css'
import reportWebVitals from './reportWebVitals';
import { StoreContext, store } from './app/stores/store';
import { RouterProvider } from "react-router-dom";
import { router } from './app/router/Routes';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  // <React.StrictMode>
    <StoreContext.Provider value={store}>
      <RouterProvider router={router} />
    </StoreContext.Provider>
  // </React.StrictMode>
);
// note: Strict mode lets components render twice (in dev) in order to detect problems and warn you

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
