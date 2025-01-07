import { FC } from 'react';
import { Outlet } from 'react-router-dom';
import AccountMiddleware from '../middlewares/AccountMiddleware';

const MainLayout: FC = () => {
  return (
    <div>
      <AccountMiddleware>
        <Outlet />
      </AccountMiddleware>
    </div>
  );
};

export default MainLayout;
