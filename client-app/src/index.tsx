import ReactDOM from 'react-dom/client';
import 'semantic-ui-css/semantic.min.css';
import 'react-calendar/dist/Calendar.css'
import 'react-toastify/dist/ReactToastify.min.css'
import 'react-datepicker/dist/react-datepicker.css'
import "cropperjs/dist/cropper.css";
import './app/layout/styles.css';
import { StoreContext, store } from './app/stores/store';
import { RouterProvider } from "react-router-dom";
import { router } from './app/router/Routes';
import React from 'react';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <StoreContext.Provider value={store}>
      <RouterProvider router={router} />
    </StoreContext.Provider>
  </React.StrictMode>
);
// note: Strict mode lets components render twice (in dev) in order to detect problems and warn you
