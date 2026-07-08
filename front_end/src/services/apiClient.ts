import axios from 'axios';
import { message } from 'antd';

const apiClient = axios.create({
  baseURL: 'http://localhost:5247/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request Interceptor
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response Interceptor
apiClient.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    const status = error.response ? error.response.status : null;
    const data = error.response ? error.response.data : null;

    let errorMessage = 'Đã xảy ra lỗi kết nối với máy chủ.';

    if (data && data.message) {
      errorMessage = data.message;
    } else if (status === 400) {
      errorMessage = 'Yêu cầu không hợp lệ (400).';
    } else if (status === 401) {
      errorMessage = 'Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.';
      localStorage.removeItem('accessToken');
    } else if (status === 403) {
      errorMessage = 'Bạn không có quyền thực hiện hành động này (403).';
    } else if (status === 404) {
      errorMessage = 'Không tìm thấy tài nguyên yêu cầu (404).';
    } else if (status === 500) {
      errorMessage = 'Lỗi máy chủ nội bộ (500).';
    }

    message.error(errorMessage);
    return Promise.reject(error);
  }
);

export default apiClient;
