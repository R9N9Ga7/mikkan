import { createBrowserRouter } from 'react-router-dom';

import App from './app/App';

import MainLayout from './layouts/MainLayout';
import AuthLayout from './layouts/AuthLayout';
import Registration from './pages/account/create';

import { AUTH_URL, REGISTRATION_URL } from './consts/pagesUrls';

const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      {
        path: '/',
        element: <MainLayout />,
        children: [],
      },
      {
        path: AUTH_URL,
        element: <AuthLayout />,
        children: [
          {
            path: REGISTRATION_URL,
            element: <Registration />,
          },
        ],
      },
    ],
  },
]);

export default router;
