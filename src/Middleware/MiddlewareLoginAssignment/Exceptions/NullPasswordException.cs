namespace MiddlewareLoginAssignment.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException()
        {

        }

        public IncorrectPasswordException(string message) : base(message)
        {

        }

        public IncorrectPasswordException(string message, Exception inner) : base(message, inner)
        {

        }
    }

    public class NullPasswordException : Exception
    {
        public NullPasswordException()
        {

        }

        public NullPasswordException(string message) : base(message)
        {

        }

        public NullPasswordException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
