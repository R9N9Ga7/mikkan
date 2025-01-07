import { FC } from 'react';
import { useNavigate } from 'react-router-dom';
import useFetchAccountLogin from '../../../hooks/api/useFetchAccountLogin';
import { MAIN_PAGE_URL, REGISTRATION_FULL_URL } from '../../../common/consts/pages_urls';
import AccountForm from '../common/AccountForm';
import { AccountLoginResponse } from '../../../api/interfaces/account';
import { Account } from '../../../common/interfaces/account';
import AccountStorage from '../../../utils/account_storage';

const AccountLogin: FC = () => {
  const navigate = useNavigate();

  const { fetchData, isLoading, error } = useFetchAccountLogin({
    onSuccess: (data: AccountLoginResponse | null) => {
      AccountStorage.set(data as Account);
      navigate(MAIN_PAGE_URL);
    },
  });

  const handleOnSubmit = async (username: string, password: string): Promise<void> => {
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
};

export default AccountLogin;
