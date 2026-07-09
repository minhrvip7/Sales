import React, { useState, useEffect } from 'react';
import { Select, Spin } from 'antd';
import type { SelectProps } from 'antd';

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
        setData(prev => {
          // Prevent duplicates just in case
          const existingIds = new Set(prev.map(item => item[valueProp]));
          const newItems = response.data.filter(item => !existingIds.has(item[valueProp]));
          return [...prev, ...newItems];
        });
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

  useEffect(() => {
    const timer = setTimeout(() => {
      setPage(1);
      setHasMore(true);
      loadData(1, keyword, false);
    }, 300); // debounce search
    
    return () => clearTimeout(timer);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [keyword]);

  const onPopupScroll = (e: React.UIEvent<HTMLDivElement>) => {
    const { target } = e;
    if (target) {
      const { scrollTop, offsetHeight, scrollHeight } = target as HTMLElement;
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
      filterOption={false}
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
