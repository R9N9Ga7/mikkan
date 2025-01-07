import { createBrowserRouter } from 'react-router-dom';

import App from './app/App';

import MainLayout from './layouts/MainLayout';
import AuthLayout from './layouts/AuthLayout';
import CreateAccount from './pages/account/create';

import { AUTH_URL, LOGIN_URL, CREATE_ACCOUNT_URL } from './common/consts/pages_urls';
import AccountLogin from './pages/account/login';

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
            path: CREATE_ACCOUNT_URL,
            element: <CreateAccount />,
          },
          {
            path: LOGIN_URL,
            element: <AccountLogin />,
          },
        ],
      },
    ],
  },
]);

export default router;
