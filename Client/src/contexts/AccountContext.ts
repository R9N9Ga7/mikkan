import { createContext } from 'react';

export type Account = {
  accessToken: string;
  refreshToken: string;
};

export type AccountContextProps = {
  account: Account | null;
  setAccount: React.Dispatch<React.SetStateAction<Account | null>>;
};

const AccountContext = createContext<AccountContextProps | null>(null);

export default AccountContext;
