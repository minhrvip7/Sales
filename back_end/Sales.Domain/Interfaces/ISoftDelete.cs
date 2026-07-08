using System;

namespace Sales.Domain.Interfaces
{
    /// <summary>
    /// Interface đánh dấu entity hỗ trợ xóa mềm (Soft Delete).
    /// Khi entity implement interface này, thao tác xóa sẽ không xóa bản ghi khỏi DB
    /// mà chỉ set <see cref="IsDeleted"/> = true.
    /// EF Core Global Query Filter sẽ tự động lọc bỏ các bản ghi đã xóa trong mọi query.
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Cờ xóa mềm: <c>true</c> = đã bị xóa (ẩn khỏi các query thông thường),
        /// <c>false</c> = đang hoạt động bình thường.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>Ngày giờ bị xóa mềm (UTC). Null nếu chưa xóa.</summary>
        DateTime? DeletedDate { get; set; }

        /// <summary>ID người dùng thực hiện xóa mềm. Null nếu chưa xóa.</summary>
        Guid? DeletedBy { get; set; }
    }
}
