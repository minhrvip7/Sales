using System;
using System.Collections.Generic;
using System.Linq;

namespace Sales.Application.DTOs.Common
{
    /// <summary>
    /// Response chuẩn trả về dữ liệu phân trang.
    /// </summary>
    public class PagedResponse<T>
    {
        /// <summary>Danh sách dữ liệu của trang hiện tại.</summary>
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

        /// <summary>Tổng số bản ghi thỏa mãn điều kiện tìm kiếm.</summary>
        public int TotalRecords { get; set; }

        /// <summary>Tổng số trang.</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        /// <summary>Trang hiện tại.</summary>
        public int PageNumber { get; set; }

        /// <summary>Số lượng bản ghi trên một trang.</summary>
        public int PageSize { get; set; }

        public PagedResponse(IEnumerable<T> data, int totalRecords, int pageNumber, int pageSize)
        {
            Data = data;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
