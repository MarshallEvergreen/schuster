using System;

namespace Schuster.Exceptions
{
    public class ErrorInRunFunctionException : Exception
    {
        public ErrorInRunFunctionException(string message)
            : base(message)
        {
        }   
    }
}