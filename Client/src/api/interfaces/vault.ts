import { VaultItem } from '../../common/interfaces/vault';

export type VaultAddItemRequest = VaultItem;
export type VaultAllItemsResponse = VaultAddItemRequest & {
  id: string;
  userId: string;
  createdAt: string;
};
