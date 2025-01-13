export const BASE_URL = import.meta.env.VITE_BASE_URL;

export const ACCOUNT_URL = `${BASE_URL}/account`;
export const CREATE_ACCOUNT_URL = `${ACCOUNT_URL}/create`;
export const ACCOUNT_LOGIN_URL = `${ACCOUNT_URL}/login`;
export const ACCOUNT_REFRESH_TOKENS_URL = `${ACCOUNT_URL}/refresh`;

export const VAULT_URL = `${BASE_URL}/vault`;
export const VAULT_ADD_ITEM_URL = VAULT_URL;
