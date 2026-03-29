using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Domain.Interfaces;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, IOrderService orderService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderService = orderService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>List of all orders</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (!orders.Any())
                return NoContent();

            var ordersDto = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
            return Ok(ordersDto);
        }

        /// <summary>
        /// Get order by ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order details</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById([FromRoute] Guid id)
        {
            var order = await _orderRepository.FindByIdAsync(id);
            if (order is null)
                return NotFound(new { message = "Order not found" });

            var orderDto = _mapper.Map<OrderResponseDTO>(order);
            return Ok(orderDto);
        }

        /// <summary>
        /// Get order details by order ID (includes product and user information)
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Detailed order information</returns>
        [HttpGet("{id:guid}/details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails([FromRoute] Guid id)
        {
            var orderDetails = await _orderService.GetOrderDetails(id);
            if (orderDetails is null)
                return NotFound(new { message = "Order not found" });

            return Ok(orderDetails);
        }

        /// <summary>
        /// Get all orders by client ID
        /// </summary>
        /// <param name="clientId">Client ID</param>
        /// <returns>List of orders for the client</returns>
        [HttpGet("client/{clientId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByClientId([FromRoute] Guid clientId)
        {
            var orders = await _orderService.GetOrdersByClientId(clientId);
            if (orders is null || !orders.Any())
                return NoContent();

            return Ok(orders);
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="createOrderDto">Order data</param>
        /// <returns>Created order</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder([FromBody] CreateOrderDTO createOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductId = createOrderDto.ProductId,
                ClientId = createOrderDto.ClientId,
                PurchaseQuantity = createOrderDto.PurchaseQuantity,
                OrderedDate = DateTime.UtcNow
            };

            var createdOrder = await _orderRepository.CreateAsync(order);
            var createdOrderDto = _mapper.Map<OrderResponseDTO>(createdOrder);

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrderDto);
        }

        /// <summary>
        /// Update an existing order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="updateOrderDto">Updated order data</param>
        /// <returns>Updated order</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrder([FromRoute] Guid id, [FromBody] UpdateOrderDTO updateOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingOrder = await _orderRepository.FindByIdAsync(id);
            if (existingOrder is null)
                return NotFound(new { message = "Order not found" });

            existingOrder.ProductId = updateOrderDto.ProductId;
            existingOrder.ClientId = updateOrderDto.ClientId;
            existingOrder.PurchaseQuantity = updateOrderDto.PurchaseQuantity;

            var updatedOrder = await _orderRepository.UpdateAsync(existingOrder);
            var updatedOrderDto = _mapper.Map<OrderResponseDTO>(updatedOrder);

            return Ok(updatedOrderDto);
        }

        /// <summary>
        /// Delete an order
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            var result = await _orderRepository.DeleteAsync(id);
            if (result == 0)
                return NotFound(new { message = "Order not found" });

            return NoContent();
        }
    }
}
