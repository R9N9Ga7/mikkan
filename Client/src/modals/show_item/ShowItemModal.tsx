import { CSSProperties, FC, useEffect, useState } from 'react';
import { Alert, Button, Form, InputGroup, Modal } from 'react-bootstrap';
import useFetchVaultGetItem from '../../hooks/api/useFetchVaultGetItem';
import { useLocation, useSearchParams } from 'react-router-dom';
import { Copy, Eye, EyeSlash } from 'react-bootstrap-icons';
import { VaultAllItemsResponse } from '../../api/interfaces/vault';
import useFetchVaultEditItem from '../../hooks/api/useFetchVaultEditItem';

type ShowtemModalProps = {
  isOpen: boolean;
  onHide: () => void;
};

const createEmptyVaultItem = (): VaultAllItemsResponse => ({
  createdAt: '',
  id: '',
  login: '',
  name: '',
  password: '',
  userId: '',
});

const ShowItemModal: FC<ShowtemModalProps> = ({
  isOpen,
  onHide,
}) => {
  const location = useLocation();
  const [searchParams] = useSearchParams();
  const itemId = searchParams.get('show-item') ?? '';

  const [isVisiblePassword, setIsVisiblePassword] = useState<boolean>(false);
  const [isEditMode, setIsEditMode] = useState<boolean>(false);
  const [vaultItem, setVaultItem] = useState<VaultAllItemsResponse>(createEmptyVaultItem());

  const {
    error: vaultItemError,
    fetchData: fetchVaultItem,
  } = useFetchVaultGetItem({
    id: itemId,
    onSuccess: (data: VaultAllItemsResponse | null) => {
      if (!data) {
        return;
      }
      setVaultItem(data);
    },
  });

  const {
    error: editItemError,
    fetchData: fetchEditItem,
    isLoading: isLoadingEditItem,
  } = useFetchVaultEditItem({
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    onSuccess: (_: null) => {
      setIsEditMode(false);
    },
  });

  useEffect(() => {
    if (!itemId) {
      return;
    }
    fetchVaultItem();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location]);

  const buttonStyle = (): CSSProperties => ({ marginTop: '-.125rem', padding: 0 });

  const copyToClipboard = async (): Promise<void> => {
    if (!vaultItem) {
      return;
    }
    await window.navigator.clipboard.writeText(vaultItem.password);
  };

  const handleOnHide = (): void => {
    setIsVisiblePassword(false);
    setIsEditMode(false);
    setVaultItem(createEmptyVaultItem());
    onHide();
  };

  const handleOnSave = async (): Promise<void> => {
    await fetchEditItem(vaultItem);
  };

  const updateVaultItem = (key: string, event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>): void => {
    setVaultItem({ ...vaultItem, [key]: event.currentTarget.value });
  };

  if (!vaultItem) {
    return (<></>);
  }

  return (
    <Modal
      show={isOpen}
      onHide={handleOnHide}
      size="lg"
      aria-labelledby="contained-modal-title-vcenter"
      centered
    >
      <Modal.Header closeButton>
        <Modal.Title>
          Item
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <InputGroup className="mb-3">
          <InputGroup.Text>
            Name
          </InputGroup.Text>
          <Form.Control
            disabled={!isEditMode}
            value={vaultItem.name}
            onChange={(event) => updateVaultItem('name', event)}
          />
        </InputGroup>
        <InputGroup className="mb-3">
          <InputGroup.Text>
            Login
          </InputGroup.Text>
          <Form.Control
            disabled={!isEditMode}
            value={vaultItem.login}
            onChange={(event) => updateVaultItem('login', event)}
          />
        </InputGroup>
        <InputGroup className="mb-3">
          <InputGroup.Text>
            Password
          </InputGroup.Text>
          <Form.Control
            disabled={!isEditMode}
            type={isVisiblePassword ? 'text' : 'password'}
            value={vaultItem.password}
            onChange={(event) => updateVaultItem('password', event)}
          />
          <InputGroup.Text>
            <button
              className="btn"
              style={buttonStyle()}
              onClick={copyToClipboard}
            ><Copy></Copy></button>
          </InputGroup.Text>
          <InputGroup.Text>
            <button
              className="btn"
              style={buttonStyle()}
              onClick={() => setIsVisiblePassword(!isVisiblePassword)}
            >
              {
                isVisiblePassword ? (<EyeSlash></EyeSlash>) : (<Eye></Eye>)
              }
            </button>
          </InputGroup.Text>
        </InputGroup>
        {
          vaultItemError || editItemError ? (<Alert variant="danger">{ vaultItemError || editItemError }</Alert>) : null
        }
      </Modal.Body>
      <Modal.Footer>
        {
          isEditMode
            ? (
              <Button
                variant="success"
                onClick={handleOnSave}
                disabled={isLoadingEditItem}
              >Save</Button>
            ) : (
              <Button
                variant="warning"
                onClick={() => setIsEditMode(true)}
              >Edit</Button>
            )
        }
        <Button
          onClick={handleOnHide}
          disabled={isLoadingEditItem}
        >Close</Button>
      </Modal.Footer>
    </Modal>
  );
};

export default ShowItemModal;
