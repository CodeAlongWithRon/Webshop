using Moq;
using Moq.AutoMock;
using System.Threading.Tasks;
using Webshop.BusinessLogic.Orders;
using Webshop.BusinessLogic.Orders.Cancel;
using Webshop.BusinessLogic.Orders.Repositories;

namespace BusinessLogic.Tests._Builders.Orders.Cancel
{
   internal class CancelOrderServiceBuilder
   {
      private readonly Mock<IOrderRepository> _orderRepository;
      private readonly CancelOrderService _service;

      public CancelOrderServiceBuilder()
      {
         var mocker = new AutoMocker(MockBehavior.Strict);
         _orderRepository = mocker.GetMock<IOrderRepository>();
         _service = mocker.CreateInstance<CancelOrderService>();
      }

      public CancelOrderService Build()
      {
         return _service;
      }

      public CancelOrderServiceBuilder UpdateOrder(Order order)
      {
         _orderRepository
            .Setup(repository => repository.UpdateAsync(order))
            .Returns(Task.CompletedTask);

         return this;
      }

      public CancelOrderServiceBuilder WithOrderForId(long orderId, Order order)
      {
         _orderRepository
            .Setup(repository => repository.GetByIdAsync(orderId))
            .ReturnsAsync(order);

         return this;
      }

      public CancelOrderServiceBuilder WithoutOrderForId(long orderId)
      {
         Order order = null;

         _orderRepository
            .Setup(repository => repository.GetByIdAsync(orderId))
            .ReturnsAsync(order);

         return this;
      }
   }
}
