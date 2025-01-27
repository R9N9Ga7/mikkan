import { FC, useState } from 'react';
import { Outlet } from 'react-router-dom';
import NotificationContext, { INotification } from '../contexts/NotificationContext';

const App: FC = () => {
  const [notifications, setNotifications] = useState<INotification[]>([]);

  return (
    <NotificationContext.Provider value={{
      notifications,
      setNotifications,
    }}>
      <Outlet />
    </NotificationContext.Provider>
  );
};

export default App;
