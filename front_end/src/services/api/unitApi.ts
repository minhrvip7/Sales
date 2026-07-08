import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { ApiResponse, Unit, CreateUnitDto } from '../../types';

export const unitApi = createApi({
  reducerPath: 'unitApi',
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
  tagTypes: ['Unit'],
  endpoints: (builder) => ({
    getUnits: builder.query<ApiResponse<Unit[]>, void>({
      query: () => '/unit',
      providesTags: ['Unit'],
    }),
    getUnitById: builder.query<ApiResponse<Unit>, string>({
      query: (id) => `/unit/${id}`,
      providesTags: (_result, _error, id) => [{ type: 'Unit', id }],
    }),
    createUnit: builder.mutation<ApiResponse<Unit>, CreateUnitDto>({
      query: (body) => ({
        url: '/unit',
        method: 'POST',
        body,
      }),
      invalidatesTags: ['Unit'],
    }),
    updateUnit: builder.mutation<ApiResponse<any>, { id: string; body: CreateUnitDto }>({
      query: ({ id, body }) => ({
        url: `/unit/${id}`,
        method: 'PUT',
        body,
      }),
      invalidatesTags: (_result, _error, { id }) => ['Unit', { type: 'Unit', id }],
    }),
    deleteUnit: builder.mutation<ApiResponse<any>, string>({
      query: (id) => ({
        url: `/unit/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Unit'],
    }),
  }),
});

export const {
  useGetUnitsQuery,
  useGetUnitByIdQuery,
  useCreateUnitMutation,
  useUpdateUnitMutation,
  useDeleteUnitMutation,
} = unitApi;
