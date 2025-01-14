import { FC, useEffect } from 'react';
import useFetchVaultGetAllItems from '../../hooks/api/useFetchVaultGetAllItems';
import { Alert, Badge, Button, ListGroup } from 'react-bootstrap';
import { Link, useLocation } from 'react-router-dom';
import { SHOW_ITEM_PARAM_FULL_URL } from '../../common/consts/pages_urls';
import { PencilSquare } from 'react-bootstrap-icons';

const Main: FC = () => {
  const location = useLocation();
  const { error, fetchData, data } = useFetchVaultGetAllItems({
    defaultData: [],
  });

  useEffect(() => {
    fetchData();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location]);

  if (!data) {
    return (<h3>Loading...</h3>);
  }

  return (
    <div>
      {
        error ? (<Alert variant="danger">{ error }</Alert>) : null
      }
      <ListGroup as="ol">
        {
          data.map((item) => (
            <ListGroup.Item
              as={Link}
              to={SHOW_ITEM_PARAM_FULL_URL + item.id}
              key={item.id}
              className="d-flex justify-content-between align-items-center"
            >
              <div className="ms-2 me-auto">
                <div className="fw-bold">{ item.name }</div>
                { item.login }
              </div>
              <Button variant="outline-secondary">
                <PencilSquare></PencilSquare>
              </Button>
            </ListGroup.Item>
          ))
        }
      </ListGroup>
    </div>
  );
};

export default Main;
