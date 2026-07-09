namespace Sales.Application.DTOs.Common
{
    /// <summary>
    /// Request chuẩn dùng cho phân trang và tìm kiếm cơ bản.
    /// </summary>
    public class PagedRequest
    {
        /// <summary>Trang hiện tại (bắt đầu từ 1).</summary>
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 20;

        /// <summary>Số lượng bản ghi trên một trang (tối đa 100).</summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 100) ? 100 : (value <= 0 ? 20 : value);
        }

        /// <summary>Từ khóa tìm kiếm (tùy chọn).</summary>
        public string? Keyword { get; set; }
    }
}
