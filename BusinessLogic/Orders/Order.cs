namespace Webshop.BusinessLogic.Orders
{
   public class Order
   {
      public long BuyerId { get; set; }

      public long Id { get; set; }

      public OrderStatus Status { get; set; }
   }
}
