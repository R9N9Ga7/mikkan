import { Alert, Button, Form, Spinner } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';
import { LOGIN_FULL_URL, REGISTRATION_FULL_URL } from '../../../consts/pagesUrls';
import useFetchCreateAccount from '../../../hooks/api/useFetchCreateAccount';

interface CreateAccountForm extends HTMLFormElement {
  username: HTMLInputElement;
  password: HTMLInputElement;
}

function CreateAccount() {
  const navigate = useNavigate();

  const { fetchData, isLoading, error } = useFetchCreateAccount({
    onSuccess: () => {
      navigate(LOGIN_FULL_URL);
    },
  });

  const handleOnSubmit = async (event: React.SyntheticEvent<CreateAccountForm>) => {
    event.preventDefault();

    const { username, password } = event.currentTarget;
    await fetchData({
      username: username.value,
      password: password.value,
    });
  };

  return (
    <Form onSubmit={handleOnSubmit}>
      <h1 className="fs-3 text-center">Registration</h1>
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
        >Submit</Button>
        {
          isLoading
            ? (
              <Spinner animation="border" variant="primary" />
            ) : null
        }
        <Link
          to={REGISTRATION_FULL_URL}
          className="me-1"
        >Login</Link>
      </Form.Group>
    </Form>
  );
}

export default CreateAccount;
