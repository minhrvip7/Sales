---
name: virtualized-select
description: Hướng dẫn triển khai Virtualized Select (Infinite Scroll Select) cho Ant Design kết hợp gọi API phân trang. Áp dụng khi hiển thị dropdown có nhiều dữ liệu.
---

# Hướng dẫn triển khai Virtualized Select (Infinite Scroll) trong React + Ant Design

Kỹ năng này hướng dẫn cách tạo một component `Select` có khả năng tải thêm dữ liệu (load more) khi người dùng cuộn xuống dưới, thay vì phải tải toàn bộ dữ liệu (GetAll) ngay từ đầu. Điều này giúp tối ưu hiệu năng frontend và backend.

## 1. Yêu cầu Backend
Backend cần cung cấp một API phân trang (`PagedRequest`) trả về `PagedResponse<T>` chứa `Data`, `PageNumber`, `PageSize`, `TotalPages`. Có hỗ trợ tìm kiếm theo từ khóa (Keyword).

## 2. Triển khai Frontend

Tạo một component dùng chung (ví dụ `PaginatedSelect` hoặc tự code thẳng vào các component) sử dụng `Select` của Ant Design kết hợp sự kiện `onPopupScroll`.

**Mẫu code tham khảo:**

```tsx
import React, { useState, useEffect, useRef } from 'react';
import { Select, Spin } from 'antd';
import type { SelectProps } from 'antd';
// Axios service gọi API backend
// import { fetchPagedData } from '../services/api';

interface PaginatedSelectProps extends Omit<SelectProps, 'options'> {
  fetchData: (page: number, keyword: string) => Promise<{ data: any[]; totalPages: number }>;
  valueProp?: string; // Tên property làm value (mặc định: 'id')
  labelProp?: string; // Tên property làm label (mặc định: 'name')
}

export const PaginatedSelect: React.FC<PaginatedSelectProps> = ({
  fetchData,
  valueProp = 'id',
  labelProp = 'name',
  ...rest
}) => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [keyword, setKeyword] = useState('');

  const loadData = async (currentPage: number, searchKeyword: string, append = false) => {
    if (loading) return;
    setLoading(true);
    try {
      const response = await fetchData(currentPage, searchKeyword);
      if (append) {
        setData(prev => [...prev, ...response.data]);
      } else {
        setData(response.data);
      }
      setHasMore(currentPage < response.totalPages);
    } catch (error) {
      console.error('Lỗi khi fetch dữ liệu select:', error);
    } finally {
      setLoading(false);
    }
  };

  // Lần đầu load dữ liệu hoặc khi keyword thay đổi (debounce nên được bổ sung trong thực tế)
  useEffect(() => {
    setPage(1);
    setHasMore(true);
    loadData(1, keyword, false);
  }, [keyword]);

  const onPopupScroll = (e: React.UIEvent<HTMLDivElement>) => {
    const { target } = e;
    if (target) {
      const { scrollTop, offsetHeight, scrollHeight } = target as HTMLElement;
      // Khi cuộn gần đến cuối (cách 10px) và còn dữ liệu
      if (scrollTop + offsetHeight >= scrollHeight - 10 && hasMore && !loading) {
        const nextPage = page + 1;
        setPage(nextPage);
        loadData(nextPage, keyword, true);
      }
    }
  };

  const onSearch = (val: string) => {
    setKeyword(val);
  };

  const options = data.map(item => ({
    label: item[labelProp],
    value: item[valueProp],
  }));

  return (
    <Select
      showSearch
      onSearch={onSearch}
      onPopupScroll={onPopupScroll}
      options={options}
      filterOption={false} // Tắt filter mặc định của antd vì đã filter từ backend
      notFoundContent={loading ? <Spin size="small" /> : 'Không có dữ liệu'}
      {...rest}
      dropdownRender={(menu) => (
        <>
          {menu}
          {loading && hasMore && (
            <div style={{ textAlign: 'center', padding: 8 }}>
              <Spin size="small" />
            </div>
          )}
        </>
      )}
    />
  );
};
```

## 3. Các lưu ý quan trọng:
- Thuộc tính `filterOption={false}`: Bắt buộc để tránh Ant Design tự ý lọc mất các option vừa load về.
- Xử lý debounce khi gọi hàm `onSearch` để giảm tải số lượng request gửi lên API.
- Luôn giữ state `hasMore` để tránh request gọi liên tục khi đã hết trang cuối cùng.
