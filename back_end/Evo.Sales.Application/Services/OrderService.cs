using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Evo.Sales.Application.DTOs.Order;
using Evo.Sales.Application.IServices;
using Evo.Sales.Domain.Entities.Customer;
using Evo.Sales.Domain.Entities.Order;
using Evo.Sales.Domain.Entities.Product;
using Evo.Sales.Domain.IRepositories;

namespace Evo.Sales.Application.Services
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
                includeProperties: "Customer,OrderDetails.Product");
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid id)
        {
            var order = await _unitOfWork.Repository<Order>().FirstOrDefaultAsync(
                filter: o => o.Id == id,
                includeProperties: "Customer,OrderDetails.Product");

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
                OrderStatus = 1, // Confirmed
                PaymentStatus = 0, // Unpaid
                Notes = dto.Notes,
                CreatedDate = DateTime.UtcNow,
                OrderNumber = $"SO-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(1000, 9999)}"
            };

            decimal subTotal = 0;
            var details = new List<OrderDetail>();

            foreach (var itemDto in dto.OrderDetails)
            {
                var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(p => p.Id == itemDto.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Không tìm thấy sản phẩm với Id: {itemDto.ProductId}");
                }

                if (product.StockQuantity < itemDto.Quantity)
                {
                    throw new InvalidOperationException($"Sản phẩm '{product.Name}' không đủ hàng trong kho (Còn {product.StockQuantity}, yêu cầu {itemDto.Quantity}).");
                }

                // Deduct stock
                product.StockQuantity -= itemDto.Quantity;
                _unitOfWork.Repository<Product>().Update(product);

                decimal unitPrice = product.Price;
                decimal itemSubtotal = itemDto.Quantity * unitPrice;
                decimal itemDiscountAmount = itemSubtotal * (decimal)(itemDto.DiscountPercentage / 100.0);
                decimal itemTotalAmount = itemSubtotal - itemDiscountAmount;

                subTotal += itemTotalAmount;

                var detail = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = unitPrice,
                    DiscountPercentage = itemDto.DiscountPercentage,
                    DiscountAmount = itemDiscountAmount,
                    TotalAmount = itemTotalAmount,
                    Product = product // Reference for mapping back
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

            if (order.OrderStatus == 3)
            {
                throw new InvalidOperationException("Đơn hàng này đã bị hủy từ trước.");
            }

            // Restore product stock
            foreach (var detail in order.OrderDetails)
            {
                var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(p => p.Id == detail.ProductId);
                if (product != null)
                {
                    product.StockQuantity += detail.Quantity;
                    _unitOfWork.Repository<Product>().Update(product);
                }
            }

            order.OrderStatus = 3; // Cancelled
            order.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
