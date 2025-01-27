import { FC, useContext } from 'react';
import { ToastContainer } from 'react-bootstrap';
import NotificationContext, { INotification, INotificationContext } from '../../../contexts/NotificationContext';
import Notification from './Notification';

const NotificationsContainer: FC = () => {
  const { notifications, setNotifications } = useContext<INotificationContext>(NotificationContext);

  const removeNotification = (notification: INotification): void => {
    setNotifications(notifications.filter((n) => n != notification));
  };

  return (
    <div
      className="bg-dark"
    >
      <ToastContainer
        className="p-3"
        position="bottom-end"
      >
        {
          notifications.map((notification) => (
            <Notification
              key={notification.message}
              notification={notification}
              onClose={() => removeNotification(notification)}
            />
          ))
        }
      </ToastContainer>
    </div>
  );
};

export default NotificationsContainer;
