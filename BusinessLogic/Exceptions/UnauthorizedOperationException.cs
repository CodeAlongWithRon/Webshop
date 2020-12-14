using System;

namespace Webshop.BusinessLogic.Exceptions
{
   public class UnauthorizedOperationException : Exception
   {
      public UnauthorizedOperationException()
      {
      }

      public UnauthorizedOperationException(string message) : base(message)
      {
      }

      public UnauthorizedOperationException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}
