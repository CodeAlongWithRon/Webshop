using System.Threading.Tasks;

namespace Webshop.BusinessLogic.Orders.Repositories
{
   public interface IOrderRepository
   {
      Task<Order> GetByIdAsync(long id);

      Task UpdateAsync(Order order);
   }
}
