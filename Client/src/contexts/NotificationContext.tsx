import { createContext } from 'react';

export type INotification = {
  message: string,
};

export type INotificationContext = {
  notifications: INotification[],
  setNotifications: React.Dispatch<React.SetStateAction<INotification[]>>,
};

const NotificationContext = createContext<INotificationContext>({
  notifications: [],
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  setNotifications: (_) => {},
});

export default NotificationContext;
