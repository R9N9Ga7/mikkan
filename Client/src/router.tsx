import { createBrowserRouter } from 'react-router-dom';

import App from './app/App';

import MainLayout from './layouts/MainLayout';
import AuthLayout from './layouts/AuthLayout';

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
        path: '/auth',
        element: <AuthLayout />,
        children: [],
      },
    ],
  },
]);

export default router;
