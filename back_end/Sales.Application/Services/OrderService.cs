using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Sales.Application.DTOs.Order;
using Sales.Application.IServices;
using Sales.Domain.Entities.Customer;
using Sales.Domain.Entities.Order;
using Sales.Domain.Entities.Product;
using Sales.Domain.Enums;
using Sales.Domain.IRepositories;

namespace Sales.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAsync(
                includeProperties: "Customer,OrderDetails.Product,OrderDetails.Unit",
                ignoreQueryFilters: true); // Bypass filter để lịch sử hiển thị cả sản phẩm/khách hàng đã xóa mềm
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.Repository<Order>().FirstOrDefaultAsync(
                filter: o => o.Id == id,
                includeProperties: "Customer,OrderDetails.Product,OrderDetails.Unit",
                ignoreQueryFilters: true); // Bypass filter để chi tiết đơn hàng luôn hiển thị đầy đủ

            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với Id: {id}");
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            if (dto.OrderDetails == null || !dto.OrderDetails.Any())
            {
                throw new ArgumentException("Đơn hàng phải có ít nhất một sản phẩm.");
            }

            // Verify Customer
            var customer = await _unitOfWork.Repository<Customer>().FirstOrDefaultAsync(c => c.Id == dto.CustomerId);
            if (customer == null)
            {
                throw new ArgumentException($"Không tìm thấy khách hàng với Id: {dto.CustomerId}");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Confirmed,
                PaymentStatus = PaymentStatus.Unpaid,
                Notes = dto.Notes,
                CreatedDate = DateTime.UtcNow,
                OrderNumber = $"SO-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}"
            };

            decimal subTotal = 0;
            var details = new List<OrderDetail>();

            foreach (var itemDto in dto.OrderDetails)
            {
                var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(
                    filter: p => p.Id == itemDto.ProductId,
                    includeProperties: "Conversions");
                if (product == null)
                {
                    throw new ArgumentException($"Không tìm thấy sản phẩm với Id: {itemDto.ProductId}");
                }

                // Verify Selected Unit is valid for this product
                decimal conversionRate = 1;
                decimal unitPrice = product.Price;

                if (itemDto.UnitId == product.BaseUnitId)
                {
                    conversionRate = 1;
                    unitPrice = product.Price;
                }
                else
                {
                    var conversion = product.Conversions.FirstOrDefault(c => c.AlternativeUnitId == itemDto.UnitId);
                    if (conversion == null)
                    {
                        throw new ArgumentException($"Đơn vị tính được chọn không hợp lệ cho sản phẩm '{product.Name}'.");
                    }
                    conversionRate = conversion.ConversionRate;
                    unitPrice = conversion.Price ?? (product.Price * conversion.ConversionRate);
                }

                // Verify Unit exists
                var unit = await _unitOfWork.Repository<Unit>().FirstOrDefaultAsync(u => u.Id == itemDto.UnitId);
                if (unit == null)
                {
                    throw new ArgumentException($"Không tìm thấy đơn vị tính với Id: {itemDto.UnitId}");
                }

                int requiredBaseQty = (int)Math.Round(itemDto.Quantity * conversionRate);

                if (product.StockQuantity < requiredBaseQty)
                {
                    throw new InvalidOperationException($"Sản phẩm '{product.Name}' không đủ hàng trong kho (Còn {product.StockQuantity}, yêu cầu {requiredBaseQty} Lon).");
                }

                // Deduct stock
                product.StockQuantity -= requiredBaseQty;
                _unitOfWork.Repository<Product>().Update(product);

                decimal itemSubtotal = itemDto.Quantity * unitPrice;
                decimal itemDiscountAmount = itemSubtotal * (decimal)(itemDto.DiscountPercentage / 100.0);
                decimal itemTotalAmount = itemSubtotal - itemDiscountAmount;

                subTotal += itemTotalAmount;

                var detail = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = itemDto.ProductId,
                    UnitId = itemDto.UnitId,
                    ConversionRate = conversionRate,
                    Quantity = itemDto.Quantity,
                    UnitPrice = unitPrice,
                    DiscountPercentage = itemDto.DiscountPercentage,
                    DiscountAmount = itemDiscountAmount,
                    TotalAmount = itemTotalAmount,
                    Product = product,
                    Unit = unit
                };

                details.Add(detail);
            }

            order.SubTotal = subTotal;
            order.DiscountAmount = 0; // Can be configured further
            order.TaxAmount = subTotal * 0.1m; // 10% VAT
            order.TotalAmount = order.SubTotal - order.DiscountAmount + order.TaxAmount;
            order.OrderDetails = details;
            order.Customer = customer;

            await _unitOfWork.Repository<Order>().InsertAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<bool> CancelOrderAsync(Guid id)
        {
            var order = await _unitOfWork.Repository<Order>().FirstOrDefaultAsync(
                filter: o => o.Id == id,
                includeProperties: "OrderDetails");

            if (order == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với Id: {id}");
            }

            if (order.OrderStatus == OrderStatus.Cancelled)
            {
                throw new InvalidOperationException("Đơn hàng này đã bị hủy từ trước.");
            }

            // Restore product stock
            foreach (var detail in order.OrderDetails)
            {
                var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(p => p.Id == detail.ProductId);
                if (product != null)
                {
                    int restoreQty = (int)Math.Round(detail.Quantity * detail.ConversionRate);
                    product.StockQuantity += restoreQty;
                    _unitOfWork.Repository<Product>().Update(product);
                }
            }

            order.OrderStatus = OrderStatus.Cancelled;
            order.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

