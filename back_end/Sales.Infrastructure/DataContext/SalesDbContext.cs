using Sales.Domain.Entities.Customer;
using Sales.Domain.Entities.Order;
using Sales.Domain.Entities.Product;
using Sales.Domain.Entities.Inventory;
using Sales.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Sales.Infrastructure.DataContext
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
        {
        }

        public DbSet<CustomerGroup> CustomerGroups { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Unit> Units { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductUnitConversion> ProductUnitConversions { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public DbSet<InventoryBalance> InventoryBalances { get; set; } = null!;
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; } = null!;
        public DbSet<ProductCost> ProductCosts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ──────────────────────────────────────────
            // CustomerGroup Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<CustomerGroup>(entity =>
            {
                entity.ToTable("CustomerGroups", t => t.HasComment("Nhóm khách hàng dùng để phân loại khách hàng theo chính sách giá, chiết khấu."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50).HasComment("Mã nhóm khách hàng nội bộ, duy nhất, tối đa 50 ký tự.");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150).HasComment("Tên nhóm khách hàng, tối đa 150 ký tự.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");
                entity.Property(e => e.DeletedDate).HasComment("Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");
                entity.Property(e => e.DeletedBy).HasComment("ID người dùng thực hiện xóa mềm.");
                entity.HasIndex(e => e.Code).IsUnique();

                // Global Query Filter: tự động lọc bỏ bản ghi đã xóa mềm
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // ──────────────────────────────────────────
            // Customer Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers", t => t.HasComment("Thông tin khách hàng mua hàng trong hệ thống bán lẻ."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50).HasComment("Mã khách hàng nội bộ, duy nhất, tối đa 50 ký tự.");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150).HasComment("Tên khách hàng, tối đa 150 ký tự.");
                entity.Property(e => e.Phone).HasMaxLength(20).HasComment("Số điện thoại liên hệ, tối đa 20 ký tự.");
                entity.Property(e => e.Email).HasMaxLength(100).HasComment("Địa chỉ email liên hệ, tối đa 100 ký tự.");
                entity.Property(e => e.Address).HasMaxLength(250).HasComment("Địa chỉ giao hàng / liên hệ, tối đa 250 ký tự.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");
                entity.Property(e => e.DeletedDate).HasComment("Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");
                entity.Property(e => e.DeletedBy).HasComment("ID người dùng thực hiện xóa mềm.");
                entity.HasIndex(e => e.Code).IsUnique();

                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasOne(d => d.CustomerGroup)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CustomerGroupId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ──────────────────────────────────────────
            // Category Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories", t => t.HasComment("Nhóm hàng (danh mục sản phẩm). Dùng để phân loại sản phẩm theo ngành hàng."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50).HasComment("Mã nhóm hàng nội bộ, duy nhất, tối đa 50 ký tự.");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150).HasComment("Tên nhóm hàng, tối đa 150 ký tự.");
                entity.Property(e => e.Description).HasComment("Mô tả thêm về nhóm hàng (tuỳ chọn).");
                entity.Property(e => e.Status).HasComment("Trạng thái: 1 = đang sử dụng, 0 = đã ẩn/ngừng dùng.");
                entity.Property(e => e.CreatedDate).HasComment("Ngày giờ tạo bản ghi (UTC).");
                entity.Property(e => e.CreatedBy).HasComment("ID người dùng tạo bản ghi.");
                entity.Property(e => e.UpdatedDate).HasComment("Ngày giờ cập nhật gần nhất (UTC).");
                entity.Property(e => e.UpdatedBy).HasComment("ID người dùng cập nhật lần cuối.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");
                entity.Property(e => e.DeletedDate).HasComment("Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");
                entity.Property(e => e.DeletedBy).HasComment("ID người dùng thực hiện xóa mềm.");
                entity.HasIndex(e => e.Code).IsUnique();

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // ──────────────────────────────────────────
            // Unit Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Units", t => t.HasComment("Đơn vị tính dùng trong quản lý sản phẩm và giao dịch. Ví dụ: Cái, Hộp, Thùng, Kg, Lít."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50).HasComment("Mã viết tắt đơn vị tính, duy nhất, tối đa 50 ký tự. Ví dụ: CHI, THG.");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150).HasComment("Tên đơn vị tính. Ví dụ: Chai, Thùng, Kg.");
                entity.Property(e => e.Description).HasComment("Mô tả thêm về đơn vị tính (tuỳ chọn).");
                entity.Property(e => e.Status).HasComment("Trạng thái: 1 = đang sử dụng, 0 = đã ẩn/ngừng dùng.");
                entity.Property(e => e.CreatedDate).HasComment("Ngày giờ tạo bản ghi (UTC).");
                entity.Property(e => e.CreatedBy).HasComment("ID người dùng tạo bản ghi.");
                entity.Property(e => e.UpdatedDate).HasComment("Ngày giờ cập nhật gần nhất (UTC).");
                entity.Property(e => e.UpdatedBy).HasComment("ID người dùng cập nhật lần cuối.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");
                entity.Property(e => e.DeletedDate).HasComment("Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");
                entity.Property(e => e.DeletedBy).HasComment("ID người dùng thực hiện xóa mềm.");
                entity.HasIndex(e => e.Code).IsUnique();

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // ──────────────────────────────────────────
            // Product Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products", t => t.HasComment("Sản phẩm hàng hóa trong hệ thống bán hàng. Mỗi sản phẩm thuộc một nhóm hàng và có một đơn vị tính cơ bản."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50).HasComment("Mã sản phẩm nội bộ, duy nhất, tối đa 50 ký tự.");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150).HasComment("Tên sản phẩm, tối đa 150 ký tự.");
                entity.Property(e => e.Barcode).HasMaxLength(50).HasComment("Mã vạch của đơn vị cơ bản (tuỳ chọn), tối đa 50 ký tự.");
                entity.Property(e => e.Description).HasComment("Mô tả chi tiết hoặc ghi chú về sản phẩm (tuỳ chọn).");
                entity.Property(e => e.Price).HasPrecision(18, 2).HasComment("Giá bán lẻ mặc định theo đơn vị cơ bản (VNĐ).");
                entity.Property(e => e.Cost).HasPrecision(18, 2).HasComment("Giá vốn / chi phí mua vào theo đơn vị cơ bản (VNĐ).");
                entity.Property(e => e.StockQuantity).HasComment("Tồn kho hiện tại tính theo đơn vị cơ bản. Cập nhật tự động khi có giao dịch.");
                entity.Property(e => e.CategoryId).HasComment("FK → Categories. Nhóm hàng của sản phẩm.");
                entity.Property(e => e.BaseUnitId).HasComment("FK → Units. Đơn vị tính cơ bản (ví dụ: Chai, Hộp).");
                entity.Property(e => e.Status).HasComment("Trạng thái: 1 = đang hoạt động, 0 = ngừng kinh doanh.");
                entity.Property(e => e.CreatedDate).HasComment("Ngày giờ tạo bản ghi (UTC).");
                entity.Property(e => e.CreatedBy).HasComment("ID người dùng tạo bản ghi.");
                entity.Property(e => e.UpdatedDate).HasComment("Ngày giờ cập nhật gần nhất (UTC).");
                entity.Property(e => e.UpdatedBy).HasComment("ID người dùng cập nhật lần cuối.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm: true = đã xóa mềm. Dữ liệu vẫn giữ lại để hiển thị trong lịch sử đơn hàng và báo cáo.");
                entity.Property(e => e.DeletedDate).HasComment("Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");
                entity.Property(e => e.DeletedBy).HasComment("ID người dùng thực hiện xóa mềm.");
                entity.HasIndex(e => e.Code).IsUnique();

                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.BaseUnit)
                    .WithMany()
                    .HasForeignKey(d => d.BaseUnitId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ──────────────────────────────────────────
            // ProductUnitConversion Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<ProductUnitConversion>(entity =>
            {
                entity.ToTable("ProductUnitConversions", t => t.HasComment("Cấu hình quy đổi đơn vị tính phụ cho sản phẩm. Ví dụ: 1 Thùng = 12 Chai."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.ProductId).HasComment("FK → Products. Sản phẩm sở hữu bản ghi quy đổi này.");
                entity.Property(e => e.AlternativeUnitId).HasComment("FK → Units. Đơn vị tính phụ (ví dụ: Thùng).");
                entity.Property(e => e.ConversionRate).HasPrecision(18, 4).HasComment("Tỷ lệ quy đổi: số lượng đơn vị cơ bản = 1 đơn vị phụ. Ví dụ: 12 (1 Thùng = 12 Chai).");
                entity.Property(e => e.Barcode).HasMaxLength(50).HasComment("Mã vạch riêng của đơn vị phụ (tuỳ chọn), duy nhất, tối đa 50 ký tự.");
                entity.Property(e => e.Price).HasPrecision(18, 2).HasComment("Giá bán riêng khi bán theo đơn vị phụ (tuỳ chọn, VNĐ). Null = tự tính từ giá cơ bản × ConversionRate.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm: true = đã xóa mềm.");
                entity.Property(e => e.DeletedDate).HasComment("Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");
                entity.Property(e => e.DeletedBy).HasComment("ID người dùng thực hiện xóa mềm.");
                entity.HasIndex(e => e.Barcode).IsUnique();

                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Conversions)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.AlternativeUnit)
                    .WithMany()
                    .HasForeignKey(d => d.AlternativeUnitId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ──────────────────────────────────────────
            // Order Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders", t => t.HasComment("Đơn hàng bán lẻ. Mỗi đơn gắn với một khách hàng và chứa danh sách mặt hàng đã mua."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50).HasComment("Mã số đơn hàng tự sinh, duy nhất. Ví dụ: ORD-20260708-001.");
                entity.Property(e => e.CustomerId).HasComment("FK → Customers. Khách hàng đặt đơn.");
                entity.Property(e => e.OrderDate).HasComment("Ngày giờ đặt hàng (UTC).");
                entity.Property(e => e.SubTotal).HasPrecision(18, 2).HasComment("Tổng tiền hàng trước chiết khấu và thuế (= Σ TotalAmount của các dòng chi tiết).");
                entity.Property(e => e.DiscountAmount).HasPrecision(18, 2).HasComment("Số tiền chiết khấu toàn đơn.");
                entity.Property(e => e.TaxAmount).HasPrecision(18, 2).HasComment("Thuế VAT hoặc các loại thuế khác tính trên đơn hàng.");
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2).HasComment("Thành tiền cuối cùng = SubTotal - DiscountAmount + TaxAmount.");
                entity.Property(e => e.Notes).HasComment("Ghi chú nội bộ hoặc yêu cầu đặc biệt của khách hàng.");
                entity.Property(e => e.OrderStatus)
                    .HasConversion<int>()
                    .HasComment("Trạng thái xử lý đơn hàng: 0=Draft (Nháp), 1=Confirmed (Đã xác nhận), 2=Completed (Hoàn thành), 3=Cancelled (Đã hủy).");
                entity.Property(e => e.PaymentStatus)
                    .HasConversion<int>()
                    .HasComment("Trạng thái thanh toán: 0=Unpaid (Chưa thanh toán), 1=PartiallyPaid (Thanh toán một phần), 2=FullyPaid (Đã thanh toán đủ).");
                entity.Property(e => e.CreatedDate).HasComment("Ngày giờ tạo bản ghi (UTC).");
                entity.Property(e => e.CreatedBy).HasComment("ID người dùng tạo đơn hàng.");
                entity.Property(e => e.UpdatedDate).HasComment("Ngày giờ cập nhật gần nhất (UTC).");
                entity.Property(e => e.UpdatedBy).HasComment("ID người dùng cập nhật lần cuối.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm: true = đã xóa mềm, không hiển thị trong query thông thường.");
                entity.Property(e => e.DeletedDate).HasComment("Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");
                entity.Property(e => e.DeletedBy).HasComment("ID người dùng thực hiện xóa mềm.");
                entity.HasIndex(e => e.OrderNumber).IsUnique();

                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasOne(d => d.Customer)
                    .WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ──────────────────────────────────────────
            // OrderDetail Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails", t => t.HasComment("Chi tiết dòng mặt hàng trong đơn hàng. Mỗi dòng là một sản phẩm ở một đơn vị tính cụ thể."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.OrderId).HasComment("FK → Orders. Đơn hàng cha chứa dòng này.");
                entity.Property(e => e.ProductId).HasComment("FK → Products. Sản phẩm được bán trong dòng này.");
                entity.Property(e => e.UnitId).HasComment("FK → Units. Đơn vị tính được chọn cho dòng này.");
                entity.Property(e => e.ConversionRate).HasPrecision(18, 4).HasComment("Tỷ lệ quy đổi sang đơn vị cơ bản. Mặc định = 1.");
                entity.Property(e => e.Quantity).HasComment("Số lượng bán tính theo đơn vị tính đã chọn.");
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2).HasComment("Đơn giá bán tại thời điểm lập đơn (VNĐ). Chốt cứng, không thay đổi theo giá sản phẩm sau.");
                entity.Property(e => e.DiscountPercentage).HasComment("Phần trăm chiết khấu dòng (0–100).");
                entity.Property(e => e.DiscountAmount).HasPrecision(18, 2).HasComment("Số tiền chiết khấu dòng = UnitPrice × Quantity × DiscountPercentage / 100.");
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2).HasComment("Thành tiền dòng = UnitPrice × Quantity − DiscountAmount.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm: true = dòng đã bị xóa mềm.");
                entity.Property(e => e.DeletedDate).HasComment("Ngày giờ xóa mềm (UTC). Null nếu chưa xóa.");
                entity.Property(e => e.DeletedBy).HasComment("ID người dùng thực hiện xóa mềm.");

                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Unit)
                    .WithMany()
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // ──────────────────────────────────────────
            // InventoryBalance Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<InventoryBalance>(entity =>
            {
                entity.ToTable("InventoryBalances", t => t.HasComment("Bảng số dư tồn kho của sản phẩm, dùng để kiểm soát tồn kho tức thời."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.ProductId).HasComment("FK → Products. Sản phẩm của số dư này.");
                entity.Property(e => e.OnHandQty).HasComment("Số lượng vật lý đang có trong kho.");
                entity.Property(e => e.AllocatedQty).HasComment("Số lượng hàng đã giữ chỗ (Sales Order Confirmed).");
                entity.Property(e => e.AvailableQty).HasComment("Số lượng khả dụng dùng để bán (Available = OnHandQty - AllocatedQty).");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm: true = bản ghi đã bị xóa mềm.");
                
                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ──────────────────────────────────────────
            // InventoryTransaction Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<InventoryTransaction>(entity =>
            {
                entity.ToTable("InventoryTransactions", t => t.HasComment("Bảng thẻ kho ghi nhận lịch sử giao dịch (Nhập, Xuất, Điều chỉnh)."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.ProductId).HasComment("FK → Products.");
                entity.Property(e => e.Type)
                    .HasConversion<int>()
                    .HasComment("Loại giao dịch: 1=Inbound, 2=Outbound, 3=AdjustmentIn, 4=AdjustmentOut, 5=OtherIssue.");
                entity.Property(e => e.ReferenceNumber).HasMaxLength(50).HasComment("Mã chứng từ tham chiếu (VD: PO-123, SO-456).");
                entity.Property(e => e.TransactedQty).HasComment("Số lượng giao dịch theo đơn vị lúc thao tác.");
                entity.Property(e => e.TransactedUomId).HasComment("FK → Units. Đơn vị thao tác.");
                entity.Property(e => e.BaseQty).HasComment("Số lượng quy đổi theo đơn vị cơ bản (cộng/trừ).");
                entity.Property(e => e.Reason).HasMaxLength(500).HasComment("Ghi chú/Lý do.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm.");
                
                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.TransactedUom)
                    .WithMany()
                    .HasForeignKey(d => d.TransactedUomId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ──────────────────────────────────────────
            // ProductCost Configuration
            // ──────────────────────────────────────────
            modelBuilder.Entity<ProductCost>(entity =>
            {
                entity.ToTable("ProductCosts", t => t.HasComment("Lịch sử giá vốn bình quân gia quyền (Moving Average Cost) của sản phẩm."));
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasComment("Khóa chính (UUID).");
                entity.Property(e => e.ProductId).HasComment("FK → Products.");
                entity.Property(e => e.MovingAverageCost).HasPrecision(18, 2).HasComment("Giá vốn bình quân theo đơn vị cơ bản.");
                entity.Property(e => e.EffectiveDate).HasComment("Ngày giờ hiệu lực của mức giá vốn.");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Cờ xóa mềm.");
                
                entity.HasQueryFilter(e => !e.IsDeleted);

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
