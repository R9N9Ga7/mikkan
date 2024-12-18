import { useNavigate } from 'react-router-dom';
import { LOGIN_FULL_URL } from '../../../consts/pagesUrls';
import useFetchCreateAccount from '../../../hooks/api/useFetchCreateAccount';
import AccountForm from '../common/AccountForm';

function CreateAccount() {
  const navigate = useNavigate();

  const { fetchData, isLoading, error } = useFetchCreateAccount({
    onSuccess: () => {
      navigate(LOGIN_FULL_URL);
    },
  });

  const handleOnSubmit = async (username: string, password: string) => {
    await fetchData({
      username,
      password,
    });
  };

  return (
    <AccountForm
      actionTitle="Create"
      error={error}
      isLoading={isLoading}
      linkTitle="Login"
      linkTo={LOGIN_FULL_URL}
      onSubmit={handleOnSubmit}
      title="Create Account"
    />
  );
}

export default CreateAccount;
