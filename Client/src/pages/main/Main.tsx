import { FC, useEffect, useState } from 'react';
import { Alert, Button, ListGroup } from 'react-bootstrap';
import { Link, useLocation } from 'react-router-dom';
import useFetchVaultGetAllItems from '../../hooks/api/useFetchVaultGetAllItems';
import { SHOW_ITEM_PARAM_FULL_URL } from '../../common/consts/pages_urls';
import { Bucket, PencilSquare } from 'react-bootstrap-icons';
import RemoveItemModal from '../../modals/remove_item/RemoveItemModal';
import { VaultAllItemsResponse } from '../../api/interfaces/vault';

const Main: FC = () => {
  const [itemForRemoving, setItemForRemoving] = useState<VaultAllItemsResponse | null>(null);

  const location = useLocation();
  const { error, fetchData, data } = useFetchVaultGetAllItems({
    defaultData: [],
  });

  useEffect(() => {
    fetchData();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location]);

  const handleOnItemRemove = (event: React.SyntheticEvent<HTMLElement>, item: VaultAllItemsResponse): void => {
    event.preventDefault();
    setItemForRemoving(item);
  };

  const handleOnCloseItemRemoveModal = async (): Promise<void> => {
    setItemForRemoving(null);
    await fetchData();
  };

  if (!data) {
    return (<h3>Loading...</h3>);
  }

  return (
    <div>
      <RemoveItemModal
        isOpen={!!itemForRemoving}
        onHide={handleOnCloseItemRemoveModal}
        item={itemForRemoving}
      />
      {
        error ? (<Alert variant="danger">{ error }</Alert>) : null
      }
      <ListGroup as="li">
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
              <div className="d-flex gap-3">
                <Button
                  variant="danger"
                  onClick={(event) => handleOnItemRemove(event, item)}
                >
                  <Bucket></Bucket>
                </Button>
                <Button variant="outline-secondary">
                  <PencilSquare></PencilSquare>
                </Button>
              </div>
            </ListGroup.Item>
          ))
        }
      </ListGroup>
    </div>
  );
};

export default Main;
