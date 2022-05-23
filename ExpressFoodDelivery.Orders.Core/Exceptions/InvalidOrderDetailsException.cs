using System;

namespace ExpressFoodDelivery.Orders.Core.Exceptions
{
    public class InvalidOrderDetailsException : Exception
    {
        public InvalidOrderDetailsException() : base()
        {

        }

        public InvalidOrderDetailsException(string message) : base(message)
        {
        }
    }
}
