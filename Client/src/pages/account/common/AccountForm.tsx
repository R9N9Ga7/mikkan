import { Alert, Button, Form, Spinner } from 'react-bootstrap';
import { Link } from 'react-router-dom';

interface AccountFormElement extends HTMLFormElement {
  username: HTMLInputElement;
  password: HTMLInputElement;
}

interface AccountFormProps {
  title: string;
  actionTitle: string;
  linkTitle: string;
  linkTo: string;

  error: string | null;
  isLoading: boolean;

  onSubmit: (username: string, password: string) => void;
}

function AccountForm({
  onSubmit,
  title,
  actionTitle,
  linkTitle,
  linkTo,
  error,
  isLoading,
}: AccountFormProps) {
  const handleOnSubmit = (event: React.SyntheticEvent<AccountFormElement>) => {
    event.preventDefault();

    const { username, password } = event.currentTarget;
    onSubmit(username.value, password.value);
  };

  return (
    <Form onSubmit={handleOnSubmit}>
      <h1 className="fs-3 text-center">{ title }</h1>
      <hr />
      <Form.Group className="mb-3">
        <Form.Label>Username</Form.Label>
        <Form.Control
          required
          type="text"
          placeholder="Enter username"
          name="username"
        />
      </Form.Group>
      <Form.Group className="mb-3">
        <Form.Label>Password</Form.Label>
        <Form.Control
          required
          type="password"
          placeholder="Password"
          name="password"
        />
      </Form.Group>
      <Form.Group>
        {
          error
            ? (
              <Alert variant="danger">{ error }</Alert>
            ) : null
        }
      </Form.Group>
      <Form.Group className="d-flex justify-content-between align-items-center">
        <Button
          variant="primary"
          type="submit"
          disabled={isLoading}
        >{ actionTitle }</Button>
        {
          isLoading
            ? (
              <Spinner animation="border" variant="primary" />
            ) : null
        }
        <Link
          to={linkTo}
          className="me-1"
        >{ linkTitle }</Link>
      </Form.Group>
    </Form>
  );
}

export default AccountForm;
