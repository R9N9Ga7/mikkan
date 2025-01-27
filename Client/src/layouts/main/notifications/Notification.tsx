import { FC, useEffect } from 'react';
import { INotification } from '../../../contexts/NotificationContext';
import { Toast } from 'react-bootstrap';

type NotificationProps = {
  notification: INotification;
  onClose: () => void;
};

const NOTIFICATION_LIFETIME_IN_MS = 3000;

const Notification: FC<NotificationProps> = ({ notification, onClose }) => {
  useEffect(() => {
    const interval = setInterval(onClose, NOTIFICATION_LIFETIME_IN_MS);
    return (): void => {
      clearInterval(interval);
    };
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <Toast onClose={onClose}>
      <Toast.Header>
        <strong className="me-auto">Notification</strong>
      </Toast.Header>
      <Toast.Body>{ notification.message }</Toast.Body>
    </Toast>
  );
};

export default Notification;
