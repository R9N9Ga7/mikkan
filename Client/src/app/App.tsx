import { Outlet } from 'react-router-dom';
import AccountProvider from '../providers/AccountProvider';

function App() {
  return (
    <AccountProvider>
      <Outlet />
    </AccountProvider>
  );
}

export default App;
