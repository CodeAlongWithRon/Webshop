using System;
using System.Threading.Tasks;
using Webshop.BusinessLogic.Exceptions;
using Webshop.BusinessLogic.Orders.Exceptions;
using Webshop.BusinessLogic.Orders.Repositories;

namespace Webshop.BusinessLogic.Orders.Cancel
{
   public class CancelOrderService
   {
      private readonly IOrderRepository _orderRepository;

      public CancelOrderService(IOrderRepository orderRepository)
      {
         _orderRepository = orderRepository;
      }

      public async Task CancelAsync(CancelOrderInputModel inputModel)
      {
         if (inputModel == null)
         {
            throw new ArgumentNullException(nameof(inputModel));
         }

         var order = await _orderRepository.GetByIdAsync(inputModel.OrderId);

         AssertOrderCanBeCancelled(inputModel, order);

         order.Status = OrderStatus.Cancelled;
         await _orderRepository.UpdateAsync(order);
      }

      private static void AssertOrderCanBeCancelled(CancelOrderInputModel inputModel, Order order)
      {
         if (order == null)
         {
            throw new OrderNotFoundException($"No order was found for id {inputModel.OrderId}.");
         }

         if (order.BuyerId != inputModel.UserId)
         {
            throw new UnauthorizedOperationException($"User with id {inputModel.UserId} retrieved order with id {order.Id}, but this order does not belong to him.");
         }

         if (order.Status != OrderStatus.Processing)
         {
            throw new InvalidOrderOperationException($"Order with id {order.Id} has the status {order.Status}. Orders can only be cancelled if the order is still being processed.");
         }
      }
   }
}
