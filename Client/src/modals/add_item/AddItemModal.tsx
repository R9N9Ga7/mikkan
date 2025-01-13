import { FC, useState } from 'react';
import { Alert, Button, Form, Modal } from 'react-bootstrap';
import useFetchVaultAddItem from '../../hooks/api/useFetchVaultAddItem';
import { VaultItem } from '../../common/interfaces/vault';

type AddItemModalProps = {
  isOpen: boolean;
  onHide: () => void;
};

const createEmptyVaultItem = (): VaultItem => ({
  name: '',
  login: '',
  password: '',
});

const AddItemModal: FC<AddItemModalProps> = ({ isOpen, onHide }) => {
  const [vaultItem, setVaultItem] = useState<VaultItem>(createEmptyVaultItem());

  const { error, fetchData, isLoading } = useFetchVaultAddItem({
    onSuccess: () => {
      setVaultItem(createEmptyVaultItem());
    },
  });

  const handleOnAdd = async (): Promise<void> => {
    await fetchData(vaultItem);
  };

  const updateVaultItem = (key: string, event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>): void => {
    setVaultItem({ ...vaultItem, [key]: event.currentTarget.value });
  };

  return (
    <Modal
      show={isOpen}
      onHide={onHide}
      size="lg"
      aria-labelledby="contained-modal-title-vcenter"
      centered
    >
      <Modal.Header closeButton>
        <Modal.Title id="contained-modal-title-vcenter">
          Add new item
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group className="mb-3">
            <Form.Label>Name</Form.Label>
            <Form.Control
              required
              type="text"
              placeholder="Enter name"
              value={vaultItem.name}
              onChange={(event) => updateVaultItem('name', event)}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Login</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter login"
              value={vaultItem.login}
              onChange={(event) => updateVaultItem('login', event)}
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              placeholder="Password"
              value={vaultItem.password}
              onChange={(event) => updateVaultItem('password', event)}
            />
          </Form.Group>
        </Form>
        {
          error ? (<Alert variant="danger">{ error }</Alert>) : null
        }
      </Modal.Body>
      <Modal.Footer>
        <Button
          variant="success"
          disabled={isLoading}
          onClick={handleOnAdd}
        >Add</Button>
        <Button
          disabled={isLoading}
          onClick={onHide}
        >Close</Button>
      </Modal.Footer>
    </Modal>
  );
};

export default AddItemModal;
