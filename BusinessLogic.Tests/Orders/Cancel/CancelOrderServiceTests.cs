using BusinessLogic.Tests._Builders.Orders.Cancel;
using System;
using System.Threading.Tasks;
using Webshop.BusinessLogic.Exceptions;
using Webshop.BusinessLogic.Orders;
using Webshop.BusinessLogic.Orders.Cancel;
using Webshop.BusinessLogic.Orders.Exceptions;
using Xunit;

namespace BusinessLogic.Tests.Orders.Cancel
{
   public class CancelOrderServiceTests
   {
      private const long OrderId = 1;
      private const long UserId = 2;

      [Fact]
      public async Task Cancel_InputModelNull_ThrowsArgumentNullException()
      {
         // Arrange
         CancelOrderInputModel inputModel = null;
         var service = new CancelOrderServiceBuilder().Build();

         // Act / Assert
         await Assert.ThrowsAsync<ArgumentNullException>(nameof(inputModel), () => service.CancelAsync(inputModel));
      }

      [Fact]
      public async Task Cancel_NoOrderForId_ThrowsOrderNotFoundException()
      {
         // Arrange
         var inputModel = CreateInputModel();

         var service = new CancelOrderServiceBuilder()
            .WithoutOrderForId(OrderId)
            .Build();

         // Act / Assert
         var exception = await Assert.ThrowsAsync<OrderNotFoundException>(() => service.CancelAsync(inputModel));

         // Assert
         Assert.Equal($"No order was found for id {inputModel.OrderId}.", exception.Message);
      }

      [Fact]
      public async Task Cancel_UserIdNotMatchingOrderBuyerId_ThrowsUnauthorizedOperationException()
      {
         // Arrange
         var inputModel = CreateInputModel();
         var order = CreateOrderBelongingToAnotherBuyer();

         var service = new CancelOrderServiceBuilder()
            .WithOrderForId(OrderId, order)
            .Build();

         // Act / Assert
         var exception = await Assert.ThrowsAsync<UnauthorizedOperationException>(() => service.CancelAsync(inputModel));

         // Assert
         Assert.Equal($"User with id {inputModel.UserId} retrieved order with id {order.Id}, but this order does not belong to him.", exception.Message);
      }

      [Theory]
      [InlineData(OrderStatus.Cancelled)]
      [InlineData(OrderStatus.Delivered)]
      public async Task Cancel_OrderStatusOtherThanProcessing_ThrowsUnauthorizedOperationException(OrderStatus status)
      {
         // Arrange
         var inputModel = CreateInputModel();
         var order = CreateOwnOrder(status);

         var service = new CancelOrderServiceBuilder()
            .WithOrderForId(OrderId, order)
            .Build();

         // Act / Assert
         var exception = await Assert.ThrowsAsync<InvalidOrderOperationException>(() => service.CancelAsync(inputModel));

         // Assert
         Assert.Equal($"Order with id {order.Id} has the status {order.Status}. Orders can only be cancelled if the order is still being processed.", exception.Message);
      }

      [Fact]
      public async Task Cancel_HappyFlow_CancelsOrder()
      {
         // Arrange
         var inputModel = CreateInputModel();
         var order = CreateOwnOrder(OrderStatus.Processing);

         var service = new CancelOrderServiceBuilder()
            .WithOrderForId(OrderId, order)
            .UpdateOrder(order)
            .Build();

         // Act
         await service.CancelAsync(inputModel);

         // Assert
         Assert.Equal(OrderStatus.Cancelled, order.Status);
      }

      private static CancelOrderInputModel CreateInputModel()
      {
         return new()
         {
            OrderId = OrderId,
            UserId = UserId
         };
      }

      private static Order CreateOrderBelongingToAnotherBuyer()
      {
         return new()
         {
            BuyerId = 3
         };
      }

      private static Order CreateOwnOrder(OrderStatus status)
      {
         return new()
         {
            BuyerId = UserId,
            Status = status
         };
      }
   }
}
