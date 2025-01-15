import { FC } from 'react';
import { Alert, Button, Modal } from 'react-bootstrap';
import useFetchVaultRemoveItem from '../../hooks/api/useFetchVaultRemoveItem';
import { VaultAllItemsResponse } from '../../api/interfaces/vault';

type RemoveItemModalProps = {
  isOpen: boolean;
  onHide: () => void;
  item: VaultAllItemsResponse | null;
};

const RemoveItemModal: FC<RemoveItemModalProps> = ({
  isOpen,
  onHide,
  item,
}) => {
  const { error, fetchData, isLoading } = useFetchVaultRemoveItem({
    id: item?.id ?? '',
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    onSuccess: (_: null) => {
      onHide();
    },
  });

  const handleOnRemove = async (): Promise<void> => {
    await fetchData();
  };

  return (
    <Modal
      show={isOpen}
      onHide={onHide}
      size="sm"
      aria-labelledby="contained-modal-title-vcenter"
      centered
    >
      <Modal.Header closeButton>
        <Modal.Title id="contained-modal-title-vcenter">
          Remove { item?.name ?? '' }
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {
          error ? (<Alert variant="danger">{ error }</Alert>) : null
        }
        <div className="d-flex justify-content-center gap-3">
          <Button
            variant="danger"
            onClick={handleOnRemove}
            disabled={isLoading}
          >Yes</Button>
          <Button
            onClick={onHide}
          >No</Button>
        </div>
      </Modal.Body>
    </Modal>
  );
};

export default RemoveItemModal;
