import apiClient from './apiClient';
import type { InventoryBalance, InventoryTransaction } from '../types';

export const inventoryService = {
  getBalances: async (): Promise<InventoryBalance[]> => {
    const response = await apiClient.get<InventoryBalance[]>('/inventory/balances');
    return response.data; 
  },

  getTransactions: async (productId: string): Promise<InventoryTransaction[]> => {
    const response = await apiClient.get<InventoryTransaction[]>(`/inventory/transactions/${productId}`);
    return response.data;
  }
};
