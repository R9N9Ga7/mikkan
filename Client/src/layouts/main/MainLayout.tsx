import { FC } from 'react';
import { Outlet, useNavigate, useSearchParams } from 'react-router-dom';
import AccountMiddleware from '../../middlewares/AccountMiddleware';
import { Container } from 'react-bootstrap';
import Header from './Header';
import AddItemModal from '../../modals/add_item/AddItemModal';
import { MAIN_PAGE_URL } from '../../common/consts/pages_urls';
import ShowItemModal from '../../modals/show_item/ShowItemModal';

const MainLayout: FC = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  return (
    <AccountMiddleware>
      <Header />
      <AddItemModal
        isOpen={!!searchParams.get('add-item')}
        onHide={() => navigate(MAIN_PAGE_URL)}
      />
      <ShowItemModal
        isOpen={!!searchParams.get('show-item')}
        onHide={() => navigate(MAIN_PAGE_URL)}
      />
      <Container className="my-3">
        <Outlet />
      </Container>
    </AccountMiddleware>
  );
};

export default MainLayout;
