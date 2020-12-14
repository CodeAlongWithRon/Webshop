using System;

namespace Webshop.BusinessLogic.Orders.Exceptions
{
   public class InvalidOrderOperationException : Exception
   {
      public InvalidOrderOperationException()
      {
      }

      public InvalidOrderOperationException(string message) : base(message)
      {
      }

      public InvalidOrderOperationException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}
