import { CSSProperties, FC, useEffect, useState } from 'react';
import { Alert, Button, Form, InputGroup, Modal } from 'react-bootstrap';
import useFetchVaultGetItem from '../../hooks/api/useFetchVaultGetItem';
import { useLocation, useSearchParams } from 'react-router-dom';
import { Copy, Eye, EyeSlash } from 'react-bootstrap-icons';

type ShowtemModalProps = {
  isOpen: boolean;
  onHide: () => void;
};

const ShowItemModal: FC<ShowtemModalProps> = ({
  isOpen,
  onHide,
}) => {
  const location = useLocation();
  const [searchParams] = useSearchParams();
  const itemId = searchParams.get('show-item') ?? '';

  const [isVisiblePassword, setIsVisiblePassword] = useState<boolean>(false);

  const { error, fetchData, data } = useFetchVaultGetItem({
    id: itemId,
  });

  useEffect(() => {
    if (!itemId) {
      return;
    }
    fetchData();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location]);

  const buttonStyle = (): CSSProperties => ({ marginTop: '-.125rem', padding: 0 });

  const copyToClipboard = async (): Promise<void> => {
    if (!data) {
      return;
    }
    await window.navigator.clipboard.writeText(data.password);
  };

  if (!data) {
    return (<></>);
  }

  return (
    <Modal
      show={isOpen}
      onHide={onHide}
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
          <Form.Control disabled={true} value={data.name} />
        </InputGroup>
        <InputGroup className="mb-3">
          <InputGroup.Text>
            Login
          </InputGroup.Text>
          <Form.Control disabled={true} value={data.login} />
        </InputGroup>
        <InputGroup className="mb-3">
          <InputGroup.Text>
            Password
          </InputGroup.Text>
          <Form.Control
            disabled={true}
            type={isVisiblePassword ? 'text' : 'password'}
            value={data.password}
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
          error ? (<Alert variant="danger">{ error }</Alert>) : null
        }
      </Modal.Body>
      <Modal.Footer>
        <Button
          onClick={onHide}
        >Close</Button>
      </Modal.Footer>
    </Modal>
  );
};

export default ShowItemModal;
