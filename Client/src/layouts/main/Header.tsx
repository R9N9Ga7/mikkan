import { FC } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Button, Container, Nav, Navbar } from 'react-bootstrap';
import { ADD_ITEM_PARAM_FULL_URL, LOGIN_FULL_URL, MAIN_PAGE_URL } from '../../common/consts/pages_urls';
import AccountStorage from '../../utils/account_storage';

const Header: FC = () => {
  const navigate = useNavigate();

  const handleOnLogout = (): void => {
    AccountStorage.clear();
    navigate(LOGIN_FULL_URL);
  };

  return (
    <Navbar expand="lg" className="bg-body-tertiary">
      <Container fluid>
        <Navbar.Brand as={Link} to={MAIN_PAGE_URL}>Mikkan</Navbar.Brand>
        <Navbar.Toggle aria-controls="navbarScroll" />
        <Navbar.Collapse id="navbarScroll">
          <Nav className="me-auto">
            <Nav.Link as={Link} to={ADD_ITEM_PARAM_FULL_URL}>Add Item</Nav.Link>
          </Nav>
          <Button
            variant="outline-danger"
            onClick={handleOnLogout}
          >Logout</Button>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default Header;
