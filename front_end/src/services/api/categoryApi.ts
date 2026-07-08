import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { ApiResponse, Category, CreateCategoryDto } from '../../types';

export const categoryApi = createApi({
  reducerPath: 'categoryApi',
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
  tagTypes: ['Category'],
  endpoints: (builder) => ({
    getCategories: builder.query<ApiResponse<Category[]>, void>({
      query: () => '/category',
      providesTags: ['Category'],
    }),
    getCategoryById: builder.query<ApiResponse<Category>, string>({
      query: (id) => `/category/${id}`,
      providesTags: (_result, _error, id) => [{ type: 'Category', id }],
    }),
    createCategory: builder.mutation<ApiResponse<Category>, CreateCategoryDto>({
      query: (body) => ({
        url: '/category',
        method: 'POST',
        body,
      }),
      invalidatesTags: ['Category'],
    }),
    updateCategory: builder.mutation<ApiResponse<any>, { id: string; body: CreateCategoryDto }>({
      query: ({ id, body }) => ({
        url: `/category/${id}`,
        method: 'PUT',
        body,
      }),
      invalidatesTags: (_result, _error, { id }) => ['Category', { type: 'Category', id }],
    }),
    deleteCategory: builder.mutation<ApiResponse<any>, string>({
      query: (id) => ({
        url: `/category/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: ['Category'],
    }),
  }),
});

export const {
  useGetCategoriesQuery,
  useGetCategoryByIdQuery,
  useCreateCategoryMutation,
  useUpdateCategoryMutation,
  useDeleteCategoryMutation,
} = categoryApi;
