import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { ApiResponse, Order, CreateOrderDto, PagedRequest, PagedResponse } from '../../types';

export const orderApi = createApi({
  reducerPath: 'orderApi',
  baseQuery: fetchBaseQuery({
    baseUrl: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api',
    prepareHeaders: (headers) => {
      const token = localStorage.getItem('accessToken');
      if (token) {
        headers.set('Authorization', `Bearer ${token}`);
      }
      return headers;
    }
  }),
  tagTypes: ['Order', 'Product'],
  endpoints: (builder) => ({
    getOrders: builder.query<ApiResponse<PagedResponse<Order>>, PagedRequest | void>({
      query: (params) => ({
        url: '/order',
        params: params || {},
      }),
      providesTags: ['Order'],
    }),
    getOrderById: builder.query<ApiResponse<Order>, string>({
      query: (id) => `/order/${id}`,
      providesTags: (_result, _error, id) => [{ type: 'Order', id }],
    }),
    createOrder: builder.mutation<ApiResponse<Order>, CreateOrderDto>({
      query: (body) => ({
        url: '/order',
        method: 'POST',
        body,
      }),
      invalidatesTags: ['Order', 'Product'],
    }),
    cancelOrder: builder.mutation<ApiResponse<any>, string>({
      query: (id) => ({
        url: `/order/${id}/cancel`,
        method: 'POST',
      }),
      invalidatesTags: ['Order', 'Product'],
    }),
  }),
});

export const {
  useGetOrdersQuery,
  useGetOrderByIdQuery,
  useCreateOrderMutation,
  useCancelOrderMutation,
} = orderApi;
