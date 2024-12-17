import { Col, Container, Row } from 'react-bootstrap';
import { Outlet } from 'react-router-dom';

function AuthLayout() {
  return (
    <Container>
      <Row className="vh-100 d-flex align-items-center justify-content-center">
        <Col xs lg="3" className="border border-secondary-subtle rounded py-2 m-3">
          <Outlet />
        </Col>
      </Row>
    </Container>
  );
}

export default AuthLayout;
