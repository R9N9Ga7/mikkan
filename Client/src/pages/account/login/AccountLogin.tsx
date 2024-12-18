import { useNavigate } from 'react-router-dom';
import useFetchAccountLogin from '../../../hooks/api/useFetchAccountLogin';
import { MAIN_PAGE_URL, REGISTRATION_FULL_URL } from '../../../consts/pagesUrls';
import AccountForm from '../common/AccountForm';
import { AccountLoginResponse } from '../../../api/interfaces/account';
import { useContext } from 'react';
import AccountContext, { AccountContextProps } from '../../../contexts/AccountContext';

function AccountLogin() {
  const navigate = useNavigate();
  const { setAccount } = useContext(AccountContext) as AccountContextProps;

  const { fetchData, isLoading, error } = useFetchAccountLogin({
    onSuccess: (data: AccountLoginResponse | null) => {
      setAccount(data);
      navigate(MAIN_PAGE_URL);
    },
  });

  const handleOnSubmit = async (username: string, password: string) => {
    await fetchData({
      username: username,
      password: password,
    });
  };

  return (
    <AccountForm
      actionTitle="Login"
      error={error}
      isLoading={isLoading}
      linkTitle="Create Account"
      linkTo={REGISTRATION_FULL_URL}
      onSubmit={handleOnSubmit}
      title="Login"
    />
  );
}

export default AccountLogin;
